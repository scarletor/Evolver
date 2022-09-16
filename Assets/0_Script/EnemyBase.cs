using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
public class EnemyBase : CreatureBase
{
    // Start is called before the first frame update

    const string TakeDamageStr = "TakeDamage";
    const string IdleStr = "Idle";
    const string AttackStr = "Attack";
    const string DieStr = "Die";

    string baseName;
    // Update is called once per frame
    public GameObject rigged;
    public bool isFlying;
    public void Start()
    {
        curHPBar.transform.parent.GetComponent<HPBar>().hpText.text = "" + curHP;
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.centerOfMass = Vector3.zero;
        rb.inertiaTensorRotation = Quaternion.identity;
        startPos = transform.position;
        baseName = name;


        InvokeRepeating("CheckTargetIntervals", 1, .3f);
    }



    public void FixedUpdate()
    {
        MoveToPlayerAndAttack();
    }
    public float distanceStopMove, timeToSpecialAttack = 0;
    public void MoveToPlayerAndAttack()
    {

        if (_target == null) return;

        if (isDie == true) return;

        if (_target.transform.root.GetComponent<PlayerController>() != null)
            if (_target.transform.root.GetComponent<PlayerController>().isDie == true)
            {
                _target = null;
                RemoveTarget(PlayerController.ins.gameObject);
                if (targetList.Count == 0)
                    SetState(EnemyState.BackToStartPos);

                if (GetComponent<Enemy_Skeleton>() != null)
                    GetComponent<Enemy_Skeleton>().canMove = true;
                return;
            }




        // special attack
        if (timeToSpecialAttack == 0)
        {
            timeToSpecialAttack = Random.Range(10, 11);
        }

        if (Time.timeSinceLevelLoad % timeToSpecialAttack <= .1f && _state != EnemyState.SpecialAttack)
        {
            timeToSpecialAttack = 0;
            SetState(EnemyState.SpecialAttack);
            Debug.LogError("SPECIAL ATTACK");
        }
        if (_state == EnemyState.SpecialAttack) return;






        if (Vector3.Distance(gameObject.transform.position, _target.transform.position) > distanceStopMove)  //move to player
        {
            var posLook = _target.transform.position;
            if (isFlying) posLook.y = 1.2f;
            gameObject.transform.LookAt(posLook);
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            SetState(EnemyState.MoveToPlayer);
        }
        else //attack
        {
            SetState(EnemyState.Attack);

        }


    }




    public GameObject _target;

    float basePosY;
    public GameObject posToMove;
    public Animator _anim;
    public float rdMove1, rdMove2, rdTime1, rdTime2;
    [Button]
    public virtual void Wandering()
    {
        if (isDie) return;
        if (_target == null) return;
        if (_target.transform.root.GetComponent<PlayerController>().isDie) return;

        var rdX = Random.RandomRange(rdMove1, rdMove2);
        var rdZ = Random.RandomRange(rdMove1, rdMove2);
        var moveTime = Random.Range(rdTime1, rdTime2);
        Vector3 posToMove = new Vector3(rdX, transform.position.y, rdZ);

        transform.DOMove(posToMove, moveTime);
        FaceToPos(posToMove);
        SetState(EnemyState.Idle);
    }

    public void FaceToPos(Vector3 pos)
    {
        Vector3 dir = pos - transform.position;
        gameObject.transform.rotation = Quaternion.LookRotation(dir);
    }
    public GameObject font;



    [Button]
    public void Wandering2()
    {

        if (_target)
            if (_target.transform.root.gameObject.GetComponent<PlayerController>().isDie == false) return;
        if (isDie) return;

        var delay = 0.5f;
        var rot = Random.RandomRange(0, 360);
        transform.DORotate(new Vector3(0, rot, 0), delay);

        Utils.ins.DelayCall(delay, () =>
        {
            if (_target) return;
            transform.DOMove(font.transform.position, 2);
            SetState(EnemyState.Idle);
        });


    }




    public GameObject wanderingPosList;

    public bool isDie;
    public EnemyState _state;
    [Button]
    public void SetState(EnemyState state)
    {
        if (isDie) return;
        switch (state)
        {
            case EnemyState.Idle:
                _anim.SetBool(IdleStr, true);
                _anim.SetBool(AttackStr, false);
                _anim.SetBool("SpecialAttack", false);

                break;
            case EnemyState.Attack:
                _anim.SetBool(AttackStr, true);
                _anim.SetBool(IdleStr, false);
                _anim.SetBool("Move", false);
                _anim.SetBool("TakeDamage", false);
                break;
            case EnemyState.MoveToPlayer:
                _anim.SetBool(AttackStr, false);
                _anim.SetBool(IdleStr, false);
                _anim.SetBool("Move", true);
                break;
            case EnemyState.Die:
                isDie = true;
                _anim.SetBool(DieStr, true);
                curHPBar.transform.parent.gameObject.SetActive(false);
                gameObject.GetComponent<CapsuleCollider>().enabled = false;

                var newGold = Instantiate(Utils.ins.gold);
                newGold.transform.position = gameObject.transform.position;
                newGold.transform.position = new Vector3(newGold.transform.position.x, 0, newGold.transform.position.z);
                break;
            case EnemyState.BackToStartPos:
                return;
                _anim.SetBool(IdleStr, false);
                _anim.SetBool("Move", true);
                _anim.SetBool(AttackStr, false);
                MoveToPosition(startPos);
                break;
            case EnemyState.SpecialAttack:
                _anim.SetBool(AttackStr, false);
                _anim.SetBool("Move", false);
                _anim.SetBool("SpecialAttack", true);



                break;
        }
        _state = state;
        name = baseName + "_#_" + state;

    }

    public GameObject watchingPosList;
    public void FoundPlayer()
    {
        _anim.CrossFade("Move", .1f);
        _target = PlayerController.ins.gameObject;

    }

    public void MoveToPosition(Vector3 pos)
    {
        //slowly lookat noupdate
        GameObject temp = new GameObject();
        temp.transform.position = pos;
        var _direction = (temp.transform.position - transform.position).normalized;
        var rot = Quaternion.LookRotation(_direction);
        transform.DORotateQuaternion(rot, 1);

        //move to pos noupdate
        var distance = Vector3.Distance(transform.position, startPos);
        var time = distance / moveSpeed;
        transform.DOMove(pos, time).OnComplete(() =>
        {
            //if (gameObject.name.Contains("Skeleton"))
            //SetState(EnemyState.Watching);

        });
        _target = null;
    }













    [SerializeField] private float _curHP, _maxHP;
    public float curHP
    {
        get
        {
            return _curHP;
        }
        set
        {
            _curHP = value;
            UpdateHealthBar();
        }

    }


    public GameObject curHPBar;
    public void UpdateHealthBar()
    {
        if (curHP <= 0)
        {
            curHPBar.transform.localScale = new Vector3(0, 1.5f, 1);
            curHPBar.transform.parent.GetComponent<HPBar>().hpText.text = "" + curHP;

            SetState(EnemyState.Die);
            return;
        }

        var scale = -100f;
        if (curHP != 0) scale = _curHP / _maxHP;

        curHPBar.transform.parent.GetComponent<HPBar>().hpText.text = "" + curHP;
        curHPBar.transform.localScale = new Vector3(scale, 1.5f, 1);
    }
    public void TakeDamage(float damage, GameObject dealer)
    {
        if (curHP <= 0)
        {
            isDie = true;
            curHPBar.transform.localScale = new Vector3(0, 1.5f, 1);

            SetState(EnemyState.Die);
            return;
        }

        if (isDie) return;


        var newTextEff = Instantiate(Utils.ins.textEffWhite);
        newTextEff.transform.position = gameObject.transform.position;
        newTextEff.SetValue("" + damage);
        Debug.LogError("MONSTER GET DAMAGE" + damage);
        AddTarget(dealer);
        _anim.SetTrigger("TakeDamage");
        curHP -= damage;
    }




    public List<GameObject> targetList;

    public void CheckTargetIntervals()
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject go in targetList)
        {
            float dist = Vector3.Distance(go.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = go.transform;
                minDist = dist;
            }
        }
        if (tMin != null)
            _target = tMin.gameObject;
    }

    public void AddTarget(GameObject go)
    {
        if (targetList.Contains(go) == false)
            targetList.Add(go);
    }
    public void RemoveTarget(GameObject go)
    {
        if (targetList.Contains(go))
            targetList.Remove(go);
    }







    public Vector3 startPos;








    public enum EnemyState
    {
        Idle,
        Attack,
        MoveToPlayer,
        Die,
        BackToStartPos,
        Wandering,
        SpecialAttack
    }



    public enum EnemyMoveType
    {
        watcher,
        group,
        moveAround

    }

}
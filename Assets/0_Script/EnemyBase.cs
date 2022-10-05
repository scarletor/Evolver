using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
public class EnemyBase : CreatureBase
{
    public int myLevel;
    public GameObject dropItemPref;
    public string dropItemName;
    public int dropGoldMin,dropGoldMax;
    public float skillDamage,dropItemChance;

    // Start is called before the first frame update

    const string TakeDamageStr = "TakeDamage";
    const string IdleStr = "Idle";
    const string AttackStr = "Attack";
    const string DieStr = "Die";

    string baseName;
    // Update is called once per frame
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

    public float distanceStopMove = 1.2f, timeToSkillAttack = 0;

    public void MoveToPlayerAndAttack()
    {

        if (CanAttack() == false) return;


        Debug.LogError("1");

        // special attack
        if (timeToSkillAttack == 0)
        {
            timeToSkillAttack = Random.Range(10, 12);
        }

        if (Time.timeSinceLevelLoad % timeToSkillAttack <= .1f && _state != EnemyState.SpecialAttack)
        {
            timeToSkillAttack = 0;
            SetState(EnemyState.SpecialAttack);
            Debug.LogError("SPECIAL ATTACK");
        }

        if (_state == EnemyState.SpecialAttack) return;



        if (Vector3.Distance(gameObject.transform.position, _target.transform.position) > distanceStopMove)  //move to player
        {
            var posLook = _target.transform.position;
            gameObject.transform.LookAt(posLook);
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            SetState(EnemyState.MoveToPlayer);
        }
        else //attack
        {
            SetState(EnemyState.Attack);

        }


    }


    public bool CanAttack()
    {

        if (_target == null) return false;
        if (isDie == true) return false;


        if (_target.transform.root.GetComponent<PlayerController>() != null)
            if (_target.transform.root.GetComponent<PlayerController>().isDie == true)
            {
                _target = null;
                RemoveTarget(PlayerController.ins.gameObject);

                if (GetComponent<Enemy_Skeleton>() != null)
                    GetComponent<Enemy_Skeleton>().canMove = true;

                if (targetList.Count == 0)
                    SetState(EnemyState.BackToStartPos);
                return false;
            }


        if (_target.GetComponent<PetBase>() != null)
            if (_target.GetComponent<PetBase>().isDie == true)
            {
                RemoveTarget(_target);
                _target = null;

                if (GetComponent<Enemy_Skeleton>() != null)
                    GetComponent<Enemy_Skeleton>().canMove = true;

                if (targetList.Count == 0)
                    SetState(EnemyState.BackToStartPos);
                return false;
            }

        return true;
    }




    public GameObject _target;

    float basePosY;
    public GameObject posToMove;
    [GUIColor(1, 1, 0, 1)]//yellow
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







    [GUIColor(1, 1, 0, .5f)]//yellow
    public GameObject groundToUnlock;

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
                _anim.SetBool("SpecialAttack", false);

                break;
            case EnemyState.MoveToPlayer:
                _anim.SetBool(AttackStr, false);
                _anim.SetBool(IdleStr, false);
                _anim.SetBool("Move", true);
                _anim.SetBool("SpecialAttack", false);

                break;
            case EnemyState.Die:
                if (groundToUnlock != null) groundToUnlock.gameObject.SetActive(true);
                isDie = true;
                _anim.SetBool(DieStr, true);
                curHPBar.transform.parent.gameObject.SetActive(false);
                gameObject.GetComponent<CapsuleCollider>().enabled = false;
                Utils.ins.SpawnGold(gameObject);


                break;
            case EnemyState.BackToStartPos:

                Utils.ins.DelayCall(3, () =>
                {
                    if (isDie) return;
                    _anim.SetBool(IdleStr, false);
                    _anim.SetBool("Move", true);
                    _anim.SetBool(AttackStr, false);
                    MoveToPosition(startPos);
                });
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

    public EnemyMoveType _enemyMoveType;
    public void FoundPlayer()
    {
        _anim.CrossFade("Move", .1f);
        _target = PlayerController.ins.gameObject;
        _enemyMoveType = EnemyMoveType.foundPlayer;
    }

    public void MoveToPosition(Vector3 pos)
    {
        //slowly lookat NO_update
        GameObject temp = new GameObject();
        temp.transform.position = pos;
        var _direction = (temp.transform.position - transform.position).normalized;
        var rot = Quaternion.LookRotation(_direction);
        transform.DORotateQuaternion(rot, 1);

        //move to pos NO_update
        var distance = Vector3.Distance(transform.position, startPos);
        var time = distance / moveSpeed;
        transform.DOMove(pos, time * 0.5f).OnComplete(() =>
          {
              _enemyMoveType = EnemyMoveType.watcher;

          }).SetEase(Ease.Linear);
        _target = null;
    }













    [SerializeField] private float _curHP = 50, _maxHP = 50;
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

    [GUIColor(1, 1, 0, .5f)]//yellow
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
    public void TakeDamage(float damage, GameObject dealer, GameObject textEffPos)
    {
        if (curHP <= 0)
        {

            curHPBar.transform.localScale = new Vector3(0, 1.5f, 1);
            SetState(EnemyState.Die);
            isDie = true;
            return;
        }

        if (isDie) return;


        var newTextEff = Instantiate(Utils.ins.textEffWhite);
        newTextEff.transform.position = textEffPos.transform.position;
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



    public void ComeBack()
    {

        Debug.LogError("Back");

    }

    [Button]
    public void AddEvent()
    {
        EventController.ins.playerDieActions.Add(ComeBack);
    }



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
        foundPlayer,
        watcher,
        group,
        moveAround

    }

}
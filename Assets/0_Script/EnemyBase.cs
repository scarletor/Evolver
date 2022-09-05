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

    // Update is called once per frame
    public GameObject rigged;


    private void Start()
    {
        InvokeRepeating("Wandering2", 1, 5);
    }
    void Update()
    {
        if (canAttack == false) return;
        if (isDie) return;

        MoveToPlayer();
    }
    public bool canAttack;
    public float distanceStopMove;
    public void MoveToPlayer()
    {

        if (_target == null) return;
        if (_target.transform.root.GetComponent<PlayerController>().isDie == true) return;




        if (Vector3.Distance(gameObject.transform.position, _target.transform.position) > distanceStopMove)  //move to player
        {
            var posLook = _target.transform.position;
            posLook.y = 1.2f;
            gameObject.transform.LookAt(posLook);
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            _anim.SetBool(AttackStr, false);
            _anim.SetBool(IdleStr, true);
        }
        else //attack
        {
            _anim.SetBool(AttackStr, true);
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
        ChangeState(EnemyState.Idle);
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
        Debug.LogError("11");

        if (_target)
            if (_target.transform.root.gameObject.GetComponent<PlayerController>().isDie == false) return;
        if (isDie) return;
        Debug.LogError("11");
        Debug.LogError("11");

        var delay = 0.5f;
        var rot = Random.RandomRange(0, 360);
        transform.DORotate(new Vector3(0, rot, 0), delay);

        Utils.ins.DelayCall(delay, () =>
        {
            if (_target) return;
            transform.DOMove(font.transform.position, 2);
            ChangeState(EnemyState.Idle);
        });


    }


    public override void MoveToPosition(GameObject pos)
    {


    }


    public bool isDie;
    public EnemyState _state;
    [Button]
    public void ChangeState(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.Idle:
                _anim.SetBool(IdleStr, true);
                _anim.SetBool(AttackStr, false);
                break;
            case EnemyState.Attack:
                _anim.SetBool(AttackStr, true);
                break;
            case EnemyState.MoveToTarget:
                break;
            case EnemyState.Die:
                _anim.SetTrigger(DieStr);
                break;
            case EnemyState.BackToStartPos:
                _anim.SetBool(IdleStr, true);
                _anim.SetBool(AttackStr, false);
                FaceToPos(startPos.transform.position);
                MoveToPosition(startPos);
                Debug.LogError("BACK");


                break;
        }



        _state = state;

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

            curHPBar.transform.localScale = new Vector3(0, 0.5f, 1);

            ChangeState(EnemyState.Die);
            return;
        }

        var scale = -100f;
        if (curHP != 0) scale = _curHP / _maxHP;

        curHPBar.transform.localScale = new Vector3(scale, 0.5f, 1);
    }
    public void TakeDamage()
    {
        if (curHP <= 0)
        {
            isDie = true;
            curHPBar.transform.localScale = new Vector3(0, 0.5f, 1);

            ChangeState(EnemyState.Die);
            return;
        }

        if (isDie) return;
        Debug.LogError("IS DIE" + isDie + "TAKE DAMAGE");
        _anim.SetTrigger("TakeDamage");
        curHP--;
    }



    public void StopAttackPlayerAndWandering()
    {
        ChangeState(EnemyState.Idle);

    }

    public GameObject startPos;
    public void BackToStartPos()
    {
        ChangeState(EnemyState.Idle);
        Debug.LogError("STOP ATTACK");
    }







    public enum EnemyState
    {
        Idle,
        Attack,
        MoveToTarget,
        Die,
        BackToStartPos,
    }
}
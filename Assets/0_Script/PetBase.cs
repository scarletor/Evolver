using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class PetBase : CreatureBase
{
    // Start is called before the first frame update


    [Button]
    void Start()
    {
        baseName = gameObject.name;
        player = PlayerController.ins;
        Debug.LogError("RUN ME 2");

    }
    string baseName;
    public bool isOwned;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (isOwned == false) return;
        FollowPlayer();
        PetAttack();
    }


    public PlayerController player;
    public GameObject _petTarget;


    public float distanceFollow, speedLookAt;


    public float distanceDisplay;
    public bool canFollowPlayer;

    public override void FollowPlayer()
    {
        distanceDisplay = Vector3.Distance(player.transform.position, transform.position);




        if (distanceDisplay > distanceFollow)
        {
            canFollowPlayer = true;
        }

        if (canFollowPlayer)
        {
            if (_petTarget == null)
                MoveTo(player.gameObject);
            if (Vector3.Distance(player.transform.position, gameObject.transform.position) < 1)
            {
                canFollowPlayer = false;
                ChangeState(PetStateEnum.idle);
            }

        }
    }
    public PetStateEnum petState;
    public Animator _animation;
    public void ChangeState(PetStateEnum state)
    {
        switch (state)
        {
            case PetStateEnum.move:
                _animation.SetTrigger("Move");
                Debug.LogError("MOVE");

                _animation.SetBool("Idle", false);
                _animation.SetBool("Move", true);
                _animation.SetBool("AttackLaser", false);

                break;
            case PetStateEnum.idle:
                _animation.SetTrigger("Idle");


                _animation.SetBool("Idle", true);
                _animation.SetBool("Move", false);
                _animation.SetBool("AttackLaser", false);


                break;
            case PetStateEnum.attackMelee:
                _animation.SetTrigger("petAttack");
                _animation.SetBool("hasTarget", true);

                break;
            case PetStateEnum.followPlayer:

                _animation.SetBool("Idle", false);
                _animation.SetBool("Move", true);
                _animation.SetBool("AttackLaser", false);

                break;

            case PetStateEnum.attackRange:


                break;
            case PetStateEnum.attackLaser:

                _animation.SetBool("Idle", false);
                _animation.SetBool("Move", false);
                _animation.SetBool("AttackLaser", true);


                break;
        }

        petState = state;
        gameObject.name = baseName + "_" + state;

    }
    public float rangeStopMoveToEnemy;

    public virtual void PetAttack()
    {
        _petTarget = player._targetRange;
        if (_petTarget == null) return;
        if (_petTarget.transform.root.GetComponent<EnemyBase>().isDie == true)
        {
            _petTarget = null;
            ChangeState(PetStateEnum.idle);
            return;
        }


        if (Vector3.Distance(_petTarget.transform.position, gameObject.transform.position) < rangeStopMoveToEnemy)
        {
            transform.LookAt(_petTarget.transform);

            if (gameObject.name.Contains("Vekol"))
            {
                ChangeState(PetStateEnum.attackRange);
            }
            else
            {
                ChangeState(PetStateEnum.attackMelee);
            }
        }
        else
        {
            MoveTo(_petTarget);

        }

    }


    public void MoveTo(GameObject target)
    {
        Debug.LogError("PET MOVE");
        ChangeState(PetStateEnum.move);
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        //slowly lookat
        Vector3 relativePos = target.transform.position - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(relativePos);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speedLookAt * Time.deltaTime);
    }


    [Button]
    public void FinishAttackPet()
    {

        if (player._targetRange == null)
        {
            _animation.SetBool("hasTarget", false);
            return;
        }
        else
        {
            _animation.SetBool("hasTarget", true);
        }
        if (petState == PetStateEnum.move) return;
        ChangeState(PetStateEnum.attackMelee);

    }




    [Button]
    public void test()
    {
        _animation.SetTrigger("petAttack");

    }





    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError("TOYUDHJ");
        isOwned = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.LogError("TOYUDHJ");

    }




    public enum PetStateEnum
    {
        move,
        idle,
        attackMelee,
        attackRange,
        followPlayer,
        attackLaser,
        die
    }











    public bool isDie;

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
            curHPBar.transform.parent.GetComponent<HPBar>().hpText.text = "" + 0;
            ChangeState(PetStateEnum.die);
            return;
        }

        var scale = -100f;
        if (curHP != 0) scale = _curHP / _maxHP;

        curHPBar.transform.localScale = new Vector3(scale, 1.5f, 1);
        curHPBar.transform.parent.GetComponent<HPBar>().hpText.text = "" + curHP;
    }
    public void TakeDamage()
    {
        if (curHP <= 0)
        {
            isDie = true;
            curHPBar.transform.localScale = new Vector3(0, 1.5f, 1);

            ChangeState(PetStateEnum.die);
            return;
        }

        if (isDie) return;
        Debug.LogError("IS DIE" + isDie + "TAKE DAMAGE");
        _animation.SetTrigger("TakeDamage");
        curHP -= 10;
    }























}
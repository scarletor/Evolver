using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class PetBase : CreatureBase
{
    // Start is called before the first frame update


    public PetAttackType _petAttackType;

    [Button]
    void Start()
    {
        baseName = gameObject.name;
        player = PlayerController.ins;
        curHPBar.transform.parent.GetComponent<HPBar>().hpText.text = "" + curHP;



    }
    string baseName;
    public bool isOwned;
    // Update is called once per frame
    public void FixedUpdate()
    {
        if (isDie) return;
        if (isOwned == false) return;
        FollowPlayer();
        PetAttackMelee();
        PetAttackRange();
    }


    public PlayerController player;
    public GameObject _petTarget;


    public float distanceFollow, speedLookAt;


    public float distanceDisplay;
    public bool canFollowPlayer;

    public void FollowPlayer()
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
            if (Vector3.Distance(player.transform.position, gameObject.transform.position) < distanceFollow*0.6f)
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
                _animation.SetBool("Idle", false);
                _animation.SetBool("Move", true);
                _animation.SetBool("AttackLaser", false);
                _animation.SetBool("petAttackRange", false);

                break;
            case PetStateEnum.idle:
                _animation.SetBool("petAttackRange", false);
                _animation.SetBool("Idle", true);
                _animation.SetBool("Move", false);
                _animation.SetBool("AttackLaser", false);


                break;
            case PetStateEnum.attackMelee:
                _animation.SetTrigger("petAttack");

                break;
            case PetStateEnum.followPlayer:

                _animation.SetBool("Idle", false);
                _animation.SetBool("Move", true);
                _animation.SetBool("AttackLaser", false);

                break;

            case PetStateEnum.attackRange:
                _animation.SetBool("petAttackRange",true);
                _animation.SetBool("Move", false);
                _animation.SetBool("Idle", false);


                break;
            case PetStateEnum.attackLaser:

                _animation.SetBool("Idle", false);
                _animation.SetBool("Move", false);
                _animation.SetBool("AttackLaser", true);
                break;
            case PetStateEnum.die:
                isDie = true;
                _animation.SetBool("Idle", false);
                _animation.SetBool("Move", false);
                _animation.SetBool("AttackLaser", true);
                _animation.SetBool("Die", true);
                curHPBar.transform.parent.gameObject.SetActive(false);
                
                break;
        }

        petState = state;
        gameObject.name = baseName + "_" + state;

    }
    public float rangeStopMoveToEnemy;

    public virtual void PetAttackMelee()
    {
        if (_petAttackType != PetAttackType.melee) return;
        if (CanPetAttack() == false) return;






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

    public bool CanPetAttack()
    {
        _petTarget = player._targetRange;
        if (_petTarget == null) return false;
        if (_petTarget.transform.root.GetComponent<EnemyBase>().isDie == true)
        {
            _petTarget = null;
            ChangeState(PetStateEnum.idle);
            return false;
        }
        return true;
    }

    public float petAttackRange;
    public void PetAttackRange()
    {
        if (_petAttackType != PetAttackType.ranger) return;
        if (CanPetAttack() == false) return;





        if (Vector3.Distance(_petTarget.transform.position, gameObject.transform.position) < petAttackRange)
        {
            transform.LookAt(_petTarget.transform);
            ChangeState(PetStateEnum.attackRange);
        }
        else
        {
            MoveTo(_petTarget);
        }
    }



    public GameObject muzzlePos;
    public void OnPetPlayAttackRangeAnim()
    {
        if (CanPetAttack() == false) return;
        var newBullet = Instantiate(Utils.ins.yellowBullet);
        newBullet.transform.position = muzzlePos.transform.position;

        var posLook = _petTarget.transform.position;
        posLook.y = 1.2f;
        newBullet.transform.LookAt(posLook + new Vector3(Random.Range(-.2f, .2f), Random.Range(-.2f, -.2f)));
        newBullet.GetComponent<BulletBase>().owner = gameObject;


        var newMuzzle = Instantiate(Utils.ins.yellowMuzzle);
        newMuzzle.transform.position = newBullet.transform.position;
        newMuzzle.transform.rotation = newBullet.transform.rotation;
    }






    public void MoveTo(GameObject target)
    {
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
    public void TakeDamage(float damage)
    {
        if (curHP <= 0)
        {
            isDie = true;
            curHPBar.transform.localScale = new Vector3(0, 1.5f, 1);
            ChangeState(PetStateEnum.die);
            return;
        }

        if (isDie) return;


        var newTextEff = Instantiate(Utils.ins.textEffRed);
        newTextEff.transform.position = transform.position;
        newTextEff.SetValue("" + damage);


        _animation.SetTrigger("TakeDamage");
        curHP -= damage;
    }























}

public enum PetAttackType
{
    melee,
    ranger,
    laser
}
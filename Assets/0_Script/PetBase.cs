using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class PetBase : CreatureBase
{
    // Start is called before the first frame update
    public PetAttackType _petAttackType;

    public GameObject _projectile, muzzle;
    public GameObject myFollowPos;


    [Button]
    public void Start()
    {
        baseName = gameObject.name;
        _player = PlayerController.ins;
        curHPBar.transform.parent.GetComponent<HPBar>().hpText.text = "" + curHP;

        SetupForScene();
        SetupSelf();
    }

    [Button]
    private void SetupSelf()
    {
        _animator = gameObject.GetComponent<Animator>();
        _player = PlayerController.ins;//fix later
        


        if (_animator == null)
        {
            Debug.LogError("RESETUP");
            Utils.ins.DelayCall(1, () =>
            {
                SetupSelf();
            });

        }
    }


    public void SetupForScene()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Lab")
        {
            curHPBar.transform.parent.gameObject.SetActive(false);
            Destroy(this);
        }
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
    [Button]
    public void Test()
    {
        Debug.LogError(PlayerController.ins);
    }

    public PlayerController _player;
    public GameObject _petTarget;


    public float distanceFollow = 2f, speedLookAt = 3f;


    public float distanceDisplay;
    public bool canFollowPlayer;

    public void FollowPlayer()
    {
        distanceDisplay = Vector3.Distance(_player.transform.position, transform.position);
        if (_player == null) _player = PlayerController.ins;



        if (distanceDisplay > distanceFollow)
        {
            canFollowPlayer = true;
        }
        if (Vector3.Distance(_player.transform.position, gameObject.transform.position) < distanceFollow)
        {
            canFollowPlayer = false;
            ChangeState(PetStateEnum.idle);
        }

        if(myFollowPos==null)
        {
            _player.petPosList.ForEach(pos => { 
            
            });
        }


        if (canFollowPlayer)
        {
            if (_petTarget == null)
                MoveTo(_player.gameObject);

        }
    }
    public PetStateEnum petState;
    public Animator _animator;
    public void ChangeState(PetStateEnum state)
    {
        switch (state)
        {
            case PetStateEnum.move:
                _animator.SetBool("Idle", false);
                _animator.SetBool("Move", true);
                _animator.SetBool("petAttackRange", false);

                break;
            case PetStateEnum.idle:
                _animator.SetBool("petAttackRange", false);
                _animator.SetBool("Idle", true);
                _animator.SetBool("Move", false);


                break;
            case PetStateEnum.attackMelee:
                _animator.SetBool("petAttackMelee",true);
                _animator.SetBool("petAttackRange", false);
                _animator.SetBool("Idle", false);
                _animator.SetBool("Move", false);
                break;
            case PetStateEnum.followPlayer:

                _animator.SetBool("Idle", false);
                _animator.SetBool("Move", true);

                break;

            case PetStateEnum.attackRange:
                _animator.SetBool("petAttackRange", true);
                _animator.SetBool("Move", false);
                _animator.SetBool("Idle", false);


                break;
            case PetStateEnum.attackLaser:

                _animator.SetBool("Idle", false);
                _animator.SetBool("Move", false);
                break;
            case PetStateEnum.die:
                isDie = true;
                _animator.SetBool("Idle", false);
                _animator.SetBool("Move", false);
                _animator.SetBool("Die", true);
                curHPBar.transform.parent.gameObject.SetActive(false);

                break;
        }

        petState = state;
        gameObject.name = baseName + "_" + state;

    }
    public float rangeStopMoveToEnemy = 2;

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
        _petTarget = _player._targetRange;
        if (_petTarget == null)
        {
            return false;
        }
        if (_petTarget.transform.root.GetComponent<EnemyBase>().isDie == true)
        {
            _petTarget = null;
            if (petState == PetStateEnum.attackRange || petState == PetStateEnum.attackMelee) ChangeState(PetStateEnum.idle);
            return false;
        }
        return true;
    }

    public float petAttackRange = 7;
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
    public void OnPetAttackMeleeAnimationEvent()
    {

        if (_player._targetRange == null)
        {
            _animator.SetBool("hasTarget", false);
            return;
        }
        else
        {
            _animator.SetBool("hasTarget", true);
        }
        if (petState == PetStateEnum.move) return;
        ChangeState(PetStateEnum.attackMelee);
        _player._targetRange.GetComponent<EnemyBase>().TakeDamage(baseDamage, gameObject,gameObject);
    }




    [Button]
    public void test()
    {
        _animator.SetTrigger("petAttack");

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


        _animator.SetTrigger("TakeDamage");
        curHP -= damage;
    }























}

public enum PetAttackType
{
    melee,
    ranger,
    laser
}
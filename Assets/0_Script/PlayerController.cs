using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class PlayerController : CreatureBase
{
    [SerializeField]
    public static PlayerController ins;
    private void Awake()
    {
        ins = this;
    }


    private void Start()
    {
        UpdateHealthBar();
        InvokeRepeating("SelfRegenarate", 1, 1);
    }
    public PetBase pet;

    private void LateUpdate()
    {
        if (isDie) return;
        MoveByJoyStick();
        ListenInput();
    }

    public GameObject camera;
    public VariableJoystick _joystick;
    public Rigidbody _rigid;
    public bool canMove;
    public float speedMove;
    public Animator _anim;
    public characterStateEnum _playerState;
    public void MoveByJoyStick()
    {



        _rigid.velocity = Vector3.zero;
        _rigid.angularVelocity = Vector3.zero;


        Vector3 dir = _joystick.Direction;
        if (dir != Vector3.zero)
        {
            ChangeState(characterStateEnum.move);
            dir.z = dir.y;
            dir.y = 0;
            dir = dir * speedMove * Time.fixedDeltaTime;


            //gameObject.transform.position += dir;
            //gameObject.transform.rotation = Quaternion.LookRotation(dir);


            Vector3 forward = dir;
            Debug.DrawRay(transform.position, forward, Color.green);


            Vector3 forward2 = Quaternion.Euler(0, CameraFollow.ins.transform.localEulerAngles.y, 0) * forward;

            Debug.DrawRay(transform.position, forward2, Color.red);

            gameObject.transform.position += forward2;
            gameObject.transform.rotation = Quaternion.LookRotation(forward2);


        }
        else
        {
            ChangeState(characterStateEnum.idle);
        }

    }





















    public bool isMoving;

    public void ListenInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            FireBulletIntervals();
        }

        if (Input.GetKey(KeyCode.S))
        {
            ChangeState(characterStateEnum.attackRange);
        }


    }


    [Button]
    public void ChangeState(characterStateEnum state, GameObject target = null)
    {
        if (isDie) return;
        switch (state)
        {

            case characterStateEnum.move:
                _anim.SetBool("isMoving", true);
                break;
            case characterStateEnum.idle:
                _anim.SetBool("isMoving", false);
                _anim.SetBool("hasTarget", false);
                _anim.SetBool("Idle", true);
                _anim.SetBool("attackRange", false);

                break;
            case characterStateEnum.attackRange:
                _targetRange = target;
                _anim.SetBool("hasTarget", true);
                _anim.SetBool("AttackRange", true);



                AttackRange();


                break;
            case characterStateEnum.die:
                _anim.SetBool("isDie", true);
                _anim.SetTrigger("die");
                Rebird();
                isDie = true;
                break;
            case characterStateEnum.fly:
                break;

            case characterStateEnum.attackMelee:
                _targetMelee = target;
                AttackMelee();

                break;
        }

        _playerState = state;
    }


    public void Rebird()
    {
        Utils.ins.DelayCall(3, () =>
        {
            isDie = false;
            _curHP = _maxHP;
            UpdateHealthBar();
            _anim.SetBool("isDie", false);
            _anim.Play("Idle");
            ChangeState(characterStateEnum.idle);
        });
    }












    public GameObject sword, gun, _targetMelee, _targetRange;
    public bool isAttackingMelee;
    public void AttackMelee()
    {
        if (_targetMelee == null) return;
        if (_targetMelee.GetComponent<EnemyBase>().isDie == true)
        {
            _targetMelee = null;
            TargetDie();
            return;
        } 
        _anim.SetTrigger("attackMelee");

        isAttackingMelee = true;
        sword.gameObject.SetActive(true);
        gun.gameObject.SetActive(false);
        _targetRange.gameObject.transform.root.GetComponent<EnemyBase>().TakeDamage(meleeDamage,gameObject);



        var newTextEff = Instantiate(Utils.ins.textEffWhite);
        newTextEff.transform.position = _targetRange.transform.position;
        newTextEff.SetValue("" + meleeDamage);




    }

    public void FinishAttackMeleeAnimEvent()
    {
        AttackMelee();
    }

    public void TargetOutRangeMelee()
    {
        _targetMelee = null;
        isAttackingMelee = false;

    }
    public void FaceToTarget(GameObject target)
    {
        var pos = target.transform.position;
        pos.y = 0;
        transform.LookAt(pos);
    }

    public void TargetDie()
    {
        Debug.LogError("TARGET DIE");
        _anim.SetBool("attackRange", false);
        _anim.SetBool("isMoving", false);
        _anim.SetBool("hasTarget", false);
        _anim.Play("meleeLayer_NoAnim");

    }
 


    public GameObject yellowMuzzle, yellowBullet, yellowImpact, muzzlePos, arrow;
    public void AttackRange()
    {
        if (isAttackingMelee == true) return;
        if (_targetRange == null) return;
        if (_targetRange.transform.GetComponent<EnemyBase>().isDie)
        {
            TargetDie();
            return;
        }


        gun.gameObject.SetActive(true);
        sword.gameObject.SetActive(false);
        _anim.Play("meleeLayer_NoAnim");
        _anim.SetBool("attackRange", true);

        if (_anim.GetBool("isMoving") == false)
            FaceToTarget(_targetRange);


    }



    public void FireBulletIntervals()
    {
        if (isAttackingMelee == true) return;
        if (_targetRange == null) return;
        if (_targetRange.transform.GetComponent<EnemyBase>().isDie) return;


        var newBullet = Instantiate(yellowBullet);
        newBullet.transform.position = muzzlePos.transform.position;
        newBullet.GetComponent<BulletBase>().owner = gameObject;


        var posLook = _targetRange.transform.position;
        posLook.y = 1.2f;
        newBullet.transform.LookAt(posLook + new Vector3(Random.Range(-.2f, .2f), Random.Range(-.2f, -.2f)));

        //var newMuzzle = Instantiate(yellowMuzzle);
        //newMuzzle.transform.position = newBullet.transform.position;
        //newMuzzle.transform.rotation = newBullet.transform.rotation;
    }

    public bool CanAttackTarget()
    {
        if (_targetRange == null) return false;
        if (_targetRange.gameObject.GetComponent<EnemyBase>().isDie)
        {
            _targetRange = null;
            ChangeState(characterStateEnum.idle);
            return false;
        }
        return true;
    }
    public void FinishAttackRange()
    {
        if (CanAttackTarget() == false) return;


        ChangeState(characterStateEnum.attackRange);
        arrow.SetActive(false);
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
            ChangeState(characterStateEnum.die);
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

            ChangeState(characterStateEnum.die);
            return;
        }

        if (isDie) return;

        var newTextEff = Instantiate(Utils.ins.textEffRed);
        newTextEff.transform.position = transform.position;
        newTextEff.SetValue("" + damage);

        _anim.SetTrigger("TakeDamage");
        curHP -= damage;
    }




    public void SelfRegenarate()
    {
        if (_curHP < _maxHP && isDie == false)
            curHP++;
    }

    public void GetHealedByFountain()
    {
        if (_curHP < _maxHP - 10 && isDie == false)
        {
            curHP += 10;
        }
        else if (_curHP > _maxHP - 10 && isDie == false)
        {
            curHP = _maxHP;
        }

    }




}
public enum characterStateEnum
{
    move,
    idle,
    attackRange,
    die,
    fly,
    attackMelee
}
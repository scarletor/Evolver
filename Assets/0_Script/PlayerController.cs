using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerController : CreatureBase
{



    public PetController pet;

    private void FixedUpdate()
    {
        if (isDie) return;
        MoveByJoyStick();
        ListenInput();
    }

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
            gameObject.transform.position += dir;

            gameObject.transform.rotation = Quaternion.LookRotation(dir);
            Debug.LogError(33);
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
                Debug.LogError("MOVEING");
                break;
            case characterStateEnum.idle:
                _anim.SetBool("isMoving", false);
                Debug.LogError("IDLEE");

                break;
            case characterStateEnum.attackRange:
                _targetRange = target;
                _anim.SetBool("hasTarget", true);
                AttackRange();


                break;
            case characterStateEnum.die:
                _anim.SetBool("isDie", true);
                _anim.SetTrigger("die");
                isDie = true;
                break;
            case characterStateEnum.fly:
                break;

            case characterStateEnum.attackMelee:
                _targetMelee = target;
                Debug.LogError(target);
                AttackMelee();

                break;
        }

        _playerState = state;




    }


    public GameObject sword, gun, _targetMelee, _targetRange;
    public bool isAttackingMelee;
    public override void AttackMelee()
    {
        if (_targetMelee == null) return;
        _anim.SetTrigger("attackMelee");
        isAttackingMelee = true;
        sword.gameObject.SetActive(true);
        gun.gameObject.SetActive(false);

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





    public GameObject yellowMuzzle, yellowBullet, yellowImpact, muzzlePos, arrow;
    public override void AttackRange()
    {
        if (isAttackingMelee == true) return;
        if (_targetRange == null) return;
        if (_targetRange.transform.parent.GetComponent<EnemyBase>().isDie)
        {
            _anim.SetBool("attackRange", false);
            _anim.SetBool("hasTarget", false);
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
        if (_targetRange.transform.parent.GetComponent<EnemyBase>().isDie) return;


        var newBullet = Instantiate(yellowBullet);
        newBullet.transform.position = muzzlePos.transform.position;

        var posLook = _targetRange.transform.position;
        posLook.y = 0;
        newBullet.transform.LookAt(_targetRange.transform.parent.position + new Vector3(Random.Range(-.2f, .2f), Random.Range(-.2f, -.2f)));

        var newMuzzle = Instantiate(yellowMuzzle);
        newMuzzle.transform.position = newBullet.transform.position;
        newMuzzle.transform.rotation = newBullet.transform.rotation;
    }
    public void FinishAttackRange()
    {
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

            ChangeState(characterStateEnum.die);
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

            ChangeState(characterStateEnum.die);
            return;
        }

        if (isDie) return;
        Debug.LogError("IS DIE" + isDie + "TAKE DAMAGE");
        _anim.SetTrigger("TakeDamage");
        curHP--;
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
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

    string baseName;
    private void Start()
    {
        InvokeRepeating("SelfRegenarate", 1, 1);
        SetupSelf();
    }

    public void SetupSelf()
    {

        UpdateHealthBar();
        baseName = name;
        //bow 
        var id = PlayerData.ins.GetCurBow();
        ChangeWeapons(id);
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
    public playerStateEnum _playerState;
    public void MoveByJoyStick()
    {



        _rigid.velocity = Vector3.zero;
        _rigid.angularVelocity = Vector3.zero;


        Vector3 dir = _joystick.Direction;
        if (dir != Vector3.zero)
        {
            SetState(playerStateEnum.move);
            dir.z = dir.y;
            dir.y = 0;
            dir = dir * speedMove * Time.fixedDeltaTime;


            //gameObject.transform.position += dir;
            //gameObject.transform.rotation = Quaternion.LookRotation(dir);


            Vector3 forward = dir;


            Vector3 forward2 = Quaternion.Euler(0, CameraFollow.ins.transform.localEulerAngles.y, 0) * forward;


            gameObject.transform.position += forward2;
            gameObject.transform.rotation = Quaternion.LookRotation(forward2);


        }
        else
        {
            if (_targetRange)
            {
                SetState(playerStateEnum.attackRange, _targetRange);
            }
            else
            {
                SetState(playerStateEnum.idle);
            }

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
            SetState(playerStateEnum.attackRange);
        }


    }


    [Button]
    public void SetState(playerStateEnum state, GameObject target = null)
    {
        if (isDie) return;
        switch (state)
        {

            case playerStateEnum.move:
                _anim.SetBool("Move", true);
                _anim.SetBool("Idle", false);
                _anim.SetBool("AttackRange", false);

                break;
            case playerStateEnum.idle:
                _anim.SetBool("Move", false);
                _anim.SetBool("Idle", true);
                _anim.SetBool("AttackRange", false);

                break;
            case playerStateEnum.attackRange:
                if (target == null) return;
                _targetRange = target;
                if (_targetRange.transform.GetComponent<EnemyBase>().isDie)
                {
                    _targetRange = null;
                    return;
                }

                FaceToTarget(_targetRange);
                _anim.SetBool("Move", false);
                _anim.SetBool("Idle", false);
                _anim.SetBool("AttackRange", true);
                arrowRange.gameObject.SetActive(true);





                break;
            case playerStateEnum.die:
                _anim.SetBool("isDie", true);
                _anim.SetTrigger("die");
                break;
            case playerStateEnum.fly:
                break;

        }
        name = baseName + "_#_" + state;
        _playerState = state;
    }


    public void Rebird()
    {
        Utils.ins.DelayCall(7, () =>
        {
            isDie = false;
            curHP = _maxHP;
            _anim.SetBool("isDie", false);
            _anim.Play("Idle");
            SetState(playerStateEnum.idle);
        });
    }












    public GameObject arrowRange;
    [GUIColor(1, 0, 1, 1)]//yellow

    public GameObject _targetRange;
    //public void AttackMelee()
    //{
    //    if (_targetMelee == null) return;
    //    if (_targetMelee.GetComponent<EnemyBase>().isDie == true)
    //    {
    //        _targetMelee = null;
    //        TargetDie();
    //        return;
    //    }
    //    _anim.SetTrigger("attackMelee");

    //    isAttackingMelee = true;
    //    sword.gameObject.SetActive(true);
    //    bow.gameObject.SetActive(false);
    //    arrowRange.gameObject.SetActive(false);
    //    _targetRange.gameObject.transform.root.GetComponent<EnemyBase>().TakeDamage(baseDamage, gameObject,_targetMelee);



    //    var newTextEff = Instantiate(Utils.ins.textEffWhite);
    //    newTextEff.transform.position = _targetRange.transform.position;
    //    newTextEff.SetValue("" + baseDamage);




    //}

    public void FaceToTarget(GameObject target)
    {
        var pos = target.transform.position;
        pos.y = 0;
        transform.LookAt(pos);
    }



    public GameObject yellowMuzzle, yellowBullet, yellowImpact, muzzlePos, arrow;



    public void FireBulletIntervals()
    {
        if (_targetRange == null) return;
        if (_targetRange.transform.GetComponent<EnemyBase>().isDie) return;


        var newBullet = Instantiate(yellowBullet);
        newBullet.transform.position = muzzlePos.transform.position;
        newBullet.GetComponent<BulletBase>().owner = gameObject;
        newBullet.GetComponent<BulletBase>().damage = baseDamage;


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
            SetState(playerStateEnum.idle);
            return false;
        }
        return true;
    }





    public void FinishAttackRange()
    {
        if (CanAttackTarget() == false) return;


        SetState(playerStateEnum.attackRange);
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
            if (_curHP <= 0) PlayerDie();
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
            return;
        }

        var scale = -100f;
        if (curHP != 0) scale = _curHP / _maxHP;

        curHPBar.transform.localScale = new Vector3(scale, 1.5f, 1);
        curHPBar.transform.parent.GetComponent<HPBar>().hpText.text = "" + curHP;
    }
    public void TakeDamage(float damage)
    {

        if (isDie) return;

        var newTextEff = Instantiate(Utils.ins.textEffRed);
        newTextEff.transform.position = transform.position;
        newTextEff.SetValue("" + damage);

        _anim.SetTrigger("TakeDamage");
        curHP -= damage;
    }

    public void PlayerDie()
    {
        Debug.LogError("DIe");
        Rebird();
        SetState(playerStateEnum.die);
        isDie = true;
        EventController.ins.OnPlayerDieActions();
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


    public List<GameObject> petPosList;




    public List<GameObject> allBows;
    public void ChangeWeapons(int weaponId)
    {
        allBows.ForEach(bow => { bow.SetActive(false); });
        allBows[weaponId].SetActive(true);
        baseDamage = PlayerData.ins._bowData[weaponId].damage;
        Debug.LogError("BOW " + weaponId);
    }







}
public enum playerStateEnum
{
    move,
    idle,
    attackRange,
    die,
    fly,
}
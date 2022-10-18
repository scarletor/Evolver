using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using System;
public class PlayerController : CreatureBase
{

    public int level;
    public float totalDamage,randomDamage,animSpeed,attackRange;
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
        var id = PlayerData.ins.GetCurSet();
        ChangeWeapons(id);
        _characterController.detectCollisions = false;
        SetupDataScriptableObject();
    }


    public Data_Player _data;
    public void SetupDataScriptableObject()
    {
        var curLv = PlayerData.ins.GetCurLevel();
		level=curLv;
		
		
        _curHP =float.Parse(_data.dataArray[curLv].HP);
        _maxHP = float.Parse(_data.dataArray[curLv].HP);
        moveSpeed = float.Parse(_data.dataArray[curLv].Movespeed);
        baseDamage = float.Parse(_data.dataArray[curLv].Basedamage);
        randomDamage = float.Parse(_data.dataArray[curLv].Randomdamage);
        attackSpeed = float.Parse(_data.dataArray[curLv].Attackspeed);
        attackRange = float.Parse(_data.dataArray[curLv].Attackrange);
        animSpeed = float.Parse(_data.dataArray[curLv].Animspeed);
    }



    public PetBase pet;

    private void FixedUpdate()
    {
        //MoveByJoyStick();
        //ListenInput();
    }
    private void Update()
    {
        delayAttack += Time.deltaTime;
        MoveByPlayerController();
    }



    public CharacterController _characterController;
    public void MoveByPlayerController()
    {

        if (isDie) return;
        Vector3 dir = _joystick.Direction;

        if (dir != Vector3.zero)
        {
            dir.z = dir.y;
            dir.y = 0;
            Vector3 forward2 = dir * moveSpeed * Time.deltaTime;


            gameObject.transform.rotation = Quaternion.LookRotation(dir);


            SetState(playerStateEnum.move);

            _characterController.Move(forward2);
        }
        else
        {
            if (_targetRange != null)
            {
                SetState(playerStateEnum.attackRange, _targetRange);
            }
            else
            {
                SetState(playerStateEnum.idle);
            }
        }
    }


















    public VariableJoystick _joystick;
    public Rigidbody _rigid;
    public bool canMove;
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
            dir = dir * moveSpeed * Time.fixedDeltaTime;


            //gameObject.transform.position += dir;
            //gameObject.transform.rotation = Quaternion.LookRotation(dir);


            Vector3 forward = dir;
            Vector3 forward2 = Quaternion.Euler(0, CameraFollow.ins.transform.localEulerAngles.y, 0) * forward;
            if (forward2 != Vector3.zero)
            {
                gameObject.transform.position += forward2;
                gameObject.transform.rotation = Quaternion.LookRotation(forward2);
            }



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

    public float delayAttack;
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


                if (delayAttack > attackSpeed)
                {
                    FaceToTarget(_targetRange);
                    _anim.SetBool("Move", false);
                    _anim.SetBool("Idle", false);
                    _anim.SetBool("AttackRange", true);
                    arrowRange.gameObject.SetActive(true);
                    delayAttack = 0;
                }
                else
                {
                    FaceToTarget(_targetRange);
                    _anim.SetBool("Move", false);
                    _anim.SetBool("Idle", true);
                    _anim.SetBool("AttackRange", false);
                }





                break;
            case playerStateEnum.die:
                _anim.SetTrigger("Die");
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
        pos.y = transform.position.y;
        transform.LookAt(pos);
    }



    public GameObject yellowMuzzle, yellowBullet, yellowImpact, muzzlePos, arrow;


    public GameObject cubeTest;
    public void FireBulletIntervals()
    {
        if (_targetRange == null) return;
        if (_targetRange.transform.GetComponent<EnemyBase>().isDie) return;


        var newBullet = Instantiate(yellowBullet);
        newBullet.transform.position = muzzlePos.transform.position;
        newBullet.GetComponent<BulletBase>().owner = gameObject;
        newBullet.GetComponent<BulletBase>().damage = baseDamage;


        var posLook = _targetRange.transform.position;
        posLook.y = muzzlePos.transform.position.y;
        newBullet.transform.LookAt(posLook);

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
            curHPBar.transform.localScale = new Vector3(0, curHPBar.transform.localScale.y, curHPBar.transform.localScale.z);
            curHPBar.transform.parent.GetComponent<HPBar>().hpText.text = "" + 0;
            return;
        }

        var scale = -100f;
        if (curHP != 0) scale = _curHP / _maxHP;

        curHPBar.transform.localScale = new Vector3(scale, curHPBar.transform.localScale.y, curHPBar.transform.localScale.z);
        curHPBar.transform.parent.GetComponent<HPBar>().hpText.text = "" + curHP;
    }
    public void TakeDamage(float damage)
    {

        if (isDie) return;

        var newTextEff = Instantiate(Utils.ins.textEffRed);
        newTextEff.transform.position = transform.position;
        newTextEff.SetValue("" + damage);

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


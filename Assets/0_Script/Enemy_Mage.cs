using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using System;
public class Enemy_Mage : EnemyBase
{

    public Data_Enemy_Mage myData;


     


    private new void Start()
    {
        SetupSelf();
        base.Start();

    }
    public void SetupSelf()
    {
        SetupDataScriptableObject();
    }

    public void SetupDataScriptableObject()
    {
        dropItemName = myData.dataArray[myLevel].Dropitemname;
        try
        {
            curHP = int.Parse(myData.dataArray[myLevel].HP);
        }
        catch (Exception e)
        {
            Debug.LogError("ParseError: " + e);
            curHP = 1;
        }
        try
        {
            baseDamage = int.Parse(myData.dataArray[myLevel].Normaldamage);
        }
        catch (Exception e)
        {
            Debug.LogError("ParseError: " + e);
            baseDamage = 1;
        }

        try
        {
            skillDamage = int.Parse(myData.dataArray[myLevel].Skilldamage);
        }
        catch (Exception e)
        {
            Debug.LogError("ParseError: " + e);
            skillDamage = 1;
        }


        try
        {
            base.timeToSkillAttack = int.Parse(myData.dataArray[myLevel].Castskilleverysecond);
        }
        catch (Exception e)
        {
            Debug.LogError("ParseError: " + e);
            base.timeToSkillAttack = 1;
        }


        try
        {
            attackSpeed = int.Parse(myData.dataArray[myLevel].Attackspeed);
        }
        catch (Exception e)
        {
            Debug.LogError("ParseError: " + e);
            attackSpeed = 1;
        }



        try
        {
            moveSpeed = int.Parse(myData.dataArray[myLevel].Movespeed);
        }
        catch (Exception e)
        {
            Debug.LogError("ParseError: " + e);
            moveSpeed = 1;
        }

        try
        {
            dropGoldMin = int.Parse(myData.dataArray[myLevel].Dropgoldmin);
        }
        catch (Exception e)
        {
            Debug.LogError("ParseError: " + e);
            dropGoldMin = 1;
        }

        try
        {
            dropGoldMax = int.Parse(myData.dataArray[myLevel].Dropgoldmax);
        }
        catch (Exception e)
        {
            Debug.LogError("ParseError: " + e);
            dropGoldMax = 1;
        }




        try
        {
            dropItemChance = int.Parse(myData.dataArray[myLevel].Dropitemchance);
        }
        catch (Exception e)
        {
            Debug.LogError("ParseError: " + e);
            dropItemChance = 1;
        }
        //dropItemPref = null;

    }




    // Update is called once per frame
    public new void FixedUpdate()
    {
        Watching();
        MoveToPlayerAndAttackRange();
    }


    public bool canMove = true;

    [GUIColor(1, 1, 0, 1f)]//yellow
    public GameObject posFaceTo1, posFaceTo2, curPosWatching;

    public GameObject posToMoveWatching;

    [GUIColor(1, 1, 0, .5f)]//yellow
    public List<GameObject> watchingPos;

    public List<GameObject> watchingPosTemp;


    [GUIColor(1, 1, 0, 1f)]//yellow
    public float rotateSpeed = 3;
    [GUIColor(1, 1, 0, 1f)]//yellow
    public float distanceStopPos = 1.3f;//distance stop then attack player
    bool isMoving;


    [GUIColor(1, 1, 0, 1f)]//yellow
    public float delayRotate = 1f;
    public void Watching()
    {
        if (isDie) { return; };
        if (_enemyMoveType == EnemyMoveType.foundPlayer) return;
        if (canMove == false) return;
        if (_target != null) return;

        if (posToMoveWatching == null)
        {
            watchingPosTemp = new List<GameObject>(watchingPos);
            watchingPosTemp.Remove(curPosWatching);
            posToMoveWatching = watchingPosTemp[UnityEngine.Random.Range(0, watchingPosTemp.Count)];
            curPosWatching = posToMoveWatching;
        }

        //slowly lookat pos
        Vector3 relativePos = posToMoveWatching.transform.position - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(relativePos);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotateSpeed * Time.deltaTime);

        //move to pos
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        if (isMoving == false)
        {
            _anim.CrossFade("Move", .1f);
            isMoving = true;
        }


        //finish moving // not in update, call one time
        if (Vector3.Distance(gameObject.transform.position, posToMoveWatching.transform.position) < distanceStopPos)
        {
            canMove = false;
            isMoving = false;


            Debug.LogError("111");

            var rd =UnityEngine.Random.Range(1, 4);
            if (rd == 1) _anim.SetTrigger("Idle1");
            if (rd == 2) _anim.SetTrigger("Idle2");
            if (rd == 3) _anim.SetTrigger("Idle3");



            var _direction = (posFaceTo1.transform.position - transform.position).normalized;
            var rot = Quaternion.LookRotation(_direction);
            transform.DORotateQuaternion(rot, delayRotate);

            Utils.ins.DelayCall(delayRotate + 2, () =>
            {
                if (isDie || _target) return;
                _direction = (posFaceTo2.transform.position - transform.position).normalized;
                rot = Quaternion.LookRotation(_direction);
                transform.DORotateQuaternion(rot, delayRotate);
            });

            Utils.ins.DelayCall(delayRotate * 2 + 2, () =>
            {
                if (isDie || _target) return;
                _direction = (posFaceTo1.transform.position - transform.position).normalized;
                rot = Quaternion.LookRotation(_direction);
                transform.DORotateQuaternion(rot, delayRotate);
            });

            Utils.ins.DelayCall(delayRotate * 3 + 2, () =>
            {
                if (isDie || _target) return;
                _direction = (posFaceTo2.transform.position - transform.position).normalized;
                rot = Quaternion.LookRotation(_direction);
                transform.DORotateQuaternion(rot, delayRotate);
            });


            Utils.ins.DelayCall(delayRotate * 4 + 2, () =>
            {
                if (isDie || _target) return;
                posToMoveWatching = null;
                canMove = true;
                Debug.LogError("FINISH MOVE");
            });
        }
    }



    [GUIColor(1, 1, 0, 1f)]//yellow
    public float  timeSpecialAttack2;
    public void MoveToPlayerAndAttackRange()
    {
        if (CanAttack() == false) return;




        // special attack


        // call once
        if (Time.timeSinceLevelLoad % timeToSkillAttack <= .1f && _state != EnemyState.SpecialAttack)
        {
            SetState(EnemyState.SpecialAttack);//charge

        }

        if (_state == EnemyState.SpecialAttack) return;





        var posLook = _target.transform.position;
        gameObject.transform.LookAt(posLook);

        if (Vector3.Distance(gameObject.transform.position, _target.transform.position) > attackRange)  //move to player
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            SetState(EnemyState.MoveToPlayer);
        }
        else //attack
        {
            if(CanAttack())
            SetState(EnemyState.Attack);

        }

    }



    [GUIColor(1, 1, 0, 1f)]//yellow

    public GameObject muzzlePosMage;
    public void OnAttackNormalAnimationEvent()
    {
        var newMuzzle = Instantiate(muzzle);
        newMuzzle.transform.position = muzzlePosMage.transform.position;
        newMuzzle.transform.rotation = muzzlePosMage.transform.rotation;


        var newProjectile = Instantiate(projectile);
        newProjectile.transform.position = muzzlePosMage.transform.position;
        newProjectile.transform.rotation = muzzlePosMage.transform.rotation;
    }


    [Button]
    public void TestAnim()
    {

    }

    public GameObject frostCharge, frostBigProjectile;
    GameObject _frostCharge;
    public void OnAttackChargeAnimationEvent1()
    {
        _frostCharge = Instantiate(frostCharge);
        _frostCharge.transform.position = muzzlePosMage.transform.position;
        _frostCharge.transform.rotation = muzzlePosMage.transform.rotation;
    }

    public void OnAttackChargeAnimationEvent2()
    {
        _frostCharge.GetComponent<BulletBase>().enabled = true;
        _frostCharge.GetComponent<CapsuleCollider>().enabled = true;

        _anim.SetBool("Attack", false);
        _anim.SetBool("Move", false);
        _anim.SetBool("SpecialAttack", false);

        // finish special attack2

        Utils.ins.DelayCall(.5f, () =>
        {
            var rd =UnityEngine.Random.Range(1, 4);
            if (rd == 1) _anim.SetTrigger("Idle1");
            if (rd == 2) _anim.SetTrigger("Idle2");
            if (rd == 3) _anim.SetTrigger("Idle3");
        });


        Utils.ins.DelayCall(3, () =>
        {
            _anim.CrossFade("Idle",.1f);
            SetState(EnemyState.Idle);
        });

    }

}

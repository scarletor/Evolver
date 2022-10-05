using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Enemy_Skeleton : EnemyBase
{

    public Data_Enemy_Skeleton myData;




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
            timeToSkillAttack = int.Parse(myData.dataArray[myLevel].Castskilleverysecond);
        }
        catch (Exception e)
        {
            Debug.LogError("ParseError: " + e);
            timeToSkillAttack = 1;
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
        base.FixedUpdate();
        Watching();
    }



    public bool canMove = true;

    public GameObject posToMoveWatching, posWatching1, posWatching2, curPosWatching;
    public List<GameObject> watchingPos, watchingPosTemp;
    public float rotateSpeed;
    public float distanceStopPos;
    bool isMoving;
    public float delay1, delay2;
    public bool canContinue;
    public void Watching()
    {
        if (isDie) { return; };
        if (_enemyMoveType == EnemyMoveType.foundPlayer) return;
        if (canMove == false) return;
        if (_target != null) return;
        if (canContinue == false) return;


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


            var _direction = (posWatching1.transform.position - transform.position).normalized;
            var rot = Quaternion.LookRotation(_direction);
            transform.DORotateQuaternion(rot, delay1);


            var rd = UnityEngine.Random.Range(1, 4);
            if (rd == 1) _anim.SetTrigger("Idle1");
            if (rd == 2) _anim.SetTrigger("Idle2");
            if (rd == 3) _anim.SetTrigger("Idle3");



            Utils.ins.DelayCall(delay1 + 2, () =>
              {
                  if (isDie || _target) return;
                  _direction = (posWatching2.transform.position - transform.position).normalized;
                  rot = Quaternion.LookRotation(_direction);
                  transform.DORotateQuaternion(rot, delay1);
              });

            Utils.ins.DelayCall(delay1 * 2 + 2, () =>
                {
                    if (isDie || _target) return;
                    _direction = (posWatching1.transform.position - transform.position).normalized;
                    rot = Quaternion.LookRotation(_direction);
                    transform.DORotateQuaternion(rot, delay1);
                });

            Utils.ins.DelayCall(delay1 * 3 + 2, () =>
            {
                if (isDie || _target) return;
                _direction = (posWatching2.transform.position - transform.position).normalized;
                rot = Quaternion.LookRotation(_direction);
                transform.DORotateQuaternion(rot, delay1);
            });


            Utils.ins.DelayCall(delay1 * 4 + 2, () =>
              {
                  if (isDie || _target) return;
                  posToMoveWatching = null;
                  canMove = true;
                  Debug.LogError("FINISH MOVE");
              });
        }
    }




}

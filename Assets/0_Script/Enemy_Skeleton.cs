using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Enemy_Skeleton : EnemyBase
{







    private void Start()
    {
        base.Start();
        SetState(EnemyState.Watching);

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Watching();
    }



    public bool canMove = true;

    public EnemyMoveType moveType;
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
        if (moveType != EnemyMoveType.watcher) return;
        if (canMove == false) return;
        if (_target != null) return;
        if (_state != EnemyState.Watching) return;
        if (canContinue == false) return;


        if (posToMoveWatching == null)
        {
            watchingPosTemp = new List<GameObject>(watchingPos);
            watchingPosTemp.Remove(curPosWatching);
            posToMoveWatching = watchingPosTemp[Random.Range(0, watchingPosTemp.Count)];
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


            var rd = Random.Range(1, 4);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spider : EnemyBase
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Wandering3();
    }



    public bool canMove;


    public GameObject pos1, pos2, pos3, pos;
    public float  rotateSpeed;
    public float distanceStopPos;
    public void Wandering3()
    {
        if (canMove == false) return;
        if (pos == null) return;
        //slowly rotate to pos

      



        //slowly lookat
        Vector3 relativePos = pos.transform.position - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(relativePos);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotateSpeed * Time.deltaTime);
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        _anim.SetBool("Move", true);
        _anim.SetBool("Idle", false);

        Debug.LogError(Vector3.Distance(gameObject.transform.position, pos.transform.position));
        if (Vector3.Distance(gameObject.transform.position, pos.transform.position) < distanceStopPos)
        {
            Debug.LogError("2222");

            canMove = false;
            _anim.SetBool("Idle", true);
            _anim.SetBool("Move", false);

            var rd = Random.RandomRange(1, 4);
            if (rd == 1) pos = pos1;
            if (rd == 2) pos = pos2;
            if (rd == 3) pos = pos3;

            Utils.ins.DelayCall(1.5f, () =>
            {
                canMove = true;
                Debug.LogError("FINISH MOVE");
            });
        }
    }




}

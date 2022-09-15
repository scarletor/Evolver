using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet_Vekol : PetBase
{

    // Update is called once per frame
    new void FixedUpdate()
    {
        base.FixedUpdate();
        BeamControl();
    }




    public List<RaycastHit> asd;


    public LineRenderer beam1, beam2;
    public GameObject muzzle1, impact1, start1, start2;

    public float textureSpeed;
    public GameObject target1;
    public void BeamControl()
    {
        target1 = PlayerController.ins._targetRange;
        if (target1 == null)
        {
            muzzle1.gameObject.SetActive(false);
            impact1.gameObject.SetActive(false);
            beam1.gameObject.SetActive(false);
            Debug.LogWarning("NO TARGET");
            return;
        }
        if (target1.transform.GetComponent<EnemyBase>().isDie)
        {
            muzzle1.gameObject.SetActive(false);
            impact1.gameObject.SetActive(false);
            beam1.gameObject.SetActive(false);
            Debug.LogWarning("NO TARGET");
            return;
        }
        if (isDie||isOwned==false)
        {
            muzzle1.gameObject.SetActive(false);
            impact1.gameObject.SetActive(false);
            beam1.gameObject.SetActive(false);
            return;
        }



        //RaycastHit[] hits;
        //hits = Physics.RaycastAll(transform.position, transform.forward, 100.0F);

        //var contact = gameObject;
        //for (int i = 0; i < hits.Length; i++)
        //{
        //    if(hits[i].collider.gameObject.transform.root.name.Contains("Enemy"))
        //    {
        //        target1.transform.position = hits[i].collider.ClosestPoint(gameObject.transform.position);
        //    }
        //}



        muzzle1.gameObject.SetActive(true);
        impact1.gameObject.SetActive(true);
        beam1.gameObject.SetActive(true);

        muzzle1.transform.position = start1.transform.position;
        impact1.transform.position = target1.transform.root.position;

        muzzle1.transform.LookAt(target1.transform.position);
        impact1.transform.LookAt(target1.transform.position);

        beam1.positionCount = 2;
        beam1.SetPosition(0, start1.transform.position);
        beam1.SetPosition(1, target1.transform.root.transform.position);


        //RaycastHit hit;
        //if (Physics.Raycast(start, dir, out hit))
        //    end = hit.point - (dir.normalized * beamEndOffset);
        //else
        //    end = transform.position + (dir * 100);

        beam1.gameObject.transform.rotation = Camera.main.transform.rotation;


        float distance = Vector3.Distance(start1.transform.position, target1.transform.position);
        beam1.sharedMaterial.mainTextureScale = new Vector2(distance / 3/*length of texture scale*/, 1);
        beam1.sharedMaterial.mainTextureOffset -= new Vector2(Time.deltaTime * textureSpeed, 0);

        BeamDamageEnemy();
    }




    public float resetTime,attackSpeedLaser;
    public void BeamDamageEnemy()
    {
        resetTime += Time.deltaTime;

        if (resetTime > 1f)
        {
            target1.transform.GetComponent<EnemyBase>().TakeDamage(baseDamage,gameObject);
            Debug.LogError(baseDamage);
            resetTime = 0;
        }
    }




    public override void PetAttackMelee()
    {

        _petTarget = _player._targetRange;
        if (_petTarget == null) return;
        if (_petTarget.transform.root.GetComponent<EnemyBase>().isDie == true)
        {
            _petTarget = null;
            return;
        }
        if (isOwned == false) return;

        transform.LookAt(_petTarget.transform);
        ChangeState(PetStateEnum.attackLaser);

    }




}




















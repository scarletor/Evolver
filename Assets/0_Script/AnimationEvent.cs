using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class AnimationEvent : MonoBehaviour
{

    public PlayerController parent;
    public PetBase parentPet;


    public string animName;
    [Button]
    public void TestAnim()
    {
        gameObject.GetComponent<Animator>().Play(animName);
    }



    public void FinishAttackMeleePlayer()
    {
        parent.FinishAttackMeleeAnimEvent();
    }

    public void FinishAttackRangePlayer()
    {
        parent.FinishAttackRange();
    }

    public void StartAttackRangeAnim()
    {
        parent.FireBulletIntervals();
    }



    public EnemyBase enemy;
    public void EnemyBatLordTakeDamage()
    {
        //enemy.ChangeState(EnemyBase.EnemyState.Move);
        enemy._anim.Play("Idle");
    }


    public void EnemyBatLordAttack()
    {
        //enemy.ChangeState(EnemyBase.EnemyState.Move);
        if (enemy._target == null) return;
        if (enemy._target.transform.root.GetComponent<PlayerController>() != null)
        {
            if (enemy._target.transform.root.GetComponent<PlayerController>().isDie == true)
            {
                Debug.LogError("PLAYER DIE");
                enemy.SetState(EnemyBase.EnemyState.BackToStartPos);
                return;
            }
            enemy._target.transform.root.GetComponent<PlayerController>().TakeDamage(enemy.baseDamage);
        }

        if (enemy._target.transform.root.GetComponent<PetBase>() != null)
        {
            if (enemy._target.transform.root.GetComponent<PetBase>().isDie == true)
            {
                Debug.LogError("PET DIE");

                enemy.targetList.Remove(enemy._target);




                return;
            }
            Debug.LogError("BATLORD attack" +enemy.gameObject.name);

            enemy._target.transform.root.GetComponent<PetBase>().TakeDamage(enemy.baseDamage);
        }
    }


    public void EnemySkeletonAttack()
    {
     


        if (enemy._target == null) return;
        if (enemy._target.transform.root.GetComponent<PlayerController>() != null)
        {
            if (enemy._target.transform.root.GetComponent<PlayerController>().isDie == true)
            {
                if(enemy.targetList.Count==0)
                enemy.SetState(EnemyBase.EnemyState.BackToStartPos);
                enemy.RemoveTarget(PlayerController.ins.gameObject);
                return;
            }
            enemy._target.transform.root.GetComponent<PlayerController>().TakeDamage(enemy.baseDamage);
        }

        if (enemy._target.transform.root.GetComponent<PetBase>() != null)
        {
            if (enemy._target.transform.root.GetComponent<PetBase>().isDie == true)
            {
                enemy.RemoveTarget(enemy._target);
                return;
            }

            enemy._target.transform.root.GetComponent<PetBase>().TakeDamage(enemy.baseDamage);
        }



    }


    public GameObject EnemySkeletonAttackSpecalParticle1, EnemySkeletonAttackSpecalParticle2,invisibleBullet;
    [Button]
    public void EnemySkeletonAttackSpecal1()
    {
        EnemySkeletonAttackSpecalParticle1.gameObject.SetActive(true);




        //var posLook = _targetRange.transform.position;
        //posLook.y = 1.2f;
        //newBullet.transform.LookAt(posLook + new Vector3(Random.Range(-.2f, .2f), Random.Range(-.2f, -.2f)));

    }

    public void EnemySkeletonAttackSpecal2()
    {
        EnemySkeletonAttackSpecalParticle1.gameObject.SetActive(false);
        EnemySkeletonAttackSpecalParticle2.gameObject.SetActive(true);

        var newInviBullet = Instantiate(invisibleBullet);
        newInviBullet.transform.position = gameObject.transform.position;
        newInviBullet.transform.rotation = gameObject.transform.rotation;

    }

    public void EnemySkeletonSpecialAttack_Finish()
    {
        EnemySkeletonAttackSpecalParticle1.gameObject.SetActive(false);
        EnemySkeletonAttackSpecalParticle2.gameObject.SetActive(false);
        enemy.SetState(EnemyBase.EnemyState.Idle);
    }


    public void PetDragonAttackRangeFinish()
    {
        parentPet.OnPetPlayAttackRangeAnim();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class AnimationEvent : MonoBehaviour
{

    public PlayerController parent;
    public PetBase parentPet;

    public GameObject EnemySkeletonAttackSpecalParticle1, EnemySkeletonAttackSpecalParticle2;
    public void EnemySkeletonAttackSpecal1()
    {
        EnemySkeletonAttackSpecalParticle1.gameObject.SetActive(true);
    }

    public void EnemySkeletonAttackSpecal2()
    {
        EnemySkeletonAttackSpecalParticle2.gameObject.SetActive(true);
        EnemySkeletonAttackSpecalParticle1.gameObject.SetActive(false);

    }

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

    public void PetDragonAttackRangeFinish()
    {
        parentPet.OnPetPlayAttackRangeAnim();
    }

}

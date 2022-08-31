using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{

    public PlayerController parent;
    public PetController parentPet;
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


    public void FinishAttackPet()
    {
        parentPet.FinishAttackPet();
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
        Debug.LogError("ATTACL");
        if (enemy._target.transform.root.GetComponent<PlayerController>().isDie == true)
        {
            Debug.LogError("PLAYER DIE");
            enemy.StopAttackPlayerAndWandering();
            return;
        }

        enemy._target.transform.root.GetComponent<PlayerController>().TakeDamage();
    }


}

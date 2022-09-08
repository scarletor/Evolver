using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{




    public PlayerController parent;


    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError("TRIGGER COL");

        if (gameObject.name.Contains("#MeleeCheck"))
        {
            if (other.gameObject.transform.parent.name.Contains("#_1_Enemy"))
            {
                parent.ChangeState(characterStateEnum.attackMelee, other.gameObject);
            }

            if (other.gameObject.transform.root.name.Contains("#_2_Pet"))
            {
                Debug.LogError("PET");
                other.gameObject.transform.root.GetComponent<PetBase>().isOwned = true;
            }


        }

        Debug.LogError(gameObject.name);


        if (gameObject.name.Contains("#GroundCheck")) //expand
        {
            Debug.LogError(gameObject.name);
            if (other.gameObject.name.Contains("#_ExpandCollider"))//expand
            {
                other.transform.parent.gameObject.GetComponent<ExpandGround>().Expand(other.transform.parent.gameObject);

                Debug.LogError(gameObject.name);
            }

            if (other.gameObject.name.Contains("#_4"))//gold
            {
                other.gameObject.SetActive(false);
                UIManager.ins.gold++;
            }


            if (other.gameObject.name.Contains("#_5_GroundObject_Dungeon"))//dungeon
            {
                UIManager.ins.ShowGoDungeonPanel();
            }
            if (other.gameObject.name.Contains("#_5_GroundObject_Lab"))//lab
            {
                UIManager.ins.ShowLabOpenUI();
            }




        }









    }

    public PetBase pet;

    public void OnTriggerStay(Collider other)
    {
        if (gameObject.name.Contains("#RangeCheck"))
        {
            if (other.gameObject.transform.parent.name.Contains("#_1_Enemy"))
            {
                parent.ChangeState(characterStateEnum.attackRange, other.gameObject);
                if (parent.pet != null)
                    parent.pet._petTarget = other.gameObject;
            }

        }




        if (gameObject.name.Contains("#GroundCheck")) //expand
        {
            if (other.gameObject.name.Contains("#_5_GroundObject_Fountain")) //regen
            {

                timeStayInFountain += Time.deltaTime;
                if (timeStayInFountain - 1 > 0)
                {
                    timeStayInFountain = 0;
                    parent.GetHealedByFountain();
                }
            }
        }
    }
    public float timeStayInFountain;





    private void OnTriggerExit(Collider other)
    {
        Debug.LogError("CANCEL");
        if (gameObject.name.Contains("#MeleeCheck"))
        {
            Debug.LogError("CANCEL_ MELEE CHECK");

            if (other.gameObject.transform.parent.name.Contains("#_1_Enemy"))
            {
                parent.TargetOutRangeMelee();

            }

        }


        if (gameObject.name.Contains("#RangeCheck"))
        {
            Debug.LogError("TRIGGER RANGE EXIT ");
            if (other.gameObject.transform.parent.name.Contains("#_1_Enemy"))
            {
                parent._targetRange = null;
                parent._anim.SetBool("hasTarget", false);

            }
        }




        if (gameObject.name.Contains("#GroundCheck")) //expand
        {
            if (other.gameObject.name.Contains("#_5_GroundObject_Dungeon"))//dungeon
            {
                UIManager.ins.CloseDungeonPanel();
            }
            if (other.gameObject.name.Contains("#_5_GroundObject_Lab"))//lab
            {
                UIManager.ins.CloseLabOpenUI();
            }
        }
    }






}

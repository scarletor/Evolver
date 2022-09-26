using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ColliderRefer : MonoBehaviour
{




    public PlayerController parent;


    private void OnTriggerEnter(Collider other)
    {

        if (gameObject.name.Contains("#MeleeCheck"))
        {

            if (other.gameObject.transform.root.name.Contains("#_2_Pet"))
            {
                other.gameObject.transform.root.GetComponent<PetBase>().isOwned = true;
            }


        }



        if (gameObject.name.Contains("#_GoldCheck")) // check gold
        {
            if (other.gameObject.name.Contains("#_4_Gold"))//gold
            {
                other.gameObject.GetComponent<Gold>().StartMoveToPlayer(gameObject);
                UIManager.ins.gold++;
            }
        }



        if (gameObject.name.Contains("#GroundCheck")) //expand
        {
            if (other.gameObject.name.Contains("#_ExpandCollider"))//expand
            {
                other.transform.parent.gameObject.GetComponent<ExpandGround>().Expand(other.transform.parent.gameObject);
            }
            if (other.gameObject.name.Contains("#_5_GroundObject_Dungeon"))//dungeon
            {
                UIManager.ins.ShowGoDungeonPanel();
            }
            if (other.gameObject.name.Contains("#_5_GroundObject_Lab"))//lab
            {
                UIManager.ins.ShowLabOpenUI();
            }

            if (other.gameObject.name.Contains("#_5_GroundObject_PortalIn"))//lab
            {
                UIManager.ins.portalInUI.gameObject.SetActive(true);
            }
            if (other.gameObject.name.Contains("#_5_GroundObject_PortalOut"))//lab
            {
                UIManager.ins.portalOutUI.gameObject.SetActive(true);
            }


        }






    }

    public PetBase pet;

    public void OnTriggerStay(Collider other)
    {
        if (gameObject.name.Contains("#_RangeCheck"))
        {
            if (other.gameObject.transform.name.Contains("#_1_Enemy") && other.gameObject.transform.name.Contains("#_Die") == false)
            {
                parent._targetRange = other.gameObject;
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
        if (gameObject.name.Contains("#MeleeCheck") && gameObject.name.Contains("#_Die") == false)
        {

            if (other.gameObject.transform.name.Contains("#_1_Enemy"))
            {

            }

        }


        if (gameObject.name.Contains("#_RangeCheck"))
        {
            if (other.gameObject.transform.name.Contains("#_1_Enemy"))
            {
                parent._targetRange = null;
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


            if (other.gameObject.name.Contains("#_5_GroundObject_PortalIn"))//lab
            {
                UIManager.ins.portalInUI.gameObject.SetActive(false);
            }
            if (other.gameObject.name.Contains("#_5_GroundObject_PortalOut"))//lab
            {
                UIManager.ins.portalOutUI.gameObject.SetActive(false);
            }





        }
    }






}

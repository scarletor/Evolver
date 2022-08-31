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
                other.gameObject.transform.root.GetComponent<PetController>().isOwned = true;
            }


        }



    }

    public PetController pet;

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
    }






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
                parent.pet._petTarget = null;
                parent.pet._animation.SetBool("hasTarget", false);

            }



        }

    }






}

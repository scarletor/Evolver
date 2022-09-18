using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{








    public float speed, damage;
    public GameObject impactParticle, owner;

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }



    private void OnTriggerEnter(Collider other)
    {

        if (gameObject.name.Contains("#_PlayerBullet"))
        {

            if (other.transform.name.Contains("#_1_Enemy"))
            {
                if (impactParticle)
                {
                    var newMuzzle = Instantiate(impactParticle);
                    newMuzzle.transform.position = gameObject.transform.position;
                }
                other.gameObject.transform.root.GetComponent<EnemyBase>().TakeDamage(damage, owner);

                var enemyTarget = other.gameObject.transform.root.GetComponent<EnemyBase>()._target;
                if (enemyTarget == null)
                {
                    other.gameObject.transform.root.GetComponent<EnemyBase>().FoundPlayer();
                }


                var newTextEff = Instantiate(Utils.ins.textEffWhite);
                newTextEff.transform.position = transform.position;
                newTextEff.SetValue("" + damage);

                Destroy(this.gameObject);

            }



        }




        if (gameObject.name.Contains("#_EnemyBullet"))
        {
            Debug.LogError('1');
            if (gameObject.transform.name.Contains("#_Invisible"))
            {
                Debug.LogError('1');

                if (other.gameObject.name.Contains("#_PlayerCheck"))
                {
                    Debug.LogError('1');
                    other.GetComponent<ColliderRefer>().parent.TakeDamage(damage);
                }
                Debug.LogError('1');


                if (other.transform.name.Contains("#_Pet"))
                {
                    Debug.LogError("PET GET BULLET FIRED");
                    other.GetComponent<ColliderRefer>().pet.TakeDamage(damage);
                }

            }



        }









    }















}

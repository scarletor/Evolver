using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{






    public float speed, damage;
    public GameObject impactParticle, owner;
    public float lifeTime;
    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);


        lifeTime +=Time.fixedDeltaTime;
        if (lifeTime >= 10) Destroy(gameObject);
    }



    private void OnTriggerEnter(Collider other)
    {

        if (gameObject.name.Contains("#_PlayerBullet"))
        {

            if (other.transform.name.Contains("#_1_Enemy"))
            {
                var enemy = other.gameObject.GetComponent<EnemyBase>();
                if (impactParticle)
                {
                    var newMuzzle = Instantiate(impactParticle);
                    newMuzzle.transform.position = gameObject.transform.position;
                }
                enemy.TakeDamage(damage, owner, gameObject);

                var enemyTarget = enemy._target;
                if (enemyTarget == null)
                {
                    enemy.FoundPlayer();
                }



                Destroy(this.gameObject);

            }



        }




        if (gameObject.name.Contains("#_EnemyBullet"))  // enemy
        {
            if (gameObject.transform.name.Contains("#_Invisible"))  // enemy skeleton special attack
            {

                if (other.gameObject.name.Contains("#_PlayerCheck"))
                {
                    Debug.LogError('1');
                    other.GetComponent<ColliderRefer>().parent.TakeDamage(damage);
                }
                Debug.LogError('1');


                if (other.transform.name.Contains("#_Pet"))
                {
                    Debug.LogError("PET GET BULLET FIRED");
                    other.gameObject.transform.parent.GetComponent<PetBase>().TakeDamage(damage);
                }

            }

            // enemy  frost mage 
            if (gameObject.transform.name.Contains("#_FrostMage") && other.gameObject.name.Contains("#_PlayerCheck"))
            {
                other.GetComponent<ColliderRefer>().parent.TakeDamage(damage);


                var newImpact = Instantiate(impactParticle);
                newImpact.transform.position = transform.position;
                Destroy(gameObject);

            }


            if (gameObject.transform.name.Contains("#_FrostMage") && other.gameObject.name.Contains("#_Pet"))
            {
                other.gameObject.transform.parent.GetComponent<PetBase>().TakeDamage(damage);


                var newImpact = Instantiate(impactParticle);
                newImpact.transform.position = transform.position;
                Destroy(gameObject);

            }


        }









    }















}

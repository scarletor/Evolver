using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{








    public float speed, damage;
    public GameObject impactParticle, owner;
    public TextFloatingEff textEff;

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }



    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError("ENTER" + other.transform.name);
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


            Destroy(this.gameObject);

        }

        Debug.LogError("ENTER" + other.transform.name);









        //EnemyBullet

        if (gameObject.name.Contains("#_EnemyBullet"))
        {





            if (other.transform.name.Contains("#_0_Player"))  //invisible bullet
            {
                other.gameObject.transform.root.GetComponent<PlayerController>().TakeDamage(damage);

                Destroy(this.gameObject);



            }

        }

    }















}

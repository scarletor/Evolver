using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{








    public float speed, damage;
    public GameObject impactParticle,owner;
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
            other.gameObject.transform.root.GetComponent<EnemyBase>().TakeDamage(damage,owner);

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















}

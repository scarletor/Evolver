using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{








    public float speed, damage;
    public GameObject impactParticle;
    public TextFloatingEff textEff;

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }



    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError("ENTER" + other.transform.parent.name);
        if (other.transform.parent.name.Contains("#_1_Enemy"))
        {
            var newMuzzle = Instantiate(impactParticle);
            newMuzzle.transform.position = gameObject.transform.position;

            other.gameObject.transform.root.GetComponent<EnemyBase>().TakeDamage(damage);


            var newTextEff = Instantiate(textEff);
            newTextEff.transform.position = transform.position;
            newTextEff.SetValue("" + damage);

            Destroy(this.gameObject);

        }
    }















}

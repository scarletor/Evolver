using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
public class Refer : MonoBehaviour
{




    private void Start()
    {
        InvokeRepeating("SpawnGold", 4, 5);
    }


    public GameObject gold;
    public GameObject pos;



    [Button]
    public void SpawnGold()
    {
        for (int i = 0; i < 5; i++)
        {
            var offset = Random.Range(-.3f, .3f);
            var newGold = Instantiate(gold);
            newGold.transform.position = new Vector3(pos.transform.position.x + offset, pos.transform.position.y + offset, pos.transform.position.z + offset);

        }
    }

}

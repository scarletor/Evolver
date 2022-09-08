using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    public EnemyBase enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.root.name.Contains("Player"))
        {
            Debug.LogError("PLAYER TOUCH ME");
            enemy._target = other.gameObject.transform.root.gameObject;
        }
    }

}


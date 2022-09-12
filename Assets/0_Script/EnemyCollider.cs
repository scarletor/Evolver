using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    public EnemyBase enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (enemy.isDie) return;
        if (other.gameObject.name.Contains("#PlayerCheck"))
        {
            Debug.LogError("PLAYER TOUCH ME");
            enemy.FoundPlayer();
            enemy._target = other.gameObject.transform.root.gameObject;
        }
    }

}


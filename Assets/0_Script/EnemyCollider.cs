using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    public EnemyBase enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (enemy.isDie) return;
        if (other.gameObject.name.Contains("#_PlayerCheck"))
        {
            Debug.LogError("PLAYER TOUCH ME");
            enemy.FoundPlayer();
        }
    }

}


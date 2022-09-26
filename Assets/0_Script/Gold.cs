using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{






    private void FixedUpdate()
    {
        MoveToPlayer();
    }

    public float speed, distanceDestroy;
    public bool canMove = false;
    public float timeDestroy;
    public void MoveToPlayer()
    {
        if (canMove == false) return;
        var pos = _player.transform.position;
        pos.y += 1;
        transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.fixedDeltaTime);

    }

    public GameObject _player;
    public void StartMoveToPlayer(GameObject player)
    {
        canMove = true;
        _player = player;
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Rigidbody>().useGravity = false;
        Utils.ins.DelayCall(timeDestroy, () => {
            Destroy(gameObject);
        });
    }

}

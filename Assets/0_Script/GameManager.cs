using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        //Physics.IgnoreCollision(collision.collider, collider);

    }

    // Update is called once per frame
    void Update()
    {

    }



    public GameObject ground, water;
}

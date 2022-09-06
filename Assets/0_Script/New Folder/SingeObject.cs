using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SingeObject : MonoBehaviour
{


    [SerializeField]
    private float maxDirChangePerSec = 0.1f;

    [SerializeField]
    private float moveSpeed = 2f;

    // Maximum radius for to find flock neighbours
    [SerializeField]
    private float maxFlockRadius = 10f;



    // Update is called once per frame
    void Update()
    {
        MoveStrait();
    }
    public GameObject cubeTest;


    public List<GameObject> cubeList;
    private void MoveStrait()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        Debug.DrawLine(gameObject.transform.position, gameObject.transform.GetChild(1).transform.position, Color.green);


        Debug.DrawLine(cubeList[0].transform.position, cubeList[1].transform.position);
        Debug.DrawLine(cubeList[2].transform.position, cubeList[3].transform.position);


        Debug.DrawLine(cubeList[4].transform.position, cubeList[5].transform.position);
        Debug.DrawLine(cubeList[6].transform.position, cubeList[7].transform.position);

        var a1 = (cubeList[0].transform.position + cubeList[2].transform.position+cubeList[4].transform.position+cubeList[6].transform.position)/4 ;
        var a2 = (cubeList[1].transform.position + cubeList[3].transform.position+cubeList[5].transform.position +cubeList[7].transform.position)/4 ;

        Debug.DrawLine(a1,a2);

    }



    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError("TRIGGER");
    }






}

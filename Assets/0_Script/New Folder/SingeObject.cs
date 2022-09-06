using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SingeObject : MonoBehaviour
{



    [SerializeField]
    private float moveSpeed = 2f;

    [SerializeField]
    private float speedLookAt = 3;


    // Update is called once per frame
    void Update()
    {
        MoveStrait();
    }
    public GameObject cubeTest;

    public GameObject start, end;

    public List<GameObject> cubeList;
    private void MoveStrait()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        Debug.DrawLine(end.transform.position, transform.position, Color.green);

        if (canLookat)
        {
            Vector3 relativePos = Caculator.ins.endGo.transform.position - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speedLookAt * Time.deltaTime);
        }
    }

    public bool canLookat;

    private void OnTriggerEnter(Collider other)
    {
        canLookat = true;
        Debug.LogError(other.gameObject);
        if (Caculator.ins.startPosList.Contains(start) == false)
            Caculator.ins.startPosList.Add(start);
        if (Caculator.ins.endPosList.Contains(end) == false)
            Caculator.ins.endPosList.Add(end);
    }

}

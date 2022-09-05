using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()

    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 smoothPos = Vector3.SmoothDamp(transform.position, target.transform.position + new Vector3(0, 4, 0), ref velocity, smoothSpeed);
        transform.position = smoothPos;
    }



    Vector3 velocity = Vector3.zero;
    public GameObject target;
    public float smoothSpeed;
    public Vector3 offset;








}

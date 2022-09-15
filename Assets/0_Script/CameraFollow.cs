using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow ins;
    private void Awake()
    {
        ins = this;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 smoothPos = Vector3.SmoothDamp(transform.position, PlayerController.ins.gameObject.transform.position + new Vector3(0, 4, 0), ref velocity, smoothSpeed);
        transform.position = smoothPos;
    }



    Vector3 velocity = Vector3.zero;
    public float smoothSpeed;
    public Vector3 offset;








}

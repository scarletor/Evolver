using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public GameObject camera;
    // Update is called once per frame
    void LateUpdate()
    {
        gameObject.transform.LookAt(camera.transform);



    }




}

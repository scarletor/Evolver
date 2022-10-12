using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HPBar : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshPro hpText;
    // Update is called once per frame
    void LateUpdate()
    {
        //transform.rotation = Camera.main.transform.rotation;
        transform.rotation = CameraFollow.ins.transform.rotation;
        //if (player)
        //    //transform.position = Vector3.Lerp(transform.position, player.gameObject.transform.position + basePos, Time.fixedDeltaTime * speed);

        //    //transform.position = Vector3.SmoothDamp(transform.position, PlayerController.ins.gameObject.transform.position + basePos, ref velocity, speed);
        //    transform.position = PlayerController.ins.gameObject.transform.position + basePos;

    }



}

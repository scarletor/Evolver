using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HPBar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        basePos = transform.position;
    }
    public TextMeshPro hpText;
    public Vector3 basePos;
    public PlayerController player;
    public float speed = 3;
    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
        if(player)
        transform.position = Vector3.Lerp(transform.position, player.gameObject.transform.position + basePos, Time.deltaTime * speed);
    }




}

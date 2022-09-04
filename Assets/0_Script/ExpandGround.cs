using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ExpandGround : MonoBehaviour
{













    public GameObject expandGround;
    public float valueToExpand;


    public void Expand()
    {
        expandGround.gameObject.SetActive(true);
        expandGround.transform.localScale = Vector3.zero;
        expandGround.transform.DOScale(1, 1);
    }






}

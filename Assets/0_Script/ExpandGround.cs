using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class ExpandGround : MonoBehaviour
{







    private void OnEnable()
    {
        ExpandMe();
    }

    public void ExpandMe()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(new Vector3(1, 0.01f, 1), 1);
    }



    public GameObject expandGround;
    public int tileToExpand, goldToExpand;


    public void Expand(GameObject expandSign)
    {
        if (UIManager.ins.tileUnlocked >= tileToExpand && expandType == ExpandGroundEnum.tile)
        {
            expandGround.gameObject.SetActive(true);
            expandGround.transform.localScale = Vector3.zero;
            expandGround.transform.DOScale(1, 1);
            UIManager.ins.tileUnlocked++;
            UIManager.ins.tileUnlocked -= tileToExpand;
            expandSign.SetActive(false);
        }

        if (UIManager.ins.gold >= goldToExpand && expandType == ExpandGroundEnum.gold)
        {
            expandGround.gameObject.SetActive(true);
            expandGround.transform.localScale = Vector3.zero;
            expandGround.transform.DOScale(1, 1);
            UIManager.ins.tileUnlocked++;
            UIManager.ins.gold -= goldToExpand;
            expandSign.SetActive(false);
        }



    }

    public TextMeshPro text;
    public ExpandGroundEnum expandType;
    private void Start()
    {
        if (expandType == ExpandGroundEnum.gold) text.text = "" + goldToExpand;
        if (expandType == ExpandGroundEnum.tile) text.text = "" + tileToExpand;
    }



}
public enum ExpandGroundEnum
{
    none,
    gold,
    tile
}
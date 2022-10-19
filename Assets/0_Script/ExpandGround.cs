using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Sirenix.OdinInspector;
public class ExpandGround : MonoBehaviour
{







    private void Start()
    {
        SetupSelf();
    }

    public void SetupSelf()
    {
        if (groundToExpand.Count != 0)
            baseScale = groundToExpand[0].transform.localScale;

        Utils.ins.DelayCall(0.01f, () =>
        {
            groundToExpand.ForEach(go =>
            {
                go.gameObject.SetActive(false);
                go.gameObject.isStatic = false;
                Debug.LogError(go);
            });

        });


        groundToExpand.ForEach(go =>
        {
            go.transform.DOMoveY(-20, .1f);
        });

        text.text = "" + goldToExpand;
        gameObject.transform.Find("@_#_ExpandCollider").gameObject.layer = 13;


    }



    public List<GameObject> groundToExpand;
    public int goldToExpand;

    [Button]
    public void Expand()
    {
        //if (UIManager.ins.gold >= goldToExpand)
        if (true)
        {
            groundToExpand.ForEach(go =>
            {
                go.gameObject.SetActive(true);
                go.transform.DOLocalMoveY(0, 2).SetEase(Ease.OutBack);
            });


            UIManager.ins.gold -= goldToExpand;
            gameObject.SetActive(false);
        }

        groundToExpand.ForEach(go => { go.gameObject.SetActive(true); });


        UIManager.ins.gold -= goldToExpand;
    }
    Vector3 baseScale;
    public TextMeshPro text;




}
public enum ExpandGroundEnum
{
    none,
    gold,
    tile
}
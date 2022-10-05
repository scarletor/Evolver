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
        Setup();
    }

    public void Setup()
    {
        baseScale = groundToExpand.transform.localScale;
        groundToExpand.gameObject.SetActive(false);
        groundToExpand.transform.DOMoveY(-20, .1f);
        text.text = "" + goldToExpand;
    }



    public GameObject groundToExpand;
    public int tileToExpand, goldToExpand;

    [Button]
    public void Expand()
    {
        //if (UIManager.ins.gold >= goldToExpand)
        //{
        //    groundToExpand.gameObject.SetActive(true);
        //    groundToExpand.transform.DOLocalMoveY(-5, 2).SetEase(Ease.OutBack).OnComplete(() =>
        //    {
        //        EnableEnemy();
        //    });
        //    UIManager.ins.gold -= goldToExpand;
        //    gameObject.SetActive(false);
        //}

        groundToExpand.gameObject.SetActive(true);
        groundToExpand.transform.DOLocalMoveY(-5, 2).SetEase(Ease.OutBack).OnComplete(() =>
        {
            EnableEnemy();
        });
        UIManager.ins.gold -= goldToExpand;
        gameObject.SetActive(false);

    }
    Vector3 baseScale;
    public TextMeshPro text;


    public void EnableEnemy()
    {
        foreach (Transform child in groundToExpand.transform)
        {
            if (child.gameObject.name.Contains("Enemy"))
            {
                Debug.LogError(1);
                child.GetComponent<EnemyBase>().enabled = true;
                child.GetComponent<CapsuleCollider>().enabled = true;
                child.GetComponent<Animator>().enabled = true;


            }
        }
    }


}
public enum ExpandGroundEnum
{
    none,
    gold,
    tile
}
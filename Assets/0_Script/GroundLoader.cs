using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
public class GroundLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {


        Application.targetFrameRate = 60;




        //InvokeRepeating("ScaleAll", 1, 1);

        //Utils.ins.DelayCall(30, () =>
        //{

        //    CancelInvoke("ScaleAll");
        //});
    }

    public void ScaleAll()
    {
        //shadows.ForEach(sprite => {
        //    sprite.gameObject.transform.DOScaleY(UnityEngine.Random.Range(1,5), 1);
        //})
        gameObject.transform.DOScaleY(UnityEngine.Random.Range(1, 5), 1);

    }









    public List<SpriteRenderer> shadows;
}

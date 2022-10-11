using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class WaterController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RandomMoveY();
    }

    // Update is called once per frame
    void Update()
    {

    }



    public float offset1, offset2;





    public void RandomMoveX()
    {
        gameObject.transform.DOMoveX(Random.Range(-offset1, offset2), 4).OnComplete(() => { RandomMoveX(); });
    }

    public void RandomMoveZ()
    {
        gameObject.transform.DOMoveZ(Random.Range(-10, 10f), 4).OnComplete(() => { RandomMoveZ(); });
    }

    public void RandomMoveY()
    {
        gameObject.transform.DOMoveY(Random.Range(offset1, offset2), 2).OnComplete(() => { RandomMoveY(); });
    }

}

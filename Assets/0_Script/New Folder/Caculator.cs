using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caculator : MonoBehaviour
{

    public static Caculator ins;
    void Awake()
    {
        ins = this;
    }

    void Update()
    {
        Caculate();
    }
    public List<GameObject> startPosList, endPosList;

    public GameObject startGo, endGo;
    public void Caculate()
    {
        if (startPosList.Count != 0)
        {
            var x = .1f;
            var y = .1f;
            var z = .1f;
            var countStart = 0;
            startPosList.ForEach(go =>
            {
                x += go.transform.position.x;
                y += go.transform.position.y;
                z += go.transform.position.z;
                countStart++;
            });
            startGo.transform.position = new Vector3(x, y, z) / countStart;
        }



        if (endPosList.Count != 0)
        {

            var x = .1f;
            var y = .1f;
            var z = .1f;
            var countEnd = 0;
            endPosList.ForEach(go =>
            {
                x += go.transform.position.x;
                y += go.transform.position.y;
                z += go.transform.position.z;
                countEnd++;
            });
            endGo.transform.position = new Vector3(x, y, z) / countEnd;
        }

    }
}

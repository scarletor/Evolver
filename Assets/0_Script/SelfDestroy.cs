using System;
using System.Collections;
using UnityEngine;
public class SelfDestroy : MonoBehaviour
{


    [SerializeField] private float timeInMinuneToDestroy;
    private void OnEnable()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            var secondToDestroy = timeInMinuneToDestroy * 60;
            StartCoroutine(DestroyThisInSecond(secondToDestroy));
        }
    }


    IEnumerator DestroyThisInSecond(float second)
    {
        yield return new WaitForSeconds(second);
        Destroy(gameObject);
    }





    public int solution(int N)
    {
        string bits = Convert.ToString(N, 2);
        int maxDiff = 0, newDiff = 0;
        for (int i = 0; i < bits.Length; i++)
        {
            if (bits[i] == '0')
            {
                newDiff++;
            }
            else
            {
                maxDiff = Math.Max(newDiff, maxDiff);
                newDiff = 0;
            }
        }
        return maxDiff;
    }


}





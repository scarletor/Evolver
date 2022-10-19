using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SharedScreen : MonoBehaviour
{

    public static SharedScreen ins;

    private void Awake()
    {
        ins = this;
    }


    public TextMeshProUGUI textGold;






    void Start()
    {
        DontDestroyOnLoad(this);
        UpdateTextGold();
    }

    public void UpdateTextGold()
    {
        textGold.text = PlayerData.ins.GetCurGold() + "";

    }








}

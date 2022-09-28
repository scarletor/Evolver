using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Sirenix.OdinInspector;
public class EggDataSelectUIBtn : MonoBehaviour
{





    public TextMeshProUGUI text;
    public EggData _eggData;

    [Button]
    public void Start()
    {
        SetupSelf();
       
    }




    public void SetupSelf()
    {
        if (_eggData.type == "fire") { transform.GetComponent<Button>().image.sprite = UIManager_Lab.ins.eggFireSprite; };
        if (_eggData.type == "frost") { transform.GetComponent<Button>().image.sprite = UIManager_Lab.ins.eggFrostSprite; };
        if (_eggData.type == "thunder") { transform.GetComponent<Button>().image.sprite = UIManager_Lab.ins.eggThunderSprite; };



        _eggData.startHatchDate = PlayerData.ins.GetEggHatchDate(_eggData.ownedID);
        name = _eggData.ownedID + "___" + _eggData.type + "__" + _eggData.startHatchDate;

        if (_eggData.startHatchDate != "")
        {
            InvokeRepeating("CheckTimeIntervals", 1, 1);
        }
        if (_eggData.startHatchDate == "" || _eggData.startHatchDate == null)
        {
            text.text = "Not hatched";
        }


    }


    public void CheckTimeIntervals()
    {
        _eggData.startHatchDate = PlayerData.ins.GetEggHatchDate(_eggData.ownedID);

        if (_eggData.startHatchDate == "" || _eggData.startHatchDate == null)
        {
            Debug.LogError("BUGGG");
            return;
        }

        if (Utils.ins.GetTimeLeft(_eggData.startHatchDate).TotalSeconds <= 0)
        {
            text.text = "Can Open";
            CancelInvoke("CheckTimeIntervals");
            return;
        }
        var textTemp = "" + Utils.ins.GetTimeLeft(_eggData.startHatchDate);
        text.text = textTemp.Substring(0, textTemp.LastIndexOf("."));

    }


}

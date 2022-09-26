using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EggDataSelectUIBtn : MonoBehaviour
{






    public EggData _eggData;

    public void Start()
    {
        SetupSelf();
    }




    public void SetupSelf()
    {
        if (_eggData.type == "fire") { transform.GetComponent<Button>().image.sprite = UIManager_Lab.ins.eggFireSprite; };
        if (_eggData.type == "frost") { transform.GetComponent<Button>().image.sprite = UIManager_Lab.ins.eggFrostSprite; };
        if (_eggData.type == "thunder") { transform.GetComponent<Button>().image.sprite = UIManager_Lab.ins.eggThunderSprite; };

        name = _eggData.type;
    }

}

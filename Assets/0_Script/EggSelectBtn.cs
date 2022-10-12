using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EggSelectBtn : MonoBehaviour
{
  

    public EggData _eggData;


    private void Start()
    {
        SetupSelf();
    }


    public Sprite eggFly, eggPlant, eggBeast;
    public void SetupSelf()
    {
        if(_eggData.eggType=="eggFly")GetComponent<Button>().image.sprite = eggFly;
        if(_eggData.eggType== "eggPlant") GetComponent<Button>().image.sprite = eggPlant;
        if(_eggData.eggType== "eggBeast") GetComponent<Button>().image.sprite = eggBeast;
    }
}

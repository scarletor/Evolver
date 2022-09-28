using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;
using System;
public class PlayerData : MonoBehaviour
{


    string playerBow = "playerBow";


    public static PlayerData ins;
    private void Awake()
    {
        ins = this;
    }

    [SerializeField]
    public TextAsset bowDataCSV;
    [SerializeField]
    public HashSet<string> allWords;

    void Start()
    {
        SetupSelf();
        ParsePetData();
        ParseBowData();
    }

    [TableList]
    public List<BowData> _bowData;


    [Button]
    public void ParseBowData()
    {
        //string[] w = wordAsset.text.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        //allWords = new HashSet<string>(w);


        string[] lines = bowDataCSV.text.Split('\n'); // line separator, i.e. newline
        lines = lines.Skip(1).Take(lines.Length - 1 - 1).ToArray(); // remove header and last empty line


        var index = 0;
        foreach (string line in lines)
        {
            string[] cols = line.Split('\t'); // column separator, i.e. tabulation
            _bowData[index].name = line.Split(char.Parse(","))[1];
            _bowData[index].damage = int.Parse(lines[index].Split(char.Parse(","))[2]);
            _bowData[index].id = index;
            index++;
        }

    }



    public void SetCurBow(int bowID)
    {

        PlayerPrefs.SetInt(playerBow, bowID);

    }


    public int GetCurBow()
    {
        return PlayerPrefs.GetInt(playerBow);
    }





    public bool IsPetUnlocked(int id)
    {
        return PlayerPrefs.GetString("PetUnlocked_" + id) == "" + 1;
    }


    [LabelText("Pet_____________________")]
    public string pet;
    [TableList]
    public List<PetData> _petList;
    [Button]
    public void GetAllPet()
    {

        Debug.LogError("G");
        int count = 0;
        StartCoroutine(GetData());
        Debug.LogError("G");

        IEnumerator GetData()
        {

            while (true)
            {
                yield return null;
                Debug.LogError("G");

                if (PlayerPrefs.GetString("Pet_Owned_" + count) == "") yield break;
                Debug.LogError("G");

                var data = PlayerPrefs.GetString("Pet_Owned_" + count);

                PetData pet = new PetData();
                pet.name = data.Split("_")[0];
                pet.type = data.Split("_")[2];
                pet.exp = int.Parse(data.Split("_")[3]);
                pet.taking = data.Split("_")[4] == "" + 0;
                _petList.Add(pet);
                Debug.LogError("GET PET__" + count + "__" + data);
                count++;
            }
        }

    }

    [Button]
    public void SavePet(string keyChain)
    {
        int count = 0;
        StartCoroutine(SavePetIE());
        IEnumerator SavePetIE()
        {
            while (true)
            {

                yield return null;
                if (PlayerPrefs.GetString("Pet_Owned_" + count) != "")
                {
                    count++;
                    continue;
                }

                PlayerPrefs.SetString("Pet_Owned_" + count, keyChain);

                Debug.LogError("loop to " + count);
                yield break;
            }
        }
    }



    [LabelText("Egg_____________________")]
    public string egg;


    [Button]
    public void SaveNewEgg(string keyChain)
    {
        int count = 0;
        StartCoroutine(SaveEggIE());
        IEnumerator SaveEggIE()
        {
            while (true)
            {

                yield return null;
                if (PlayerPrefs.GetString("Egg_Owned_" + count) != "")
                {
                    count++;
                    continue;
                }

                PlayerPrefs.SetString("Egg_Owned_" + count, keyChain);

                Debug.LogError("loop to " + count);
                yield break;
            }
        }
    }

    [Button]
    public void ChangeEggDataElement(string element, int eggOwnId)
    {


        var oldKey = PlayerPrefs.GetString("Egg_Owned_" + eggOwnId);
        oldKey = oldKey.Replace("fire", element);
        oldKey = oldKey.Replace("frost", element);
        oldKey = oldKey.Replace("thunder", element);

        PlayerPrefs.SetString("Egg_Owned_" + eggOwnId, oldKey);

    }


    public string GetEggHatchDate(int eggOwnId)
    {
        var key = PlayerPrefs.GetString("Egg_Owned_" + eggOwnId);
        EggData eggData = new EggData();
        eggData.SetDataByKey(key);
        return eggData.startHatchDate;
    }




    [Button]
    public void ChangeEggDataHatchTime(int eggOwnId)
    {
        var oldKey = PlayerPrefs.GetString("Egg_Owned_" + eggOwnId);
        if (oldKey.Contains(":"))
        {
            Debug.LogError("BUGG"); //already have timehatch
            return;
        }


        oldKey = oldKey + "_" + DateTime.Now;
        PlayerPrefs.SetString("Egg_Owned_" + eggOwnId, oldKey);

    }


    [Button]
    public void RemoveEggData(int eggOwnId)
    {
        PlayerPrefs.DeleteKey("Egg_Owned_" + eggOwnId);
    }


    [TableList]
    public List<EggData> _EggList;
    [Button]
    public void GetAllEgg()
    {

        Debug.LogError("G");
        int count = 0;
        StartCoroutine(GetData());
        Debug.LogError("G");

        IEnumerator GetData()
        {

            while (true)
            {
                yield return null;
                Debug.LogError("G");

                if (PlayerPrefs.GetString("Egg_Owned_" + count) == "") yield break;
                Debug.LogError("G");

                var data = PlayerPrefs.GetString("Egg_Owned_" + count);

                EggData egg = new EggData();
                egg.ownedID = count;
                egg.type = data.Split("_")[0];
                if (data.Split("_").Length > 1)
                    egg.startHatchDate = data.Split("_")[1];

                _EggList.Add(egg);
                Debug.LogError("GET Egg" + count + "__" + data);
                count++;
            }
        }

    }

    public void SetupSelf()
    {
        GetAllEgg();
        GetAllPet();
    }





    public TextAsset petDataCSV;
    [TableList]
    public List<PetData> petDataRef;


    [Button]
    public void ParsePetData()
    {
        //string[] w = wordAsset.text.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        //allWords = new HashSet<string>(w);

        Debug.LogError("WW");

        string[] lines = petDataCSV.text.Split('\n'); // line separator, i.e. newline
        lines = lines.Skip(1).Take(lines.Length - 1 - 1).ToArray(); // remove header and last empty line

        Debug.LogError("WW");

        var index = 0;
        foreach (string line in lines)
        {
            Debug.LogError("WW");

            string[] cols = line.Split('\t'); // column separator, i.e. tabulation
            petDataRef.Add(new PetData());
            petDataRef[index].name = line.Split(char.Parse(","))[1];
            petDataRef[index].damage = int.Parse(lines[index].Split(char.Parse(","))[2]);
            petDataRef[index].HP = int.Parse(lines[index].Split(char.Parse(","))[3]);
            petDataRef[index].id = index;
            index++;
        }
    }










}



[Serializable]
public class BowData
{
    public int id;
    public string name;
    public int damage;

}

[Serializable]
public class PetData
{
    public int id;
    public string name;
    public string type;// fire frost thunder
    public float exp;
    public bool taking;
    public float damage;
    public float HP;
}

[Serializable]
public class EggData
{
    public int ownedID;  // use to save data
    public string type;
    public string startHatchDate = "";  //hatching, hatched,



    public void SetDataByKey(string keyData)
    {
        type = keyData.Split("_")[0];
        if (keyData.Split("_").Length > 1)
            startHatchDate = keyData.Split("_")[1];

    }
}



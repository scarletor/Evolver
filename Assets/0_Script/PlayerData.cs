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
    public TextAsset wordAsset;
    [SerializeField]
    public HashSet<string> allWords;

    void Start()
    {

        Parse();
    }

    [TableList]
    public List<BowData> _bowData;


    [Button]
    public void Parse()
    {
        string[] w = wordAsset.text.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        allWords = new HashSet<string>(w);


        string[] lines = wordAsset.text.Split('\n'); // line separator, i.e. newline
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


}
[Serializable]
public class BowData
{
    public int id;
    public string name;
    public int damage;

}

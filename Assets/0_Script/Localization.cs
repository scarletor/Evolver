using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Localization : MonoBehaviour
{
    public TextMesh helloWordText, gameoverText;

    public class TextData
    {
        public string helloWordTextID, gameoverTextID;
        public string helloWordText_EN, gameoverText_EN;
        public string helloWordText_FE, gameoverText_FE;
    }

    [SerializeField] static TextData staticTextData;
    [SerializeField] public TextData publicTextData;

    private void Start()
    {
        ChangeAllTextToCurrentLanguage();
        staticTextData = publicTextData;
    }




    public void ChangeAllTextToCurrentLanguage()
    {
        var systemLanguage = Application.systemLanguage;

        switch (systemLanguage)
        {
            case SystemLanguage.French:
                helloWordText.text = "" + publicTextData.helloWordText_FE;
                break;
            case SystemLanguage.English:
                helloWordText.text = "" + publicTextData.helloWordText_EN;
                break;
        }
    }



    public static string GetTextLanguageByID(string stringID, SystemLanguage language)
    {
        switch (language)
        {
            case SystemLanguage.French:
                if (stringID == staticTextData.helloWordTextID) { return staticTextData.helloWordText_FE; };
                break;
            case SystemLanguage.English:
                if (stringID == staticTextData.helloWordTextID) { return staticTextData.helloWordText_EN; };
                break;
        }
        Debug.LogError("No word found!");
        return null;
    }

}

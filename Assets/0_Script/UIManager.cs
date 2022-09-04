using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{


    public static UIManager ins;

    private void Awake()
    {
        ins = this;
    }




    public TextMeshPro textGold, textTileUnlocked;
    public int gold, tileUnlocked;

    public void Reload()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Play");
    }



    public void UpdateGold()
    {
        textGold.text = "Gold: " + gold;
    }
    public void UpdateTileUnlocked()
    {
        textTileUnlocked.text = "TileUnlock: " + tileUnlocked;

    }




}

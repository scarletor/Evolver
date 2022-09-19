using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{


    public static UIManager ins;

    private void Awake()
    {
        ins = this;
    }

    private void Start()
    {
        RefreshText();
        gold = 6;
        tileUnlocked = 1;
    }



    [Button]
    public void RefreshText()
    {
        UpdateGold();
        UpdateTileUnlocked();
    }

    public TextMeshProUGUI textGold, textTileUnlocked;
    int _gold, _tileUnlocked;

    public int tileUnlocked 
    {
        get { return _tileUnlocked; }
        set { _tileUnlocked = value;
            RefreshText();
        }
    }
    public int gold
    {
        get { return _gold; }
        set
        {
            _gold = value;
            RefreshText();
        }
    }




    public void Reload()
    {
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }



    public void UpdateGold()
    {
        textGold.text = "Gold: " + _gold;
    }
    public void UpdateTileUnlocked()
    {
        textTileUnlocked.text = "TileUnlock: " + _tileUnlocked;

    }



    public GameObject dungeonPanel;
    public void ShowGoDungeonPanel()
    {
        dungeonPanel.SetActive(true);
    }

    public void CloseDungeonPanel()
    {
        dungeonPanel.SetActive(false);
    }
    public void OpenDungeon()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Dungeon");
    }




    public GameObject labOpenUI;
    public void ShowLabOpenUI()
    {
        labOpenUI.SetActive(true);
    }

    public void CloseLabOpenUI()
    {
        labOpenUI.SetActive(false);
    }
    public void OpenLabScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Lab");
    }





    public void QuitDungeon()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Play");
    }




    public GameObject ChangeItemUI;
    public void OnClickChangeItemBtn()
    {
        ChangeItemUI.gameObject.SetActive(true);
    }













    public GameObject portalIn, portalOut;
    public GameObject portalInUI, portalOutUI;
    public void OnClickOpenPortal()
    {
        PlayerController.ins.gameObject.transform.position = portalOut.transform.position;
    }
    public void OnClickBackPortal()
    {
        PlayerController.ins.gameObject.transform.position = portalIn.transform.position;
    }







}

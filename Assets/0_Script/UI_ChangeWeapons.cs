using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.UI;
public class UI_ChangeWeapons : MonoBehaviour
{



    public TextMeshProUGUI displayName, damage;

    public static UI_ChangeWeapons ins;
    private void Awake()
    {
        ins = this;
    }



    private void Start()
    {
        InvokeRepeating("ChangeAnim", 4, 4);
        RefreshUI();
       
    }

    public List<GameObject> allBows;
    public List<GameObject> allBtn;


    public void OnClickChangeBowBtn()
    {
        var selectedBowBtn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;



        var id = int.Parse(selectedBowBtn.name.Split("@_")[1]);


        displayName.text = "name: " + PlayerData.ins._bowData[id].name;
        damage.text = "damage: " + PlayerData.ins._bowData[id].damage + "";



        allBows.ForEach(ob => { ob.gameObject.SetActive(false); });
        allBows[id].gameObject.SetActive(true);

        allBtn.ForEach(btn => {
            btn.GetComponent<Button>().image.color = new Color(1, 0.7f, 1, 1);
        });
        selectedBowBtn.GetComponent<Button>().image.color = Color.green;

        PlayerData.ins.SetCurBow(id);

        PlayerController.ins.ChangeWeapons(id);
    }




    public Animator _anim;
    [Button]
    public void ChangeAnim()
    {

        var rd = Random.Range(1, 4);
        if(rd==1)
        {
        _anim.SetBool("AttackRange", false);
        _anim.SetBool("isMoving", false);
        _anim.SetBool("Idle", true);

        }

        if (rd == 2)
        {
            _anim.SetBool("AttackRange", false);
            _anim.SetBool("isMoving", true);
            _anim.SetBool("Idle", false);
        }
        if (rd == 3)
        {
            _anim.SetBool("AttackRange", true);
            _anim.SetBool("isMoving", false);
            _anim.SetBool("Idle", false);
        }

    }


    [Button]
    public void RefreshUI()
    {
        allBtn.ForEach(btn => {
            btn.GetComponent<Button>().image.color = new Color(1,0.7f,1,1);
        });

        allBows.ForEach(bow =>
        {
            bow.gameObject.SetActive(false);
        });

        var id = PlayerData.ins.GetCurBow();
        allBows[id].gameObject.SetActive(true);
        allBtn[id].GetComponent<Button>().image.color = Color.green;

        displayName.text = "name: " + PlayerData.ins._bowData[id].name;
        damage.text = "damage: " + PlayerData.ins._bowData[id].damage + "";

        PlayerController.ins.ChangeWeapons(id);

    }



    public void CloseBtn()
    {
        gameObject.SetActive(false);
    }








}

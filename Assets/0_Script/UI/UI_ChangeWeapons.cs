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

    public List<itemSet> setList;
    public List<GameObject> allBtn;


    public void OnClickChangeBowBtn()
    {
        var selectedBowBtn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;



        var id = int.Parse(selectedBowBtn.name.Split("@_")[1]);


        displayName.text = "name: " + PlayerData.ins._bowData[id].name;
        damage.text = "damage: " + PlayerData.ins._bowData[id].damage + "";



        setList.ForEach(set =>
        {
            set.itemList.ForEach(go => { go.gameObject.SetActive(false); });
        });
        setList[id].itemList.ForEach(go => { go.SetActive(true); });





        allBtn.ForEach(btn =>
        {
            btn.transform.GetChild(1).gameObject.SetActive(true);
            btn.transform.GetChild(2).gameObject.SetActive(false);
        });
        selectedBowBtn.transform.GetChild(2).gameObject.SetActive(true);

        PlayerData.ins.SetCurBow(id);

        PlayerController.ins.ChangeWeapons(id);


        // allBtn.ForEach(btn =>
        //{
        //    //default btn
        //    btn.transform.GetChild(1).gameObject.SetActive(false);
        //    btn.transform.GetChild(2).gameObject.SetActive(false);

        //});

        //setList.ForEach(set =>
        //{
        //    set.itemList.ForEach(go => { go.gameObject.SetActive(false); });
        //});



        //var id = PlayerData.ins.GetCurSet();
        //setList[id].itemList.ForEach(go => { go.SetActive(true); });


        //allBtn[id].transform.GetChild(2).gameObject.SetActive(true);


    }




    public Animator _anim;
    [Button]
    public void ChangeAnim()
    {

        var rd = Random.Range(1, 4);
        if (rd == 1)
        {
            //_anim.SetBool("AttackRange", false);
            _anim.SetBool("isMoving", false);
            _anim.SetBool("Idle", true);

        }

        if (rd == 2)
        {
            //_anim.SetBool("AttackRange", false);
            _anim.SetBool("isMoving", true);
            _anim.SetBool("Idle", false);
        }
        if (rd == 3)
        {
            //_anim.SetBool("AttackRange", true);
            _anim.SetBool("isMoving", false);
            _anim.SetBool("Idle", false);
        }

    }


    [Button]
    public void RefreshUI()
    {
        allBtn.ForEach(btn =>
        {
            //default btn
            btn.transform.GetChild(1).gameObject.SetActive(true);
            btn.transform.GetChild(2).gameObject.SetActive(false);

        });

        setList.ForEach(set =>
        {
            set.itemList.ForEach(go => { go.gameObject.SetActive(false); });
        });



        var id = PlayerData.ins.GetCurSet();
        setList[id].itemList.ForEach(go => { go.SetActive(true); });


        allBtn[id].transform.GetChild(2).gameObject.SetActive(true);


        PlayerController.ins.ChangeWeapons(id);

    }



    public void CloseBtn()
    {
        gameObject.SetActive(false);
    }







}
[System.Serializable]
public class itemSet
{
    public int index;
    public List<GameObject> itemList;
}
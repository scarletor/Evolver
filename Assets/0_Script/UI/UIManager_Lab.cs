using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;
public class UIManager_Lab : MonoBehaviour
{

    public static UIManager_Lab ins;
    private void Awake()
    {
        ins = this;
    }
    void Start()
    {
        Setup();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }







    public GameObject drone1, drone2, particle1, particle2, boomParticle, guardParticle;
    public Animator _anim;


    public GameObject particleFireInjection1, particleFireInjection2;
    public GameObject particleFrostInjection1, particleFrostInjection2;



    public GameObject drone1PosStart, drone2PosStart, drone1PosEnd, drone2PosEnd;

    [Button]
    public void InjectEgg(string element)
    {
        drone1.transform.DOMove(drone1PosEnd.transform.position, 1).OnComplete(() =>
        {
            drone1.transform.DORotate(new Vector3(0, -40, 0), 1).OnComplete(() =>
            {
                drone1.transform.DORotate(new Vector3(25, -40, 0), 1).OnComplete(() =>
                {
                    guardParticle.SetActive(false);
                    particle1.gameObject.SetActive(true);
                    Utils.ins.DelayCall(4, () =>
                    {
                        particle1.gameObject.SetActive(false);
                        boomParticle.SetActive(false);
                        boomParticle.SetActive(true);
                        guardParticle.SetActive(true);
                        ChangeElementCurrentEgg(element);
                        Utils.ins.DelayCall(2, () =>
                        {
                            drone1.transform.DORotate(new Vector3(0, -40, 0), 1).SetEase(Ease.Linear);
                        });

                        Utils.ins.DelayCall(3, () =>
                        {
                            drone1.transform.DORotate(Vector3.zero, 1).SetEase(Ease.Linear).OnComplete(() =>
                            {
                                drone1.transform.DOMove(drone1PosStart.transform.position, 1);
                            });
                        });
                    });
                }).SetEase(Ease.Linear);
                ;
            }).SetEase(Ease.Linear);
        });



        drone2.transform.DOMove(drone2PosEnd.transform.position, 1).OnComplete(() =>
        {
            drone2.transform.DORotate(new Vector3(0, 40, 0), 1).OnComplete(() =>
            {
                drone2.transform.DORotate(new Vector3(25, 40, 0), 1).OnComplete(() =>
                {
                    particle2.gameObject.SetActive(true);
                    Utils.ins.DelayCall(4, () =>
                    {
                        particle2.gameObject.SetActive(false);

                        Utils.ins.DelayCall(2, () =>
                        {
                            drone2.transform.DORotate(new Vector3(0, 40, 0), 1).SetEase(Ease.Linear);
                        });

                        Utils.ins.DelayCall(3, () =>
                        {
                            drone2.transform.DORotate(Vector3.zero, 1).SetEase(Ease.Linear).OnComplete(() =>
                            {
                                drone2.transform.DOMove(drone2PosStart.transform.position, 1);
                            });
                        });
                    });
                }).SetEase(Ease.Linear);
                ;
            }).SetEase(Ease.Linear);
        });

    }


    public GameObject curEggSelecting;
    public void ChangeElementCurrentEgg(string element)
    {
        var eggData = curEggSelecting.GetComponent<EggDataSelectUIBtn>();
        eggData._eggData.type = element;
        eggData.SetupSelf();


        PlayerData.ins.ChangeEggDataElement(element, eggData._eggData.ownedID);
        if (element == "fire")
        {
            eggDemoMesh.material = eggDemoFire;
            btnInjectFire.gameObject.SetActive(false);
            btnInjectFrost.gameObject.SetActive(true);
            btnInjectThunder.gameObject.SetActive(true);
        }
        if (element == "frost")
        {
            eggDemoMesh.material = eggDemoFrost;
            btnInjectFire.gameObject.SetActive(true);
            btnInjectFrost.gameObject.SetActive(false);
            btnInjectThunder.gameObject.SetActive(true);
        }
        if (element == "thunder")
        {
            eggDemoMesh.material = eggDemoThunder;
            btnInjectFire.gameObject.SetActive(true);
            btnInjectFrost.gameObject.SetActive(true);
            btnInjectThunder.gameObject.SetActive(false);
        }
    }







    [Button]
    public void DroneLook3()
    {
        drone1.transform.DORotate(new Vector3(0, 0, 0), 1);
    }



    [Button]
    public void DroneLook4()
    {
    }


    [Button]
    public void DroneLook5()
    {
        particle1.gameObject.SetActive(false);
    }


    [Button]
    public void DroneLook6()
    {
        particle1.gameObject.SetActive(false);
    }



    public void Back()
    {

        SceneManager.LoadScene("Play");


    }



    public GameObject eggScrollView, petScrollView, eggTabBtn, petTabBtn, eggGrBtn, petGrBtn, eggContent, petContent;

    public void OnClickEggTab()
    {

        eggScrollView.gameObject.SetActive(true);
        eggTabBtn.GetComponent<Button>().image.color = Color.white;
        eggGrBtn.gameObject.SetActive(true);
        eggDemo.gameObject.SetActive(true);

        petScrollView.gameObject.SetActive(false);
        petTabBtn.GetComponent<Button>().image.color = Color.gray;
        petGrBtn.gameObject.SetActive(false);
        petDemo.gameObject.SetActive(false);



    }



    public void OnClickPetTab()
    {

        eggScrollView.gameObject.SetActive(false);
        eggTabBtn.GetComponent<Button>().image.color = Color.gray;
        eggGrBtn.gameObject.SetActive(false);

        petScrollView.gameObject.SetActive(true);
        petTabBtn.GetComponent<Button>().image.color = Color.white;
        petGrBtn.gameObject.SetActive(true);


    }

    public Sprite eggFireSprite, eggThunderSprite, eggFrostSprite;
    public GameObject selector;
    public SkinnedMeshRenderer eggDemoMesh;
    public Material eggDemoFrost, eggDemoFire, eggDemoThunder;
    public GameObject btnInjectFire, btnInjectFrost, btnInjectThunder;


    public void OnClickHatchEgg()
    {
        var curEggID = curEggSelecting.GetComponent<EggDataSelectUIBtn>()._eggData.ownedID;
        PlayerData.ins.ChangeEggDataHatchTime(curEggID);
        curEggSelecting.GetComponent<EggDataSelectUIBtn>().SetupSelf();
        btnInjectFire.transform.parent.gameObject.SetActive(false);
        eggHatchBtn.gameObject.SetActive(false);
        eggOpenBtn.gameObject.SetActive(true);
        eggOpenBtn.GetComponent<Button>().interactable = false;
    }


    public void OnClickOpenEgg()
    {
        takePetUI.gameObject.SetActive(true);
        SetupDisplayPet(0, "thunder");

        curEggSelecting.gameObject.SetActive(false);
        selector.gameObject.transform.position = new Vector2(10000, 100000000);

        eggOpenBtn.gameObject.SetActive(false);
        eggDemo.gameObject.SetActive(false);
    }

    public void OnClickInjectEgg(string element)
    {
        if (element == "fire")
        {
            InjectEgg(element);

        }
        if (element == "frost")
        {
            InjectEgg(element);

        }
        if (element == "thunder")
        {
            InjectEgg(element);
        }




    }


    public GameObject petBtnPref, eggBtnPref;

    public void OnClickSelectPetBtn()
    {
        var curGo = EventSystem.current.currentSelectedGameObject;
        selector.transform.position = curGo.transform.position;
        var petData = curGo.GetComponent<PetDataSelectBtn>()._petData;
    }



    [Button]
    public void testTime()
    {
        currentTime = "" + DateTime.Now.ToUniversalTime().ToString();
        Debug.LogError(currentTime);
        Debug.LogError(DateTime.Now);

        //Debug.LogError(DateTime.Now.ToFileTime());
        //Debug.LogError(DateTime.Now.ToLongDateString());
        //Debug.LogError(DateTime.Now.ToLongTimeString());
        //Debug.LogError(DateTime.Now.ToString());
        //Debug.LogError(DateTime.Now.ToUniversalTime());
        //Debug.LogError(DateTime.Now.ToShortTimeString());
        //Debug.LogError(DateTime.Now.ToBinary());

    }

    public string currentTime;
    public GameObject eggDemo, petDemo, eggHatchBtn, eggOpenBtn;
    public TextMeshProUGUI textEggHatchTimeLeft;
    public void OnClickSelectEggBtn()
    {

        var thisBtn = EventSystem.current.currentSelectedGameObject;


        selector.transform.position = thisBtn.transform.position;
        curEggSelecting = thisBtn;
        eggDemo.gameObject.SetActive(true);
        petDemo.gameObject.SetActive(false);
        btnInjectFire.transform.parent.gameObject.SetActive(true);


        var eggData = thisBtn.GetComponent<EggDataSelectUIBtn>()._eggData;





        if (eggData.type == "fire")
        {
            eggDemoMesh.material = eggDemoFire;
            btnInjectFire.gameObject.SetActive(false);
            btnInjectFrost.gameObject.SetActive(true);
            btnInjectThunder.gameObject.SetActive(true);
        }
        if (eggData.type == "frost")
        {
            eggDemoMesh.material = eggDemoFrost;
            btnInjectFire.gameObject.SetActive(true);
            btnInjectFrost.gameObject.SetActive(false);
            btnInjectThunder.gameObject.SetActive(true);
        }
        if (eggData.type == "thunder")
        {
            eggDemoMesh.material = eggDemoThunder;
            btnInjectFire.gameObject.SetActive(true);
            btnInjectFrost.gameObject.SetActive(true);
            btnInjectThunder.gameObject.SetActive(false);
        }


        var eggState = thisBtn.GetComponent<EggDataSelectUIBtn>().text.text;

        if (eggState == "Can Open")
        {
            eggOpenBtn.gameObject.SetActive(true);
            eggOpenBtn.GetComponent<Button>().interactable = true;

            eggHatchBtn.gameObject.SetActive(false);
            btnInjectFire.transform.parent.gameObject.SetActive(false);
            _anim.Play("Shake");
        }
        else if (eggState == "Not hatched")
        {
            eggOpenBtn.gameObject.SetActive(false);
            eggOpenBtn.GetComponent<Button>().interactable = false;

            eggHatchBtn.gameObject.SetActive(true);
            btnInjectFire.transform.parent.gameObject.SetActive(true);
            _anim.Play("EggIdle");

        }
        else  // hatching time running
        {
            eggOpenBtn.gameObject.SetActive(true);
            eggOpenBtn.GetComponent<Button>().interactable = false;

            eggHatchBtn.gameObject.SetActive(false);
            btnInjectFire.transform.parent.gameObject.SetActive(false);
            _anim.Play("EggIdle");

        }

    }



    [Button]
    public void Setup()
    {
        Utils.ins.DelayCall(2, () =>
         {
             PlayerData.ins._EggList.ForEach(eggData =>
             {
                 var newEggBtn = Instantiate(eggBtnPref);
                 newEggBtn.GetComponent<EggDataSelectUIBtn>()._eggData = eggData;
                 newEggBtn.GetComponent<EggDataSelectUIBtn>().SetupSelf();
                 newEggBtn.transform.SetParent(eggContent.transform, false);
                 newEggBtn.GetComponent<Button>().onClick.AddListener(() => { OnClickSelectEggBtn(); });
             });

         });
    }













    public GameObject takePetUI;
    public TextMeshProUGUI petName, petHp, petDamage, petElement;

    public void OnClickTakePetBtn()
    {
        petTaken.Add(currentPetSelect);
        takePetBtn.gameObject.SetActive(false);
        leavePetBtn.gameObject.SetActive(true);
        currentPetSelect.transform.GetChild(0).gameObject.SetActive(true);

    }
    public void OnClickLeavePetBtn()
    {
        petTaken.Remove(currentPetSelect);
        takePetBtn.gameObject.SetActive(true);
        leavePetBtn.gameObject.SetActive(false);
        currentPetSelect.transform.GetChild(0).gameObject.SetActive(false);
    }

    [Button]
    public void SetupDisplayPet(int petID, string element)
    {
        var petData = PlayerData.ins.petDataRef.Find((pet) => pet.id == petID);
        petName.text = "Name: " + petData.name;
        petHp.text = "HP: " + petData.HP;
        petDamage.text = "Damage: " + petData.damage;
        petElement.text = "Element: " + element;
    }







    public List<GameObject> petTaken;
    public GameObject petSelector, currentPetSelect, takePetBtn, leavePetBtn;
    public void OnClickSelectPet(int id)
    {
        petSelector.gameObject.SetActive(true);
        petSelector.transform.position = EventSystem.current.currentSelectedGameObject.transform.position;
        currentPetSelect = EventSystem.current.currentSelectedGameObject;
        selector.gameObject.SetActive(false);

        eggDemo.gameObject.SetActive(false);
        petDemo.gameObject.SetActive(true);

        foreach (Transform trans in petDemo.transform)
        {
            trans.gameObject.SetActive(false);
        }

        petDemo.transform.GetChild(id).gameObject.SetActive(true);


        takePetBtn.gameObject.SetActive(!petTaken.Contains(currentPetSelect));
        leavePetBtn.gameObject.SetActive(petTaken.Contains(currentPetSelect));                   
    }






   








}

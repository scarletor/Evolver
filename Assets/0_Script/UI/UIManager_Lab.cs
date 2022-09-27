using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    public void InjectEgg(string elemet)
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
                        boomParticle.SetActive(true);
                        _anim.Play("Shake");
                        guardParticle.SetActive(true);

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

    }

    public void OnClickOpenEgg()
    {

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


    public GameObject eggDemo, petDemo;
    public void OnClickSelectEggBtn()
    {
        selector.transform.position = EventSystem.current.currentSelectedGameObject.transform.position;
        eggDemo.gameObject.SetActive(true);
        petDemo.gameObject.SetActive(false);

        var eggData = EventSystem.current.currentSelectedGameObject.GetComponent<EggDataSelectUIBtn>()._eggData;
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
                 newEggBtn.transform.parent = eggContent.transform;
                 newEggBtn.GetComponent<Button>().onClick.AddListener(() => { OnClickSelectEggBtn(); });
             });

         });
    }









}

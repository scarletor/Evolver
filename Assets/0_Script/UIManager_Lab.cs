using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class UIManager_Lab : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
   

    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }







    public GameObject drone1, drone2, particle1,particle2, boomParticle, guardParticle;
    public Animator _anim;

    public void Upgrade()
    {

    }

    [Button]
    public void UpgradeEgg()
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
                        drone1.transform.DORotate(Vector3.zero, 1).SetEase(Ease.Linear);
                    });


                });
            }).SetEase(Ease.Linear);
                ;
        }).SetEase(Ease.Linear);





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
                        drone2.transform.DORotate(Vector3.zero, 1).SetEase(Ease.Linear);
                    });


                });
            }).SetEase(Ease.Linear);
            ;
        }).SetEase(Ease.Linear);

















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
}

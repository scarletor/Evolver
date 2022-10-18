using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using System;
using UnityEditor;
public class Utils : MonoBehaviour
{
    public static Utils ins;
    public GameObject gold, goldGr;
    public TextFloatingEff textEffRed, textEffWhite;
    public GameObject yellowBullet, yellowMuzzle, yellowImpact;
    float timeHatch = 3600;


    public void Awake()
    {
        if (ins == null)
        {
            ins = this;
        }
        else
        {
            Destroy(gameObject);
        }


    }

    public void DelayCall(float dl, System.Action cd)
    {

        StartCoroutine(DelayCallIE(cd, dl));
        IEnumerator DelayCallIE(System.Action cd2, float dl2)
        {
            yield return new WaitForSeconds(dl);
            if (cd != null)
                cd.Invoke();
        }
    }


    [Button]
    public void FadeInUI(GameObject go)
    {
        CanvasGroup cvg = new CanvasGroup();
        if (go.GetComponent<CanvasGroup>() == null)
        {
            cvg = go.AddComponent<CanvasGroup>();
        }
        else
        {
            cvg = go.GetComponent<CanvasGroup>();
        }
        go.SetActive(true);
        var duration = 2;
        cvg.alpha = 0;
        float tweenValue = 0;
        float endTweenValue = 1;
        DOTween.To(() => tweenValue, x => tweenValue = x, endTweenValue, duration)
            .OnUpdate(() =>
            {
                cvg.alpha = tweenValue;
            });
    }


    [Button]
    public void FadeOutUI(GameObject go)
    {
        CanvasGroup cvg = new CanvasGroup();
        if (go.GetComponent<CanvasGroup>() == null)
        {
            cvg = go.AddComponent<CanvasGroup>();
        }
        else
        {
            cvg = go.GetComponent<CanvasGroup>();
        }
        go.SetActive(true);
        var duration = 2;
        cvg.alpha = 0;
        float tweenValue = 0;
        float endTweenValue = 1;
        DOTween.To(() => tweenValue, x => tweenValue = x, endTweenValue, duration)
            .OnUpdate(() =>
            {
                cvg.alpha = tweenValue;
                Debug.LogError(cvg.alpha);
                Debug.LogError(tweenValue);
            });
    }


    [Button]
    public void ReloadCurrentScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }



    [Button]
    public void IncreaseNumEff(Text text, int value)
    {
        var curValue = Int32.Parse(text.text);


        int endTweenValue = curValue + value;
        DOTween.To(() => curValue, x => curValue = x, endTweenValue, 2)
           .OnUpdate(() =>
           {
               text.text = "" + curValue;
           });
    }


    public int off1, off2;
    public float off3;
    [Button]
    public void SpawnGold(GameObject spawnPos)
    {

        StartCoroutine(Run());
        IEnumerator Run()
        {

            var rd = UnityEngine.Random.Range(4, 8);
            for (int i = 0; i < rd; i++)
            {
                yield return new WaitForSeconds(.03f);
                var offset = UnityEngine.Random.Range(-1.5f, 1.5f);
                var offset2 = UnityEngine.Random.Range(-1.5f, 1.5f);
                var newGold = Instantiate(gold);
                newGold.transform.position = spawnPos.transform.position;

                newGold.transform.DOLocalJump(new Vector3(spawnPos.transform.position.x + offset, 5, spawnPos.transform.position.z + offset2), off1, off2, off3).SetEase(Ease.OutQuart);

                //newGold.transform.position = spawnPos.transform.position;



            }
        }
    }

    [Button]
    public TimeSpan GetTimeLeft(string timeHatchString)
    {
        var _timeHatch = DateTime.Parse(timeHatchString);
        //var minus = (DateTime.Now - _timeHatch.AddSeconds(this.timeHatch));
        var timeSpan = DateTime.Now - DateTime.Now.AddSeconds(3600);


        var minus = timeHatch - (DateTime.Now - _timeHatch).TotalSeconds;

        return TimeSpan.FromSeconds(minus);

    }

    public List<GameObject> tileToRemove;
    [Button]
    public void RemoveGroundColliderFunc()
    {
        GameObject tileBlock = LevelDesign_BlockGround.ins.gameObject;
        GameObject tileGround = GroundLoader.ins.gameObject;


        foreach (Transform childBlock in tileBlock.transform)
        {
            var isFar = true;
            foreach (Transform childGround in tileGround.transform)
            {
                if (Vector3.Distance(childBlock.position, childGround.position) < 7)
                {
                    tileToRemove.Add(childBlock.gameObject);
                }

                if (Vector3.Distance(childBlock.position, childGround.position) < 15)
                {
                    isFar = false;
                }
            }
            if (isFar == true) tileToRemove.Add(childBlock.gameObject);
        }
        tileToRemove.ForEach(tile => { Destroy(tile); });



        foreach (Transform childBlock1 in tileBlock.transform)
        {
            childBlock1.transform.position = new Vector3(childBlock1.transform.position.x, 0, childBlock1.transform.position.z);
            Destroy(childBlock1.GetComponent<MeshRenderer>());
            Destroy(childBlock1.GetComponent<MeshFilter>());
        }

    }


}
#if (UNITY_EDITOR)
public class RenameChildren : EditorWindow
{
    private static readonly Vector2Int size = new Vector2Int(250, 100);
    private string childrenPrefix;
    private int startIndex;
    [MenuItem("Tools/Rename children")]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow<RenameChildren>();
        window.minSize = size;
        window.maxSize = size;
    }
    private void OnGUI()
    {
        childrenPrefix = EditorGUILayout.TextField("Children prefix", childrenPrefix);
        startIndex = EditorGUILayout.IntField("Start index", startIndex);
        if (GUILayout.Button("Rename children"))
        {
            GameObject[] selectedObjects = Selection.gameObjects;
            for (int objectI = 0; objectI < selectedObjects.Length; objectI++)
            {
                Transform selectedObjectT = selectedObjects[objectI].transform;
                for (int childI = 0, i = startIndex; childI < selectedObjectT.childCount; childI++) selectedObjectT.GetChild(childI).name = $"{childrenPrefix}{i++}";
            }
        }
    }





    [MenuItem("Tools/Remove Ground Collider")]
    public static void Run()
    {
        Utils.ins.RemoveGroundColliderFunc();
    }






}















#endif

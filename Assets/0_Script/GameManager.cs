using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        SceneManager.LoadScene("Shared", LoadSceneMode.Additive);

        Debug.LogError("LOAD");
    }

    // Update is called once per frame
    void Update()
    {

    }



    public GameObject ground, water;
}

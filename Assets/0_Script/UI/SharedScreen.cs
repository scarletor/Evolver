using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SharedScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateFPS", 1, .2f);
    }

    // Update is called once per frame
    public int avgFrameRate;
    public TextMeshProUGUI display_Text;
    private float currentFPS;
    public void UpdateFPS()
    {
        currentFPS = (int)(1f / Time.unscaledDeltaTime);
        avgFrameRate = (int)currentFPS;
        display_Text.text = avgFrameRate.ToString() + " FPS";
    }










}

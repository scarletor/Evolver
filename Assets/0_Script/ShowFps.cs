using UnityEngine;
using System.Collections;

public class ShowFps : MonoBehaviour
{

    const float fpsMeasurePeriod = 0.5f;
    private int m_FpsAccumulator = 0;
    private float m_FpsNextPeriod = 0;
    private int m_CurrentFps;
    const string display = "{0} FPS";


    void Start()
    {
        Application.targetFrameRate = 60;
        m_FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
        guiStyle.fontSize = 30;
        guiStyle.normal.textColor = Color.white;
    }

    void Update()
    {
        // measure average frames per second
        m_FpsAccumulator++;
        if (Time.realtimeSinceStartup > m_FpsNextPeriod)
        {
            m_CurrentFps = (int)(m_FpsAccumulator / fpsMeasurePeriod);
            m_FpsAccumulator = 0;
            m_FpsNextPeriod += fpsMeasurePeriod;
        }
    }
    private GUIStyle guiStyle = new GUIStyle();
    void OnGUI()
    {
        GUI.Label(new Rect(100,200,300,300),string.Format(display, m_CurrentFps), guiStyle);
        //GUILayout.Label(string.Format(display, m_CurrentFps));
    }

}
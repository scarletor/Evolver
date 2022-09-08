using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using System;
using UnityEngine;

public class EGA_Laser : MonoBehaviour
{
    public GameObject HitEffect;
    public float HitOffset = 0;

    public float MaxLength;
    private LineRenderer Laser;

    public float MainTextureLength = 1f;
    public float NoiseTextureLength = 1f;
    private Vector4 Length = new Vector4(1, 1, 1, 1);
    //private Vector4 LaserSpeed = new Vector4(0, 0, 0, 0); {DISABLED AFTER UPDATE}
    //private Vector4 LaserStartSpeed; {DISABLED AFTER UPDATE}
    //One activation per shoot
    private bool LaserSaver = false;
    private bool UpdateSaver = false;

    private ParticleSystem[] Effects;
    private ParticleSystem[] Hit;

    void Start()
    {
        //Get LineRender and ParticleSystem components from current prefab;  
        Laser = GetComponent<LineRenderer>();
        Effects = GetComponentsInChildren<ParticleSystem>();
        Hit = HitEffect.GetComponentsInChildren<ParticleSystem>();
        //if (Laser.material.HasProperty("_SpeedMainTexUVNoiseZW")) LaserStartSpeed = Laser.material.GetVector("_SpeedMainTexUVNoiseZW");
        //Save [1] and [3] textures speed
        //{ DISABLED AFTER UPDATE}
        //LaserSpeed = LaserStartSpeed;
    }

    public GameObject startPos, endPos;
    void Update()
    {
        //if (Laser.material.HasProperty("_SpeedMainTexUVNoiseZW")) Laser.material.SetVector("_SpeedMainTexUVNoiseZW", LaserSpeed);
        //SetVector("_TilingMainTexUVNoiseZW", Length); - old code, _TilingMainTexUVNoiseZW no more exist
        Laser.material.SetTextureScale("_MainTex", new Vector2(Length[0], Length[1]));
        Laser.material.SetTextureScale("_Noise", new Vector2(Length[2], Length[3]));
        //To set LineRender position
        Laser.SetPosition(0, startPos.transform.position);

        Laser.SetPosition(1, endPos.transform.position);
        HitEffect.transform.position = endPos.transform.position;
        //Hit effect zero rotation
        HitEffect.transform.rotation = Quaternion.identity;
        foreach (var AllPs in Effects)
        {
            if (!AllPs.isPlaying) AllPs.Play();
        }
        //Texture tiling
        Length[0] = MainTextureLength * (Vector3.Distance(transform.position, endPos.transform.position));
        Length[2] = NoiseTextureLength * (Vector3.Distance(transform.position, endPos.transform.position));
        //Texture speed balancer {DISABLED AFTER UPDATE}
        //LaserSpeed[0] = (LaserStartSpeed[0] * 4) / (Vector3.Distance(transform.position, hit.point));
        //LaserSpeed[2] = (LaserStartSpeed[2] * 4) / (Vector3.Distance(transform.position, hit.point));
        {
            LaserSaver = true;
            Laser.enabled = true;
        }

    }

    public void DisablePrepare()
    {
        if (Laser != null)
        {
            Laser.enabled = false;
        }
        UpdateSaver = true;
        //Effects can = null in multiply shooting
        if (Effects != null)
        {
            foreach (var AllPs in Effects)
            {
                if (AllPs.isPlaying) AllPs.Stop();
            }
        }
    }
}

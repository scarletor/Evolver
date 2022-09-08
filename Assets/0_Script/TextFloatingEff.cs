using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class TextFloatingEff : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Camera.main.transform.rotation;



        transform.DOMoveY(3, 2).OnComplete(() =>
        {
            DOTween.To(() => color, x => color = x, 0, 2).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        });
        _text = GetComponent<TextMeshPro>();
    }

    public float color;


    public TextMeshPro _text;
    public void SetValue(string value)
    {
        _text.text = value;
    }

    private void LateUpdate()
    {
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, color);
    }


}

using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SmoothTextController : MonoBehaviour
{

    public bool show;
    public float aOn, aOff;

    public List<TMP_Text> text;

    void Update()
    {
        for (int i = 0; i < text.Count; i++) {
            text[i].color = new Color(text[i].color.r, text[i].color.g, text[i].color.b, Mathf.Lerp(text[i].color.a, (show) ? aOn : aOff, 5f * Time.deltaTime));
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class DigitSlot : MonoBehaviour
{

    public List<float> ys;
    public RectTransform d;

    public bool show;
    public int number;

    public float speed;

    void Update()
    {
        float index = (!show) ? ys[0] : ys[number + 1];

        d.anchoredPosition = new Vector2(d.anchoredPosition.x, Mathf.Lerp(d.anchoredPosition.y, index, speed*Time.deltaTime));
    }
}

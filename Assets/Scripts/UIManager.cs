using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public UIStyleSO currentStyle;
    UIStyleSO previous;

    public void Awake()
    {
        instance = this;
    }
    public void UpdateUI() {
        if (previous == currentStyle) return;


        previous = currentStyle;

    }
    public void Update()
    {
        UpdateUI();
    }


}

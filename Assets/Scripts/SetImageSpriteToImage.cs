using UnityEngine;
using UnityEngine.UI;
[ExecuteInEditMode]
public class SetImageSpriteToImage : MonoBehaviour
{

    public Image self, inherit;
    void Update()
    {
        if (self == null) return;
        if (inherit==null)
        {
            return;
        }
        self.sprite = inherit.sprite;
        self.rectTransform.localScale = inherit.rectTransform.localScale;
        self.rectTransform.sizeDelta = inherit.rectTransform.sizeDelta;
    }
}

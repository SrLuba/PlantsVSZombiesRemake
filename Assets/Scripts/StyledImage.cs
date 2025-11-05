using UnityEngine;
using UnityEngine.UI;

public class StyledImage : MonoBehaviour
{
    public int imageID;
    Image im;
    void Start()
    {
        im = this.GetComponent<Image>();    
    }

    void Update()
    {
        if (im == null) return;

        im.sprite = UIManager.instance.currentStyle.sprites[imageID];
    }
}

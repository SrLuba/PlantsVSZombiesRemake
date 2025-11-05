using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    private void Awake()
    {
        instance = this;
    }

    public bool cys;
    public Vector3 defaultPos;

    public Transform cam;

    bool pCYS = false;

    public Animator anim;

    public void Reset()
    {
        cys = false;
        cam.transform.position = defaultPos;
    }
    public void Update()
    {
        if (pCYS != cys) {
            pCYS = cys;

            anim.Play(cys ? "cys" : "start", 0, 0f);
        }
        
    }

}

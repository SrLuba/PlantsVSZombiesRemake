using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    private void Awake()
    {
        instance = this;
    }

    public bool cys;
    public Vector3 chooseYourSeedsPos, defaultPos;

    public Transform cam;
    public float speed;

    bool pCYS = false;
    float timer = 0f;


    public void Reset()
    {
        cys = false;
        cam.transform.position = defaultPos;
    }
    public void Update()
    {
        if (pCYS != cys) {
            timer = 0f;
            pCYS = cys;
        }
        Vector3 targetPos = cys ? chooseYourSeedsPos : defaultPos;

        cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, speed*Time.deltaTime);
    }

}

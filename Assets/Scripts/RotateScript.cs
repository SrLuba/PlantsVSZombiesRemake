using UnityEngine;

public class RotateScript : MonoBehaviour
{
    public Vector3 rotateSpeed;
 
 
    void Update()
    {
        this.transform.localEulerAngles += rotateSpeed * Time.deltaTime;

        if (this.transform.localEulerAngles.z > 360f) this.transform.localEulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, 0f);
    }
}

using UnityEngine;

public class LerpObject : MonoBehaviour
{
    public Transform target;
    public Vector3 normalPos, showPos;

    public bool show;
    public float speed;
    public void Reset()
    {
        target.transform.position = normalPos;
        show = false;
    }

    void Update()
    {
        Vector3 res = show ? showPos : normalPos;

        target.transform.localPosition = Vector3.Lerp(target.transform.localPosition, res, speed*Time.deltaTime);

    }
}

using UnityEngine;

public class JumpingObject : MonoBehaviour
{
    public Vector2 end;
    Vector2 start;

    public float jumpHeight;
    public float jumpSpeed;
    float timer = 0f;


    public float progress = 0f;

    public bool should = false;
    private void Start()
    {
        start = (Vector2)this.transform.position;
    }
    public void Reset()
    {
        this.transform.position = start;
        progress = 0f;
    }
    void Update()
    {
        if (!should) return;

        progress += Time.deltaTime * jumpSpeed;


        if (progress < 1f) { 
            this.transform.position = JumpUtility.GetJumpPosition(start,end,jumpHeight,progress);
        }
    }
}

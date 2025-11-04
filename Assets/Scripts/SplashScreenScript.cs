using UnityEngine;

public class SplashScreenScript : MonoBehaviour
{
    public float duration;
    float timer = 0f;

    public Animator animator;


    public void Splash(float duration) {
        timer = 0f;
        this.duration = duration;
        animator.Play("splash", 0, 0f);
    }
    public void Hide() {
        timer = 999f;
    }
    public void Update()
    {
        timer += Time.deltaTime;
        if (timer > duration) { 
            animator.Play("n", 0, 0f);
        }
    }
}

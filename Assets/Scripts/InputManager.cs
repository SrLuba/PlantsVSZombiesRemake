using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static InputManager instance;

    public Vector2 mousePosition;
    public void Awake()
    {
        instance = this;
    }


    void Start()
    {
        
    }

    void Update()
    {
        this.mousePosition = Mouse.current.position.ReadValue();
    }
}

using UnityEngine;


[CreateAssetMenu]
public class InputManager : ScriptableObject
{
    [SerializeField] private KeyCode Left, Right, Up, Down;

    public Vector2 GetInput()
    {
        int InputX = GetInputKey(Right) - GetInputKey(Left);
        int InputY = GetInputKey(Up) - GetInputKey(Down);

        return new Vector2(InputX, InputY);
    }

    private int GetInputKey(KeyCode key)
    {
        int input = 0;
        if (Input.GetKey(key)) input++;
        return input;
    }


    public bool HasJumped()
    {
        return Input.GetKeyDown(Up);
    }

    public bool IsJumping()
    {
        return Input.GetKey(Up);
    }
}

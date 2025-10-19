using UnityEngine;
using UnityEngine.InputSystem;

public class BasicPlayerController3d : MonoBehaviour
{
    public float movementScale;
    // 2. These variables are to hold the Action references
    InputAction moveAction;

    private void Start()
    {
        // 3. Find the references to the "Move" and "Jump" actions
        moveAction = InputSystem.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        var forward = this.transform.forward;
        var right = this.transform.right;

        Vector2 value = this.moveAction.ReadValue<Vector2>();

        this.transform.position += forward * value.y * this.movementScale;
        this.transform.position += right * value.x * this.movementScale;
        
    }
}

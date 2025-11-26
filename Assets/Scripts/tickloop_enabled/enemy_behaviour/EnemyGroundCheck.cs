using UnityEngine;

public class EnemyGroundCheck : MonoBehaviour
{
    private int groundLayer;
    private int platformLayer;
    private float groundOffset = 0.1f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        groundLayer = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public bool IsGrounded(CharacterController characterController)
    {
        if (characterController != null)
        {
            var radius = characterController.radius;
            var position = transform.position;
            position.y += -characterController.height / 2f + characterController.radius - characterController.skinWidth*2f;

            return Physics.CheckBox(position, radius * Vector3.one, transform.rotation,
                                    groundLayer, QueryTriggerInteraction.Ignore)
            || IsOnPlattform(characterController);
        }
        else
        {
            return false;
        }
    }

    public bool IsOnPlattform(CharacterController characterController)
    {
        if (characterController != null)
        {
            var radius = characterController.radius;
            var position = transform.position;
            position.y += -characterController.height / 2f + characterController.radius - characterController.skinWidth;

            return Physics.CheckBox(position, radius * Vector3.one, transform.rotation,
                                    platformLayer, QueryTriggerInteraction.Ignore);
        }
        else
        {
            return false;
        }
    }
}

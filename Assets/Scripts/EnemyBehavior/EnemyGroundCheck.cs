using UnityEngine;

public class EnemyGroundCheck : MonoBehaviour
{
    private int groundLayer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        groundLayer = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isGrounded(CharacterController enemy)
    {
        return Physics.CheckSphere(enemy.transform.position , enemy.radius, groundLayer, QueryTriggerInteraction.Ignore);
        
    }
}

using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveHandler : MonoBehaviour
{
    public CharacterController controller;
    public List<IMove> moves;
    public int currentMoveIndex = 0;
    public GameObject target;
    public float gravity = -35f;
    private Vector3 playerVelocity;

    public bool currentlyMoving;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<TickloopAddable>().triggeredByTickloop += Move;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentlyMoving)
        {
            if (moves[currentMoveIndex].Move())
            {
                currentMoveIndex = (currentMoveIndex + 1) % moves.Count;
                currentlyMoving = false;
                playerVelocity = Vector3.zero;
            }
        }

        if (!currentlyMoving)
        {
            playerVelocity.y += gravity * Time.deltaTime;
            Vector3 finalMove = playerVelocity.y * Vector3.up;
            controller.Move(finalMove * Time.deltaTime);
        }
    }
    
    void Move(Tickloop tp)
    {
        moves[currentMoveIndex].PrepareMove(controller, target, gravity);
        currentlyMoving = true;
    }
}

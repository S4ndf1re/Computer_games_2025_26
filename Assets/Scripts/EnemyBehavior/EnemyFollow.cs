using UnityEngine;

public class EnemyFollow : EnemyMove
{


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        nextState = State.waitingForJump;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(controller.isGrounded);
        if (nextState == State.walking)
        {
            moveDuration += Time.deltaTime;
            playerVelocity.y += gravityValue * Time.deltaTime;
            Vector3 move = new Vector3(1, 0, 0);
            move = Vector3.ClampMagnitude(move, 1.0f);
            // Combine horizontal and vertical movement
            Vector3 finalMove = (move * walkSpeed) + (playerVelocity.y * Vector3.up);
            controller.Move(finalMove * Time.deltaTime);
            if (moveDuration >= walkSpeed / walkDistance && controller.isGrounded)
            {
                nextState = State.waitingForJump;
            }
        }
        if (nextState == State.jumping)
        {
            moveDuration += Time.deltaTime;
            playerVelocity.y += gravityValue * Time.deltaTime;
            Vector3 move = new Vector3(1, 0, 0);
            // Combine horizontal and vertical movement
            Vector3 finalMove = (move * jumpSpeed) + (playerVelocity.y * Vector3.up);
            controller.Move(finalMove * Time.deltaTime);
        }

        if (nextState == State.waitingForWalk || nextState == State.waitingForJump)
        {
            playerVelocity.y += gravityValue * Time.deltaTime;


            Vector3 finalMove = playerVelocity.y * Vector3.up;
            controller.Move(finalMove * Time.deltaTime);
        }
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            if (nextState == State.jumping)
            {
                nextState = State.waitingForWalk;
            }

        }

    }
    public override void Move(GameObject enemy, GameObject target)
    {
        switch (nextState)
        {
            case State.waitingForWalk:
                if (controller.isGrounded)
                {
                    nextState = State.walking;
                    moveDuration = 0;
                    //StartCoroutine(WalkCoroutine(enemy, DetermineWalkPoint(enemy, target)));
                }
                break;
            case State.waitingForJump:
                if (controller.isGrounded)
                {
                    nextState = State.jumping;
                    playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
                    //StartCoroutine(JumpCoroutine(enemy, DetermineJumpPoint(enemy, target)));
                    moveDuration = 0;
                }

                break;
        }
    }

    private Vector3 getLaunchDirection()
    {
        //4 * x * (1 - x) * jumpHeight -> x == 0
        //- 4* jumpheight x^2 + 4*jumpheight x
        // derivation: -8 * jumpheight x + 4jumpheight -> f'(0) == 4*jumpHeight
        //tangent for f(x) == 20x
        return new Vector3(1, 20, 0);
    }

}

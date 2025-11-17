using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMoveHandler : MonoBehaviour
{
    public CharacterController enemy;
    public List<EnemyAct> moves;
    public int currentMoveIndex = 0;
    public GameObject target;
    public float gravity = -35f;
    public bool currentlyMoving;
    private Vector3 playerVelocity;
    public Hurtbox hurtbox;
    //public EnemyAct gettingHitMove;
    public EnemyAct idleMove;
    public bool currentlyHitMoving;
    public EnemyGroundCheck groundCheck;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        groundCheck = GetComponent<EnemyGroundCheck>();
        GetComponent<TickloopAddable>().triggeredByTickloop += StartMove;
        enemy = GetComponent<CharacterController>();
        if (hurtbox)
        {
            hurtbox.onHitTriggerEvent += OnHit;
        }
        idleMove.PrepareMove(enemy, null, gravity);
    }

    // Update is called once per frame
    void Update()
    {   //call current Moves Action
        if (currentlyMoving)
        {
            if (moves[currentMoveIndex].Move())
            {
                currentMoveIndex = (currentMoveIndex + 1) % moves.Count;
                currentlyMoving = false;
                playerVelocity = Vector3.zero;
                idleMove.PrepareMove(enemy, target, gravity);
            }
        }
        //if move finished, use gravity
        if (!currentlyMoving)
        {
            // if (currentlyHitMoving)
            // {
            //     if (gettingHitMove.Move())
            //     {
            //         currentlyHitMoving = false;
            //     }
            // }
            // else
            // {
            //     idleMove.Move();
            //     // playerVelocity.y += gravity * Time.deltaTime;
            //     // Vector3 finalMove = playerVelocity.y * Vector3.up;
            //     // controller.Move(finalMove * Time.deltaTime);
            // }
            idleMove.Move();

        }
    }
    /// <summary>
    /// Prepares the next Move so that it can be executed in Update, and activate Moving.
    /// </summary>
    /// <param name="tp"></param>
    void StartMove(Tickloop tp)
    {
        if (!currentlyHitMoving)
        {
            moves[currentMoveIndex].PrepareMove(enemy, target, gravity);
            currentlyMoving = true;
        }
    }

    /// <summary>
    /// Notifies the action that the hurtBox was being hit and the behavior should change accordingly. If there was no current executed action, the standard gettingHitAction will be executed.
    /// </summary>
    /// <param name="tp"></param>
    void OnHit(Hitbox hitbox)
    {
        if (currentlyMoving)
        {
            moves[currentMoveIndex].OnHit(hitbox);
        }
        else
        {
            // if (!currentlyHitMoving)
            // {
            //     gettingHitMove.PrepareMove(enemy, target, gravity);
            //     currentlyHitMoving = true;
            // }
            idleMove.OnHit(hitbox);
        }
    }
}

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMoveHandler : MonoBehaviour
{
    public CharacterController controller;
    public List<EnemyAct> moves;
    public int currentMoveIndex = 0;
    public GameObject target;
    public float gravity = -35f;
    private Vector3 playerVelocity;
    public Hurtbox hurtbox;

    public bool currentlyMoving;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<TickloopAddable>().triggeredByTickloop += StartMove;
        controller = GetComponent<CharacterController>();
        if (hurtbox)
        {
            hurtbox.onHitTriggerEvent += OnHit;
        }
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
            }
        }
        //if move finished, use gravity
        if (!currentlyMoving)
        {
            playerVelocity.y += gravity * Time.deltaTime;
            Vector3 finalMove = playerVelocity.y * Vector3.up;
            controller.Move(finalMove * Time.deltaTime);
        }
    }
    /// <summary>
    /// Prepares the next Move so that it can be executed in Update, and activate Moving.
    /// </summary>
    /// <param name="tp"></param>
    void StartMove(Tickloop tp)
    {
        moves[currentMoveIndex].PrepareMove(controller, target, gravity);
        currentlyMoving = true;
    }

    void OnHit(Hitbox hitbox)
    {
        if (currentlyMoving)
        {
            Debug.Log("WEEWOO");
            moves[currentMoveIndex].OnHit(hitbox);
        }

    }
}

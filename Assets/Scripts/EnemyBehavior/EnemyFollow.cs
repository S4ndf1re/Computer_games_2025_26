using UnityEngine;

public class EnemyFollow : EnemyMove
{


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nextState = State.walk;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void Move(GameObject enemy, GameObject target)
    {
        switch (nextState)
        {
            case State.walk:
                nextState = State.jump;
                StartCoroutine(WalkCoroutine(enemy, DetermineWalkPoint(enemy, target)));
                break;
            case State.jump:
                nextState = State.walk;
                StartCoroutine(JumpCoroutine(enemy, DetermineJumpPoint(enemy, target)));
                
                break;
        }
    }


}

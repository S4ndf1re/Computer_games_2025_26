using UnityEngine;

    /// <summary>
    /// Abstract Class <c>EnemyAct</c> should be implemented from Scripts that describe a Move an Enemy can do. EnemyMoveHandler calls these functions.
    /// </summary>
public abstract class EnemyAct : MonoBehaviour
{
    /// <summary>
    /// Makes the enemy move in the specified way. Is called from the Update-Method of EnemyMoveHandler. 
    /// <returns> Returns true when the move is finished. </returns>
    /// </summary>
    abstract public bool Move();
    /// <summary>
    /// Prepares the EnemyAct by instantiating all necessary variables for the Move. Called from EnemyMoveHandler when the Tickloop notifies it.
    /// <returns> Returns true when the move could be prepared so that it can be executed. </returns>
    /// </summary>
    abstract public bool PrepareMove(CharacterController enemy, GameObject target, float currentGravity);

}

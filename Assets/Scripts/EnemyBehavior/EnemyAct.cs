using UnityEngine;

public abstract class EnemyAct : MonoBehaviour
{
    abstract public bool Move();
    abstract public bool PrepareMove(CharacterController enemy, GameObject target, float currentGravity);

}

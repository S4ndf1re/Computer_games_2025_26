
using UnityEngine;

public interface IMove
{
    bool Move();
    bool PrepareMove(CharacterController enemy, GameObject target, float currentGravity);
}

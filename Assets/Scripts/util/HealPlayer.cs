using UnityEngine;

public class HealPlayer : MonoBehaviour
{ 
    public Player player;

    void OnTriggerEnter(Collider other)
    {
        player.Heal();
    }
}

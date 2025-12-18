using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public delegate void HitboxEntered();
    public event HitboxEntered hitboxEnteredEvent;

    public delegate void HitboxExit();
    public event HitboxExit hitboxExitEvent;

    public delegate void HitboxStay();
    public event HitboxStay hitboxStayEvent;

    public int damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        Hurtbox hurtbox = other.GetComponent<Hurtbox>();

        if (hurtbox != null)
        {
            hurtbox.EnterTrigger(this);
            hitboxEnteredEvent?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other) {
        Hurtbox hurtbox = other.GetComponent<Hurtbox>();

        if (hurtbox != null)
        {
            hurtbox.ExitTrigger(this);
            hitboxExitEvent?.Invoke();
        }
    }

    private void OnTriggerStay(Collider other) {
        Hurtbox hurtbox = other.GetComponent<Hurtbox>();

        if (hurtbox != null)
        {
            hurtbox.StayTrigger(this);
            hitboxStayEvent?.Invoke();
        }
    }
}

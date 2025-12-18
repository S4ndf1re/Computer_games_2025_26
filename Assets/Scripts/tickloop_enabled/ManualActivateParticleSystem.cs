using UnityEngine;

public class ManualActivateParticleSystem : MonoBehaviour
{
    public ParticleSystem system;
    public bool oneTimeUse = false;

    private bool used = false;

    void OnTriggerEnter(Collider other)
    {
        if (!used)
        {
            this.system.Play();
        }

        if (oneTimeUse)
        {
            used = true;
        }
    }
}

using StarterAssets;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Checkpoint currentCheckpoint;
    public bool withYAdjustment = false;
    public float newYThreshold = 0.0f;
    public bool collected = false;

    [Header("Collected Particles")]
    public ParticleSystem particles;

    void OnTriggerEnter(Collider other)
    {
        var respawnScript = other.GetComponent<RespawnPlayer>();
        if (respawnScript != null)
        {
            if ((currentCheckpoint == null || currentCheckpoint != this) && !collected)
            {
                respawnScript.startingPosition = other.transform.position;
                respawnScript.startingRotation = other.transform.rotation;
                if (withYAdjustment)
                {
                    respawnScript.yThreshold = newYThreshold;
                }
                currentCheckpoint = this;
                GetComponent<AudioSource>()?.Play();
                collected = true;
                if (particles != null)
                {
                    particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    particles.Play();
                }
            }
        }

    }
}

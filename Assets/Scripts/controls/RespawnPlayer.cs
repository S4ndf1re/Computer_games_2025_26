using UnityEngine;

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
    public class RespawnPlayer : MonoBehaviour
    {
        [Tooltip("The Y position threshold at which the player will respawn.")]
        public float yThreshold = -5f;

        public Vector3 startingPosition;

        public Quaternion startingRotation;

        private CharacterController characterController;
        private Velocity velocity;

        public AudioClip respawnSound;

        private Player playerScript;


        private void Start()
        {
            // Save the starting position and rotation
            startingPosition = transform.position;
            startingRotation = transform.rotation;

            characterController = GetComponent<CharacterController>();
            if (characterController == null)
            {
                Debug.LogError("CharacterController component is required for RespawnPlayer script!");
            }

            playerScript = GetComponent<Player>();
            if (playerScript == null)
            {
                Debug.LogError("Player-Script is required for RespawnPlayer!");
            }

            velocity = GetComponent<Velocity>();
            if (velocity == null)
            {
                Debug.LogError("Player-Script is required for RespawnPlayer!");
            }
        }

        private void Update()
        {
            // Check if the player's Y position has fallen below the threshold
            if (playerScript.GetHealth() <= 0)
            {
                playerScript.Respawn();
                Respawn();
            }
            if (transform.position.y < yThreshold)
            {
                Respawn();
            }
        }

        private void Respawn()
        {
            if (characterController != null)
            {
                characterController.enabled = false;
            }

            // Reset the player's position and rotation
            transform.position = startingPosition;
            transform.rotation = startingRotation;

            if (characterController != null)
            {
                characterController.enabled = true;
            }

            if (velocity != null)
            {
                velocity.ResetVelocity();
            }

            AudioSource.PlayClipAtPoint(respawnSound, transform.position);

        }
    }
}

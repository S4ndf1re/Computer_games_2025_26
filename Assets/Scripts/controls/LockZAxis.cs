using UnityEngine;

public class LockZAxis : MonoBehaviour
{

    CharacterController controller;
    float zStart = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        zStart = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        controller.enabled = false;
        var newPos = transform.position;
        newPos.z = zStart;
        transform.position = newPos;
        controller.enabled = true;
    }
}

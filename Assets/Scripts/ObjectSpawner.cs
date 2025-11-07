using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject prefab;
    [Tooltip("It is advised, that this is set to true, otherwise, the objects must get deleted manually. For example, DespawnAfterSeconds component may be used for this")]
    public bool withAutomaticDespawn = true;
    public float despawnTimeSeconds = 5.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<TickloopAddable>().triggeredByTickloop += Spawn;
    }

    void OnDisable() {
        GetComponent<TickloopAddable>().triggeredByTickloop -= Spawn;
    }


    void Spawn(Tickloop loop) {
        var obj = Instantiate(prefab, this.transform.position, Quaternion.identity);

        if (withAutomaticDespawn && despawnTimeSeconds > 0) {
            Destroy(obj, despawnTimeSeconds);
        }
    }
}

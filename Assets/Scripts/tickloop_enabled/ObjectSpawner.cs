using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject prefab;
    [Tooltip("It is advised, that this is set to true, otherwise, the objects must get deleted manually. For example, DespawnAfterSeconds component may be used for this")]
    public bool withAutomaticDespawn = true;
    public float despawnTimeSeconds = 5.0f;

    private TickloopAddable addable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        addable = GetComponent<TickloopAddable>();
        addable.triggeredByTickloop += Spawn;
    }

    void OnDisable()
    {
        addable.triggeredByTickloop -= Spawn;
    }


    void Spawn(Tickloop loop)
    {
        var obj = Instantiate(prefab, this.transform.position, Quaternion.identity);
        var renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.SetColor("_BaseColor", addable.color);
        }

        if (withAutomaticDespawn && despawnTimeSeconds > 0)
        {
            Destroy(obj, despawnTimeSeconds);
        }
    }
}

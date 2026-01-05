using UnityEngine;

public class EnableWhenInTickloop : MonoBehaviour
{

    public GameState state;
    public MonoBehaviour script;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        script.enabled = state.tickloopEnabled;
    }
}

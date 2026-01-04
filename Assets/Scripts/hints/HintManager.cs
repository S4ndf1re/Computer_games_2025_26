using UnityEngine;

public class HintManager : MonoBehaviour
{
    public static HintManager Instance { get; private set; }

    [Header("Setup")]
    [SerializeField] private HintController hintPrefab;
    [SerializeField] private Transform hintParent;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ShowHint(string message)
    {
        HintController hint = Instantiate(hintPrefab, hintParent);
        hint.Show(message);
    }
}

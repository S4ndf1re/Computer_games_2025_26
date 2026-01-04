using System;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    public static HintManager Instance { get; private set; }

    [Header("Setup")]
    [SerializeField] private HintController hintPrefab;
    [SerializeField] private Transform hintParent;
    public Player player;
    public string firstText;
    public string secondText;

    public const int firstHint = 2;
    public const int secondHint = 5;
    private int deathCounter;

    void Start()
    {
        deathCounter = 0;
        if (player != null)
        {
            player.playerRespawned += OnDeath;
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void OnDeath()
    {
        deathCounter += 1;
        switch(deathCounter)
        {
            case firstHint:
                ShowHint(firstText);
                break;
            case secondHint:
                ShowHint(secondText);
                break;
            default:
                break;
        }
    }

    public void ShowHint(string message)
    {
        HintController hint = Instantiate(hintPrefab, hintParent);
        hint.Show(message);
    }
}

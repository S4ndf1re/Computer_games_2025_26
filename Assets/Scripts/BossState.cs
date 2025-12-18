using Unity.VisualScripting;
using UnityEngine;

public class BossState : MonoBehaviour
{
    [Header("Control Elements")]
    public Player player;
    public TickloopAddable printHead;
    public TickloopAddable head;
    public TickloopAddable printHead2;
    public TickloopAddable head2;
    public TickloopAddable obstacles;


    [Header("Conditions")]
    public GameObject conditionDodge;

    public GameObject conditionPrinthead;
    public GameObject conditionPrintheadHard;
    public GameObject conditionPrintheadDodge;
    private TaskCondition conditionDodgePhase;

    private TaskCondition conditionPrintheadPhase;
    private TaskCondition conditionPrintheadHardPhase;
    private TaskCondition conditionPrintheadDodgePhase;

    public enum Phase
    {
        Dodge,
        Printhead,
        PrintheadHard,
        PrintheadDodge,
        Finished,
    }

    [Header("Phase Settings")]
    public Phase currentPhase;
    public int dodgeLoops = 5;
    public int printheadLoops = 5;
    public int printheadHardLoops = 5;
    public int printheadDodgeLoops = 5;

    // Internal counter
    private int maxLoopCountForPhase;
    private int currentLoop;
    private bool isNextPhase;

    void OnEnable()
    {
        player.playerRespawned += PlayerRespawned;
        GetComponent<TickloopAddable>().loopStart += OnLoopStart;
        GetComponent<TickloopAddable>().loopEnd += OnLoopEnd;

        conditionDodgePhase = conditionDodge.GetComponent<TaskCondition>();
        conditionPrintheadPhase = conditionPrinthead.GetComponent<TaskCondition>();
        conditionPrintheadHardPhase = conditionPrintheadHard.GetComponent<TaskCondition>();
        conditionPrintheadDodgePhase = conditionPrintheadDodge.GetComponent<TaskCondition>();
    }

    void OnDisable()
    {
        player.playerRespawned -= PlayerRespawned;
        GetComponent<TickloopAddable>().loopStart -= OnLoopStart;
        GetComponent<TickloopAddable>().loopEnd -= OnLoopEnd;
    }

    void Start()
    {
        SetupPhase();
        isNextPhase = true;
    }


    bool CanContinue()
    {
        bool canContinue = false;
        switch (this.currentPhase)
        {
            case Phase.Dodge:
                if (conditionDodgePhase != null)
                {
                    canContinue = conditionDodgePhase.TaskFinished();
                }
                else
                {
                    canContinue = true;
                }
                break;
            case Phase.Printhead:
                if (conditionPrintheadPhase != null)
                {
                    canContinue = conditionPrintheadPhase.TaskFinished();
                }
                else
                {
                    canContinue = true;
                }
                break;
            case Phase.PrintheadHard:
                if (conditionPrintheadHardPhase != null)
                {
                    canContinue = conditionPrintheadHardPhase.TaskFinished();
                }
                else
                {
                    canContinue = true;
                }
                break;
            case Phase.PrintheadDodge:
                if (conditionPrintheadDodgePhase != null)
                {
                    canContinue = conditionPrintheadDodgePhase.TaskFinished();
                }
                else
                {
                    canContinue = true;
                }
                break;
        }

        return canContinue;
    }

    void NextPhase()
    {
        if (!CanContinue())
        {
            return;
        }

        switch (this.currentPhase)
        {
            case Phase.Dodge:
                this.currentPhase = Phase.Printhead;
                break;
            case Phase.Printhead:
                this.currentPhase = Phase.PrintheadHard;
                break;
            case Phase.PrintheadHard:
                this.currentPhase = Phase.PrintheadDodge;
                break;
            case Phase.PrintheadDodge:
                this.currentPhase = Phase.Finished;
                break;
        }
        isNextPhase = true;
    }

    void SetupPhase()
    {
        switch (this.currentPhase)
        {
            case Phase.Dodge:
                SetupDodgePhase();
                break;
            case Phase.Printhead:
                SetupPrintheadPhase();
                break;
            case Phase.PrintheadHard:
                SetupPrintheadHardPhase();
                break;
            case Phase.PrintheadDodge:
                SetupPrintheadDodgePhase();
                break;
        }
    }

    void SetupDodgePhase()
    {
        maxLoopCountForPhase = dodgeLoops;
        currentLoop = 0;
        head.enabled = false;
        printHead.enabled = false;
        head2.enabled = false;
        printHead2.enabled = false;
        obstacles.enabled = true;
    }

    void SetupPrintheadPhase()
    {
        maxLoopCountForPhase = printheadLoops;
        currentLoop = 0;
        head.enabled = true;
        printHead.enabled = true;
        head2.enabled = false;
        printHead2.enabled = false;
        obstacles.enabled = false;
    }

    void SetupPrintheadHardPhase()
    {
        maxLoopCountForPhase = printheadHardLoops;
        currentLoop = 0;
        head.enabled = true;
        printHead.enabled = true;
        head2.enabled = true;
        printHead2.enabled = true;
        obstacles.enabled = false;
    }

    void SetupPrintheadDodgePhase()
    {
        maxLoopCountForPhase = printheadDodgeLoops;
        currentLoop = 0;
        head.enabled = true;
        printHead.enabled = true;
        head2.enabled = false;
        printHead2.enabled = false;
        obstacles.enabled = true;
    }

    public bool IsFinished()
    {
        return this.currentPhase == Phase.Finished;
    }

    void PlayerRespawned()
    {
        // Restart completely from the beginning. no checkpoints
        currentPhase = Phase.Dodge;
        isNextPhase = true;
    }

    void OnLoopStart(Tickloop loop)
    {
        if (isNextPhase)
        {
            SetupPhase();
            isNextPhase = false;
        }
    }

    void OnLoopEnd(Tickloop loop)
    {
        currentLoop += 1;
        if (currentLoop >= maxLoopCountForPhase)
        {
            NextPhase();
        }

    }
}

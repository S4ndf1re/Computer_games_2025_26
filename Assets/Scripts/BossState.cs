using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossState : MonoBehaviour
{
    [Header("Tickloop Control")]
    public Tickloop loop;

    [Header("Control Elements")]
    public Player player;
    public TickloopAddable printHead;
    public TickloopAddable head;
    public TickloopAddable printHead2;
    public TickloopAddable head2;
    public TickloopAddable obstacles;
    public TickloopAddable inkjetSideShooters;


    [Header("Conditions")]
    public GameObject conditionDodge;

    public GameObject conditionPrinthead;
    public GameObject conditionPrintheadHard;
    public GameObject conditionPrintheadDodge;
    private TaskCondition conditionDodgePhase;

    private TaskCondition conditionPrintheadPhase;
    private TaskCondition conditionPrintheadHardPhase;
    private TaskCondition conditionPrintheadDodgePhase;

    [Header("Restart")]
    public Animator transition;


    [Header("Task Creation")]
    public string dodgeTaskTitle;
    public string printheadTaskTitle;
    public string printheadHardTaskTitle;
    public string printheadDodgeTaskTitle;

    [Header("Game State")]
    public GameState state;

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
    private bool isNextPhase;

    private Coroutine sceneSwap;

    void OnEnable()
    {
        player.playerRespawned += PlayerRespawned;
        GetComponent<TickloopAddable>().loopStart += OnLoopStart;
        GetComponent<TickloopAddable>().loopEnd += OnLoopEnd;

        if (conditionDodge != null)
        {
            conditionDodgePhase = conditionDodge.GetComponent<TaskCondition>();
            conditionDodgePhase.GetInstance().Deactivate();
        }

        if (conditionPrinthead != null)
        {
            conditionPrintheadPhase = conditionPrinthead.GetComponent<TaskCondition>();
            conditionPrintheadPhase.GetInstance().Deactivate();
        }
        if (conditionPrintheadHard != null)
        {
            conditionPrintheadHardPhase = conditionPrintheadHard.GetComponent<TaskCondition>();
            conditionPrintheadHardPhase.GetInstance().Deactivate();
        }
        if (conditionPrintheadDodge != null)
        {
            conditionPrintheadDodgePhase = conditionPrintheadDodge.GetComponent<TaskCondition>();
            conditionPrintheadDodgePhase.GetInstance().Deactivate();
        }


        if (state != null)
        {
            if (state.currentBossPhase == null)
            {
                state.currentBossPhase = currentPhase;
            }
            else
            {
                currentPhase = (Phase)state.currentBossPhase;
            }
        }
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
    }


    void Update()
    {
        if (CanContinue() && !isNextPhase)
        {
            DeactivateAll();
            NextPhase();
        }
    }

    bool CanContinue()
    {
        bool canContinue = false;
        switch (this.currentPhase)
        {
            case Phase.Dodge:
                if (conditionDodgePhase != null)
                {
                    canContinue = conditionDodgePhase.GetInstance().TaskFinished();
                }
                break;
            case Phase.Printhead:
                if (conditionPrintheadPhase != null)
                {
                    canContinue = conditionPrintheadPhase.GetInstance().TaskFinished();
                }
                break;
            case Phase.PrintheadHard:
                if (conditionPrintheadHardPhase != null)
                {
                    canContinue = conditionPrintheadHardPhase.GetInstance().TaskFinished();
                }
                break;
            case Phase.PrintheadDodge:
                if (conditionPrintheadDodgePhase != null)
                {
                    canContinue = conditionPrintheadDodgePhase.GetInstance().TaskFinished();
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
            case Phase.Finished:
                SceneManager.LoadScene("EndScreen");
                break;
        }

        if (state != null)
        {
            state.currentBossPhase = currentPhase;
        }
    }

    void SetupDodgePhase()
    {
        head.enabled = false;
        printHead.enabled = false;
        head2.enabled = false;
        printHead2.enabled = false;
        obstacles.enabled = true;
        inkjetSideShooters.enabled = false;
        TaskListManager.Instance.SpawnTask(dodgeTaskTitle, conditionDodgePhase);
        conditionDodgePhase?.GetInstance().Reset();
        conditionDodgePhase?.GetInstance().Activate();
    }

    void SetupPrintheadPhase()
    {
        head.enabled = true;
        printHead.enabled = true;
        head2.enabled = false;
        printHead2.enabled = false;
        obstacles.enabled = false;
        inkjetSideShooters.enabled = true;
        TaskListManager.Instance.SpawnTask(printheadTaskTitle, conditionPrintheadPhase);
        conditionPrintheadPhase?.GetInstance().Reset();
        conditionPrintheadPhase?.GetInstance().Activate();
    }

    void SetupPrintheadHardPhase()
    {
        head.enabled = true;
        printHead.enabled = true;
        head2.enabled = true;
        printHead2.enabled = true;
        obstacles.enabled = false;
        inkjetSideShooters.enabled = true;
        TaskListManager.Instance.SpawnTask(printheadHardTaskTitle, conditionPrintheadHardPhase);
        conditionPrintheadHardPhase?.GetInstance().Reset();
        conditionPrintheadHardPhase?.GetInstance().Activate();
        player.Heal();
    }

    void SetupPrintheadDodgePhase()
    {
        Debug.Log("Setting up phase printhead dodge");
        head.enabled = true;
        printHead.enabled = true;
        head2.enabled = false;
        printHead2.enabled = false;
        obstacles.enabled = true;
        inkjetSideShooters.enabled = true;
        TaskListManager.Instance.SpawnTask(printheadDodgeTaskTitle, conditionPrintheadDodgePhase);
        Debug.Log("Resetting");
        conditionPrintheadDodgePhase?.GetInstance().Reset();
        Debug.Log("Activating");
        conditionPrintheadDodgePhase?.GetInstance().Activate();
    }

    public void DeactivateAll()
    {
        conditionDodgePhase?.GetInstance().Deactivate();
        conditionPrintheadPhase?.GetInstance().Deactivate();
        conditionPrintheadHardPhase?.GetInstance().Deactivate();
        conditionPrintheadDodgePhase?.GetInstance().Deactivate();
        head.enabled = false;
        printHead.enabled = false;
        head2.enabled = false;
        printHead2.enabled = false;
        obstacles.enabled = false;
        inkjetSideShooters.enabled = false;
        loop.SetToLastMeasure();
    }

    public bool IsFinished()
    {
        return this.currentPhase == Phase.Finished;
    }

    void PlayerRespawned()
    {
        // Restart completely from the beginning. no checkpoints
        // currentPhase = Phase.Dodge;
        // isNextPhase = true;

        conditionDodgePhase?.GetInstance().FinishTask();
        conditionPrintheadPhase?.GetInstance().FinishTask();
        conditionPrintheadHardPhase?.GetInstance().FinishTask();
        conditionPrintheadDodgePhase?.GetInstance().FinishTask();

        conditionDodgePhase?.GetInstance().Deactivate();
        conditionPrintheadPhase?.GetInstance().Deactivate();
        conditionPrintheadHardPhase?.GetInstance().Deactivate();
        conditionPrintheadDodgePhase?.GetInstance().Deactivate();
        if (sceneSwap == null)
        {
            sceneSwap = StartCoroutine(LoadScene());
        }

    }

    IEnumerator LoadScene()
    {
        transition.SetTrigger("start");

        yield return new WaitForSeconds(1.0f);
        yield return SceneManager.LoadSceneAsync("Boss");

        var scene = SceneManager.GetSceneByName("Boss");
        SceneManager.SetActiveScene(scene);

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
        // currentLoop += 1;
        // if (currentLoop >= maxLoopCountForPhase)
        // {
        //     NextPhase();
        // }

    }
}

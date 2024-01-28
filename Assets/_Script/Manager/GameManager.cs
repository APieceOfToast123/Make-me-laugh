using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.State;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

/// <summary>
/// Nice, easy to understand enum-based game manager. For larger and more complex games, look into
/// state machines. But this will serve just fine for most games.
/// 
/// </summary>
public class GameManager : StaticInstance<GameManager> {
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;
    private Coroutine moodCoroutine;//协程
    
    [Header("========== Units ==========")]
    public List<GameObject> Patients;
    public List<GameObject> Doctors;

    
    [Header("========== Normal setting  ==========")]
    [SerializeField]private float changeInterval = .5f;//修改心情的间隔时间
    [SerializeField] private Vector3 InitalDocPosition;
    [SerializeField] private Vector3 OffsetDocPosition;
    [SerializeField] private Vector3 InitalPatPosition;
    [SerializeField] private Vector3 OffsetPatPosition;
    
    private List<PaientManager> SettledPaients = new List<PaientManager>();

    [Header("========== Prefabs ==========")] 
    [SerializeField]private GameObject DocPrefab;
    [SerializeField]private GameObject PatPrefab;

    private bool isRoundingComplete;
    
    public int RoundCount { get; private set; }

    public void IncrementRound() {
        RoundCount++;
    }


    public GameState State { get; private set; }

    // Kick the game off with the first state
    void Start()
    {
        ChangeState(GameState.Starting);
    }


    //订阅事件
    private void OnEnable()
    {
        //添加监听事件
        EventManager.AddSettledOne += (manager) => SettledPaients.Add(manager);
        EventManager.CheckEffect += CheckEffect;
        EventManager.DeleteSettledOne += (manager) => SettledPaients.RemoveAt(SettledPaients.IndexOf(manager));
        EventManager.Charge += Charge;
        
        //只在第一个被放入的时候执行
        // EventManager.FirstOneSettled += () =>
        // {
        //     StartChangingMood(); // 启动协程
        //     EventManager.FirstOneSettled -= StartChangingMood; // 移除订阅
        // };

        EventManager.TrySettlePatient += OnTrySettlePatient;
    }

    //删除订阅
    private void OnDisable()
    {
        EventManager.AddSettledOne -= (manager) => SettledPaients.Add(manager);
        EventManager.CheckEffect -= CheckEffect;
        EventManager.DeleteSettledOne -= (manager) => SettledPaients.RemoveAt(SettledPaients.IndexOf(manager));
        EventManager.Charge -= Charge;
    }
    
    /// <summary>
    /// 改变所有的Settled paient的Mood by Tick
    /// </summary>
    private IEnumerator ChangeMoodOverTime()
    {
        while (State == GameState.Running) // 无限循环
        {
            Debug.Log("ChangeOverTime");
            yield return new WaitForSeconds(changeInterval); // 等待一定时间
            foreach (var VARIABLE in SettledPaients)
            {
                VARIABLE.TickChangeCurrentCount();
            }
        }
    }
    
    /// <summary>
    /// 开启ChangeMoodOverTime协程
    /// </summary>
    public void StartChangingMood()
    {
        moodCoroutine = StartCoroutine(ChangeMoodOverTime());
    }

    /// <summary>
    /// 关闭ChangeMoodOverTime协程
    /// </summary>
    public void StopDecreasingMood()
    {
        if (moodCoroutine != null)
        {
            StopCoroutine(moodCoroutine);
            moodCoroutine = null;
        }
    }

    public void CheckEffect()
    {
        foreach (var VARIABLE in SettledPaients)
        {
            VARIABLE.patientAttributes.tickEffectAmount = 0;
        }
        foreach (var VARIABLE in SettledPaients)
        {
            VARIABLE.changeEffectFactors();
        }
    }

    public void Charge()
    {
        
    }
    
    GameObject CreateUnit(GameObject prefab, Vector3 position)
    {
        if (prefab == null)
        {
            Debug.Log("No valid prefab");
            return null;
        }
        else
        {
            GameObject instance = Instantiate(prefab);
            instance.transform.position = position; // 设置实例的位置
            return instance;
        }
    }

    public void CompleteRounding()
    {
        if (State == GameState.Rounding)
        {
            isRoundingComplete = true;
        }
    }

    private void OnTrySettlePatient(bool flag)
    {
        if (flag)
        {
            Patients.RemoveAt(0);
            for (int i = 0; i < Patients.Count; i++)
            {
                Patients[i].transform.position = InitalPatPosition + i * OffsetPatPosition;
            }
            Patients[0].GetComponent<PaientManager>().sm.ChangeState(StateID.Selectable);
        }
        else
            Patients[0].transform.position = InitalPatPosition;
    }

    /// <summary>
    /// 倒计时x秒
    /// </summary>
    private IEnumerator CountdownCoroutine(int duration)
    {
        int remainingTime = duration;
        while (remainingTime > 0)
        {
        //    Debug.Log("Remaining time: " + remainingTime + " seconds");
            yield return new WaitForSeconds(1);
            remainingTime--;
        }
        // 当倒计时结束时，执行一些操作，比如改变游戏状态
        ChangeState(GameState.Rounding);
    }
    
    /// <summary>
    /// 每隔一段时间放置一个角色
    /// </summary>
    private IEnumerator SpawnPatPeriodically(float interval)
    {
        while (State == GameState.Running)
        {
            if (Patients.Count == 0)
            {
                GameObject theFirst = CreateUnit(PatPrefab, InitalPatPosition + Patients.Count * OffsetPatPosition);
                Patients.Add(theFirst);
                theFirst.GetComponent<PaientManager>().sm.ChangeState(StateID.Selectable);
            }
            Patients.Add(CreateUnit(PatPrefab,InitalPatPosition+Patients.Count*OffsetPatPosition));
            yield return new WaitForSeconds(interval);
        }
    }
    
    /// <summary>
    /// 等待直到isRoundingComlete
    /// </summary>
    private IEnumerator WaitForRoundingComplete()
    {
        Debug.Log("Rounding started, waiting for completion...");
        yield return new WaitUntil(() => isRoundingComplete);
        Debug.Log("Rounding completed.");

        // Rounding 完成后的逻辑，例如改变游戏状态
        ChangeState(GameState.Running);
    }

    public void ChangeState(GameState newState) {
        OnBeforeStateChanged?.Invoke(newState);

        State = newState;
        switch (newState) {
            case GameState.Starting:
                HandleStarting();
                break;
            case GameState.Running:
                HandleRunning();
                break;
            case GameState.Rounding:
                HandleRounding();
                break;
            case GameState.HeroTurn:
                HandleHeroTurn();
                break;
            case GameState.EnemyTurn:
                break;
            case GameState.Win:
                break;
            case GameState.Lose:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterStateChanged?.Invoke(newState);
        
        Debug.Log($"New state: {newState}");
    }

    private void HandleStarting() {
        
        //根据当前DocList的数量放置Doc的位置放置本游戏的Doctor
        Doctors.Add(CreateUnit(DocPrefab,InitalDocPosition+Doctors.Count*OffsetDocPosition));
        Doctors.Add(CreateUnit(DocPrefab,InitalDocPosition+Doctors.Count*OffsetDocPosition));
        Doctors.Add(CreateUnit(DocPrefab,InitalDocPosition+Doctors.Count*OffsetDocPosition));
        
        
        //开始游戏流程
        ChangeState(GameState.Running);
    }

    private void HandleRunning() {
        //StopAllCoroutines();
        isRoundingComplete = false;
        StartCoroutine(CountdownCoroutine(30)); // 启动30秒的倒计时
        StartCoroutine(SpawnPatPeriodically( 5f));
        StartCoroutine(ChangeMoodOverTime());
    }

    private void HandleRounding()
    {
        StopAllCoroutines();
        StartCoroutine(WaitForRoundingComplete());
    }

    private void HandleHeroTurn() {
        // If you're making a turn based game, this could show the turn menu, highlight available units etc
        
        // Keep track of how many units need to make a move, once they've all finished, change the state. This could
        // be monitored in the unit manager or the units themselves.
    }
    
    void OnGUI()
    {
        // 添加一些垂直空间，将内容向下移动
        GUILayout.Space(50); // 你可以根据需要调整这个值

        // 现在显示标签
        GUILayout.Label($"<color='black'><size=40>{State}</size></color>");
    }
}



/// <summary>
/// 游戏流程控制状态
/// </summary>
[Serializable]
public enum GameState {
    Starting = 0,
    Running = 1,
    Rounding = 2,
    HeroTurn = 3,
    EnemyTurn = 4,
    Win = 5,
    Lose = 6,
}
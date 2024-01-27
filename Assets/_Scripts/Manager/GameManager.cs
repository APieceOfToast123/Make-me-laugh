using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Nice, easy to understand enum-based game manager. For larger and more complex games, look into
/// state machines. But this will serve just fine for most games.
/// 
/// </summary>
public class GameManager : StaticInstance<GameManager> {
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;

    public static List<PaientManager> Paients;//管理生成的Paient列表
    private Coroutine moodCoroutine;//协程

    
    [Header("=========Change mood setting=========")]
    [SerializeField]private float changeInterval = .2f;//修改心情的间隔时间
    public List<PaientManager> SettledPaients;

    public GameState State { get; private set; }

    // Kick the game off with the first state
    //void Start() => ChangeState(GameState.Starting);

    
    //订阅事件
    private void OnEnable()
    {
        //添加监听事件
        EventManager.AddSettledOne += (manager) => SettledPaients.Add(manager);
        EventManager.CheckEffect += CheckEffect;
        
        //只在第一个被放入的时候执行
        EventManager.FirstOneSettled += () =>
        {
            StartChangingMood(); // 启动协程
            EventManager.FirstOneSettled -= StartChangingMood; // 移除订阅
        };
    }

    //删除订阅
    private void OnDisable()
    {
        EventManager.AddSettledOne -= (manager) => SettledPaients.Add(manager);
        EventManager.CheckEffect -= CheckEffect;
    }
    
    /// <summary>
    /// 改变所有的Settled paient的Mood by Tick
    /// </summary>
    private IEnumerator ChangeMoodOverTime()
    {
        while (true) // 无限循环
        {
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
            VARIABLE.attributes.tickEffectAmount = 0;
        }
        foreach (var VARIABLE in SettledPaients)
        {
            VARIABLE.changeEffectFactors();
        }
    }

    public void ChangeState(GameState newState) {
        OnBeforeStateChanged?.Invoke(newState);

        State = newState;
        switch (newState) {
            case GameState.Starting:
                HandleStarting();
                break;
            case GameState.SpawningHeroes:
                HandleSpawningHeroes();
                break;
            case GameState.SpawningEnemies:
                HandleSpawningEnemies();
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
        //Debug.Log("1");
        
        ChangeState(GameState.SpawningHeroes);
    }

    private void HandleSpawningHeroes() {
        //ExampleUnitManager.Instance.SpawnHeroes();
        
        ChangeState(GameState.SpawningEnemies);
    }

    private void HandleSpawningEnemies() {
        
        // Spawn enemies
        
        ChangeState(GameState.HeroTurn);
    }

    private void HandleHeroTurn() {
        // If you're making a turn based game, this could show the turn menu, highlight available units etc
        
        // Keep track of how many units need to make a move, once they've all finished, change the state. This could
        // be monitored in the unit manager or the units themselves.
    }
}



/// <summary>
/// 游戏流程控制状态
/// </summary>
[Serializable]
public enum GameState {
    Starting = 0,
    SpawningHeroes = 1,
    SpawningEnemies = 2,
    HeroTurn = 3,
    EnemyTurn = 4,
    Win = 5,
    Lose = 6,
}
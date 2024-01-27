using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.State;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PaientManager : MonoBehaviour
{
    public Attributes attributes;
    public LayerMask layermask;
    private PaientSM sm;
    
    public event Action<PaientManager> OnStateChanged;
    //TODO: 改写成系统自动检测到GM
    public GameManager GameManager;
    
    [Header("=========Change mood setting=========")]
    [SerializeField]private float changeInterval;//修改心情的间隔时间
    [SerializeField]private float currentChangeAmount;//每次修改心情的数量
    
    private void Awake()
    {
        attributes = new Attributes();
        // if (m_SendThisToGM == null)
        //     m_SendThisToGM = new UnityEvent<Transform>();
        // m_SendThisToGM.AddListener(GameManager);

        //新建状态机
        sm = new PaientSM(this.gameObject);
        
        //为状态机添加状态，第一个被添加的状态被视为初始状态
        sm.AddState(StateID.Seletable,new SSelectable(sm));
        sm.AddState(StateID.Settled,new SSettled("Settled",sm));
        sm.AddState(StateID.Laugh,new SLaugh(sm));
        sm.AddState(StateID.Cry,new SCry(sm));
        sm.AddState(StateID.Normal,new SNormal(sm));
        
    }
    private void Update()
    {
        if (sm.CurrentState != null)
            sm.Update();
    }
    public void changeMood(float changeMood)
    {
        attributes.mood += changeMood;
    }

    /// <summary>
    /// 影响半径范围内的其他病人，修改他们的收到的影响因子
    /// </summary>
    public void changeEffectFactors()
    {
        float radius;
        //判断当前状态是Laugh 还是 cry
        switch (sm.CurrentStateID)
        {
            case StateID.Laugh:
                radius = attributes.laughRadius;
                break;
            case StateID.Cry:
                radius = attributes.cryRadius;
                break;
            default:
                radius = 0f;
                Debug.Log("Not valid effect check");
                break;
        }
        
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, layermask);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<PaientManager>() != null)
            {
                hitCollider.GetComponent<PaientManager>().attributes.tickEffectAmount += this.attributes.effectFactors;
            }
        }
    }

    /// <summary>
    /// 每秒增减Mood
    /// </summary>
    public void TickChangeCurrentCount()
    {
        attributes.mood += attributes.tickEffectAmount;
    }


    void OnGUI()
    {
        GUI.color = Color.black;
        // 将角色的世界坐标转换为屏幕坐标
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        // 设置 GUI 文本的位置
        screenPosition.y = Screen.height - screenPosition.y; // 注意 Unity 屏幕坐标的 Y 轴是从下往上的
        screenPosition.y += 10; // 在角色的正上方 10 像素处显示
    
        // 绘制 GUI 文本
        GUI.Label(new Rect(screenPosition.x, screenPosition.y - 25, 100, 50), "Mood: " + attributes.mood);
    
        if (sm.CurrentState != null)
        {
            GUI.Label(new Rect(screenPosition.x, screenPosition.y - 35, 100, 50), "State: " + sm.CurrentState.name);
        }
    }
    
    void OnDrawGizmosSelected()
    {
        // 在编辑器中显示效果范围
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attributes.laughRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attributes.cryRadius);
    }

}

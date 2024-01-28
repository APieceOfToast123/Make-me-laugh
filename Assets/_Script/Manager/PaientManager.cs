using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.State;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PaientManager : UnitManager
{
    [FormerlySerializedAs("attributes")] 
    public PatientAttributes patientAttributes;
    public LayerMask patientLayer;
    public LayerMask bedLayer;
    public PatientSM sm;
    private Collider[] hitColliders;
    private Vector3 originalPosition;
    
    //TODO: 改写成系统自动检测到GM
    //
    //public Draggable Draggable;

    public float treatingForce;
    public DoctorManager TreatingDoctor;
    
    private void Awake()
    {
        originalPosition = this.transform.position;
//        print("Awake"+originalPosition);
        base.Awake();
//        Debug.Log("Awake");
        //TODO:目前方便测试，后续改回来
        //patientAttributes = new PatientAttributes();
        // if (m_SendThisToGM == null)
        //     m_SendThisToGM = new UnityEvent<Transform>();
        // m_SendThisToGM.AddListener(GameManager);

        //GameManager.OnBeforeStateChanged += OnStateChanged;
        //新建状态机
        sm = new PatientSM(this.gameObject);
        
        //为状态机添加状态，第一个被添加的状态被视为初始状态
        sm.AddState(StateID.waiting,new SWaiting(sm));
        sm.AddState(StateID.Selectable,new SSelectable(sm));
        //sm.AddState(StateID.Settled,new SSettled("Settled",sm));
        sm.AddState(StateID.Laugh,new SLaugh(sm));
        sm.AddState(StateID.Cry,new SCry(sm));
        sm.AddState(StateID.Normal,new SNormal(sm));
        sm.AddState(StateID.fail,new SFail(sm));
        sm.AddState(StateID.Complete,new SComplete(sm));


        treatingForce = 0f;
    }

    private void OnDestroy()
    {
        base.OnDestroy();
    }

    private void OnStateChanged(GameState newState)
    {
        if (newState == GameState.Starting)
            Debug.Log("Start");
    }
    
    private void Update()
    {
        if (sm.CurrentState != null)
            sm.Update();
    }
    public void changeMood(float changeMood)
    {
        patientAttributes.mood += changeMood;
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
                radius = patientAttributes.laughRadius;
                break;
            case StateID.Cry:
                radius = patientAttributes.cryRadius;
                break;
            default:
                radius = 0f;
                //Debug.Log("Not valid effect check");
                break;
        }
        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, patientLayer);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<PaientManager>() != null)
            {
                hitCollider.GetComponent<PaientManager>().patientAttributes.tickEffectAmount += this.patientAttributes.effectFactors;
            }
        }

        this.patientAttributes.tickEffectAmount += treatingForce;
    }

    public bool CheckStateID(StateID checkState)
    {
        if (sm.CurrentStateID.Equals(checkState))
            return true;
        return false;
    }

    /// <summary>
    /// 每秒增减Mood
    /// </summary>
    public void TickChangeCurrentCount()
    {
        patientAttributes.mood += (patientAttributes.tickEffectAmount+treatingForce);
    }

    //TODO:以后有空绝对要升级
    /// <summary>
    /// 清空自身的TreatForce
    /// </summary>
    public void ClearTreatForce()
    {
        treatingForce = 0f;
        if (TreatingDoctor != null)
        {
            TreatingDoctor.lastTimeVaildHit = null;
            TreatingDoctor = null;
        }
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
    
    public bool CheckNearbyBeds()
    {
        hitColliders = Physics.OverlapSphere(transform.position, patientAttributes.checkRadius, bedLayer);
        return hitColliders.Length > 0;
    }

    public Collider[] GetNearBeds()
    {
        return Physics.OverlapSphere(transform.position, patientAttributes.checkRadius, bedLayer);
    }

    public Vector3 GetBedPosition()
    {
        if (hitColliders.Length > 0)
        {
            return hitColliders[0].transform.position;
        }
        else
            return new Vector3();
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
        GUI.Label(new Rect(screenPosition.x, screenPosition.y - 25, 100, 50), "Mood: " + patientAttributes.mood);
    
        if (sm.CurrentState != null)
        {
            GUI.Label(new Rect(screenPosition.x, screenPosition.y - 35, 100, 50), "State: " + sm.CurrentState.name);
        }
    }
    
    void OnDrawGizmosSelected()
    {
        // 在编辑器中显示效果范围
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, patientAttributes.laughRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, patientAttributes.cryRadius);
    }

}

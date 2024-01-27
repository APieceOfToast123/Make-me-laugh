using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.State;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class DoctorManager : UnitManager
{
    [Header("========== 医生属性 ==========")]
    public DoctorAttributes attributes;
    public LayerMask PatientLayer;
    private DoctorSM sm;
    
    //TODO: 改写成系统自动检测到GM
    //public GameManager GameManager;
    public Vector3 originalPosition;

    [SerializeField]private Collider[] hitColliders;
    public Transform firstVaildHit;
    public Transform lastTimeVaildHit;
    public Vector3 posOffset;
    
    
    private void Awake()
    {
        base.Awake();
        //TODO:目前方便测试，后续改回来
        //attributes = new DoctorAttributes();
        
        //新建状态机
        sm = new DoctorSM(this.gameObject);
        
        //为状态机添加状态，第一个被添加的状态被视为初始状态
        sm.AddState(StateID.DocSelectable,new SDocSelectable(sm));
        sm.AddState(StateID.DocTreating,new SDocTreating(sm));

        firstVaildHit = null;
    }
    
    private void OnDestroy()
    {
        base.OnDestroy();
    }

    public override void HandleBeforeRunning()
    {
        base.HandleBeforeRunning();
        Debug.Log("Entered");
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (sm.CurrentState != null)
            sm.Update();
    }

    public void CheckNearbyPaitents()
    {
        hitColliders = Physics.OverlapSphere(transform.position, attributes.radius, PatientLayer);
        if (hitColliders.Length > 0)
        {
            foreach (var VARIABLE in hitColliders)
            {
                PaientManager pm = VARIABLE.transform.GetComponent<PaientManager>();
                if (pm != null)
                {
                    if (pm.CheckStateID(StateID.Laugh) || pm.CheckStateID(StateID.Normal) ||
                        pm.CheckStateID(StateID.Cry))
                    {
                        firstVaildHit = VARIABLE.transform;
                    }
                }
            }
        }
        else
            firstVaildHit = null;
    }

    public void Trans2Pat()
    {
        Debug.Log("Trans");
        this.transform.position = firstVaildHit.position + posOffset;
    }

    public void Relocate()
    {
        Debug.Log("Relocate");
        this.transform.position = originalPosition;
    }

    public void AddTreatingForce()
    {
        if (firstVaildHit != null && sm.CurrentStateID.Equals(StateID.DocTreating))
        {
            lastTimeVaildHit = firstVaildHit;
            lastTimeVaildHit.GetComponent<PaientManager>().treatingForce = attributes.treatEffect;
            lastTimeVaildHit.GetComponent<PaientManager>().TreatingDoctor = this;
            //lastTimeVaildHit.GetComponent<PaientManager>().patientAttributes.tickEffectAmount += attributes.treatEffect;
        }
    }

    void OnGUI()
    {
        GUI.color = Color.black;
        // 将角色的世界坐标转换为屏幕坐标
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        // 设置 GUI 文本的位置
        screenPosition.y = Screen.height - screenPosition.y; // 注意 Unity 屏幕坐标的 Y 轴是从下往上的
        screenPosition.y += 10; // 在角色的正上方 10 像素处显示

        if (sm.CurrentState != null)
        {
            GUI.Label(new Rect(screenPosition.x, screenPosition.y - 35, 100, 50), "State: " + sm.CurrentState.name);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position,attributes.radius);
    }

}

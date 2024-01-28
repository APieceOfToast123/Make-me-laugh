using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.State;
using UnityEngine;
using UnityEngine.Events;

public class SSelectable : BasicState
{
    protected PatientSM sm;
    private PaientManager pm;
    
    private bool dragging = false;
    private Vector3 offset;
    private float zDistanceToCamera;
    
    private float timer; // 添加计时器
    private float duration = 5f; // 设置状态持续时间，例如5秒
    
    public SSelectable(PatientSM stateMachine) : base("Selectable", stateMachine)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        sm = (PatientSM)stateMachine;
        if (sm.OwnerGo.GetComponent<PaientManager>() != null)
        {
            pm = sm.OwnerGo.GetComponent<PaientManager>();
        }
        timer = duration; // 初始化计时器
    }

    public override void Action()
    {
        base.Action();

        Camera cam = Camera.main;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out RaycastHit hitInfo) && hitInfo.collider.gameObject == sm.OwnerGo)
            {
                zDistanceToCamera = cam.WorldToScreenPoint(sm.OwnerGo.transform.position).z;
                Plane plane = new Plane(cam.transform.forward, new Vector3(0, sm.OwnerGo.transform.position.y, 0));
            
                if (plane.Raycast(ray, out float enter))
                {
                    Vector3 hitPoint = ray.GetPoint(enter);
                    offset = sm.OwnerGo.transform.position - hitPoint;
                    dragging = true;
                }
            }
        }

        if (dragging && Input.GetMouseButton(0))
        {
            if (new Plane(cam.transform.forward, new Vector3(0, sm.OwnerGo.transform.position.y, 0)).Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                Vector3 newPosition = hitPoint + offset;
                newPosition.y = sm.OwnerGo.transform.position.y; // 保持 y 坐标不变
                sm.OwnerGo.transform.position = newPosition;
            }
        }
        
        // 更新计时器
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    public override void Check()
    {
        base.Check();
        if (dragging && Input.GetMouseButtonUp(0))
        {
            dragging = false;
            if (pm.CheckNearbyBeds() != null)
            {
                sm.ChangeState(StateID.Normal);
//                GameManager.SettledBeds.Add(pm.hitColliders[0].gameObject);
                //pm.transform.position = pm.GetNearBeds()[0].transform.position;
            }
        }
        // if (timer <= 0)
        // {
        //     // 计时结束，改变状态
        //     pm.patientAttributes.mood = 39;
        //     sm.ChangeState(StateID.Cry);
        //     
        //     // 获取存在于list1中但不在list2中的元素
        //     List<GameObject> difference = ObjectPlacer.placedGameObjects.Except(GameManager.SettledBeds).ToList();
        //     if (difference.Count >0)
        //     {
        //         pm.transform.position = difference[0].transform.position;
        //         EventManager.CallTrySettlePatient(true);
        //     }
        // }
    }

    public override void OnExit()
    {
        base.OnExit();
        EventManager.CallTrySettlePatient(pm.CheckNearbyBeds());
        EventManager.CallAddSettledOne(sm.OwnerGo.GetComponent<PaientManager>());
        EventManager.CallFirstOneSetteld();
    }
}

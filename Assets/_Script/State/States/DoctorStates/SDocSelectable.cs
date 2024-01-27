using System.Collections;
using System.Collections.Generic;
using _Scripts.State;
using UnityEngine;
using UnityEngine.Events;

public class SDocSelectable : BasicState
{
    protected DoctorSM sm;
    private DoctorManager dm;
    
    private bool dragging = false;
    //偏移值
    private Vector3 offset;
    private float zDistanceToCamera;
    
    public SDocSelectable(DoctorSM stateMachine) : base("Selectable", stateMachine)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        sm = (DoctorSM)stateMachine;
        if (sm.OwnerGo.GetComponent<DoctorManager>() != null)
        {
            dm = sm.OwnerGo.GetComponent<DoctorManager>();
        }

        //进入可选状态锁起点
        dm.originalPosition = dm.transform.position;
    }

    public override void Action()
    {
        base.Action();

        //TODO: 移动的逻辑有BUG
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
        
        //TODO: 有点垃圾
        dm.CheckNearbyPaitents();
    }

    public override void Check()
    {
        base.Check();
        if (dragging && Input.GetMouseButtonUp(0))
        {
            dragging = false;
            
            if (dm.firstVaildHit == null)
            {
                dm.Relocate();
            }
            else
            {
                if (dm.firstVaildHit.transform.GetComponent<PaientManager>().TreatingDoctor != null)
                {
                    dm.Relocate();
                }
                // 只有在有碰撞物体，并且物体不具有主治医生的时候，可以
                else
                {
                    sm.ChangeState(StateID.DocTreating); 
                }
            }
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        // if (dm.firstVaildHit != null)
        // {
        //     EventManager.CallAddSettledOne(dm.firstVaildHit.GetComponent<PaientManager>());
        // }
    }
}

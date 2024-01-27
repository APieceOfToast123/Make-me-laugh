using System.Collections;
using System.Collections.Generic;
using _Scripts.State;
using UnityEngine;
using UnityEngine.Events;

public class SDocTreating : BasicState
{
    protected DoctorSM sm;
    private DoctorManager dm;
    
    private bool dragging = false;
    //偏移值
    private Vector3 offset;
    private float zDistanceToCamera;
    
    public SDocTreating(DoctorSM stateMachine) : base("SDocTreating", stateMachine)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        sm = (DoctorSM)stateMachine;
        dm = sm.OwnerGo.GetComponent<DoctorManager>();
        if (dm.firstVaildHit != null)
        {
            dm.AddTreatingForce();
        }
    }

    public override void Action()
    {
        base.Action();
    }

    public override void Check()
    {
        base.Check();
        
        //TODO: 移动的逻辑有BUG
        Camera cam = Camera.main;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out RaycastHit hitInfo) && hitInfo.collider.gameObject == sm.OwnerGo)
            {
                sm.ChangeState(StateID.DocSelectable);
            }
        }

        if (dm.lastTimeVaildHit == null)
        {
            sm.ChangeState(StateID.DocSelectable);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        if (dm.lastTimeVaildHit != null)
        {
            dm.lastTimeVaildHit.GetComponent<PaientManager>().ClearTreatForce();
        }
        
    }
}

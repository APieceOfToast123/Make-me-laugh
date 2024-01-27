using System.Collections;
using System.Collections.Generic;
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
    }

    public override void Check()
    {
        base.Check();
        if (dragging && Input.GetMouseButtonUp(0))
        {
            dragging = false;
            sm.ChangeState(StateID.Normal);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        EventManager.CallAddSettledOne(sm.OwnerGo.GetComponent<PaientManager>());
        EventManager.CallFirstOneSetteld();
    }
}

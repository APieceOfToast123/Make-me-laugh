using System.Collections;
using System.Collections.Generic;
using _Scripts.State;
using UnityEngine;

public class SSelectable : BasicState
{
    protected PaientSM sm;
    private float distance;
    private bool dragging = false;
    
    public SSelectable(PaientSM stateMachine) : base("Selectable", stateMachine)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        sm = (PaientSM)stateMachine;
    }

    public override void Action()
    {
        base.Action();
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 当鼠标按下且光标在对象上时开始拖拽
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D collider = sm.OwnerGo.GetComponent<Collider2D>();
            Debug.Log("Clicked");
            if (collider != null && collider.OverlapPoint(mousePosition))
            {
                distance = sm.OwnerGo.transform.position.z - Camera.main.transform.position.z;
                dragging = true;
            }
        }

        // 拖拽时更新对象位置
        if (dragging && Input.GetMouseButton(0))
        {
            Vector3 newPos = new Vector3(mousePosition.x, mousePosition.y, distance);
            sm.OwnerGo.transform.position = newPos;
        }
    }

    public override void Check()
    {
        base.Check();
        // 释放鼠标时停止拖拽
        if (dragging && Input.GetMouseButtonUp(0))
        {
            dragging = false;
            sm.ChangeState(StateID.Laugh);
        }
    }
}

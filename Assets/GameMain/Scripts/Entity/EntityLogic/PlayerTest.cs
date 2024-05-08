using System.Collections;
using System.Collections.Generic;
using GameMain.Scripts.Entity.EntityData;
using UnityEngine;
using UnityEngine.AI;

public class PlayerTest : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;
        
    private static readonly int Speed = Animator.StringToHash("Speed");

    public void Update()
    {
        animator.SetFloat(Speed, agent.velocity.magnitude);

        if (Input.GetMouseButton(0))
        {
            var mousePosition = Input.mousePosition;
            
            var ray = Camera.main.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                agent.SetDestination(hit.point);
            }
        }
    }
}

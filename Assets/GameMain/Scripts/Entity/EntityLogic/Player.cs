using System;
using GameMain.Scripts.Entity.EntityData;
using GameMain.Scripts.Event;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using UnityGameFramework.Runtime;

namespace GameMain.Scripts.Entity.EntityLogic
{
    public enum NavMeshStatus
    {
        Running,
        Complete
    }
    
    public class Player : Entity
    {
        [SerializeField, HideInInspector] 
        private PlayerData playerData;

        [SerializeField]
        private Animator animator;
        [SerializeField]
        private NavMeshAgent agent;

        private Vector2 smoothDeltaPosition;
        private Vector2 velocity;

        public ReactiveProperty<NavMeshStatus> pathStatus = new ReactiveProperty<NavMeshStatus>()
            { Value = NavMeshStatus.Complete };

        private const float TurningSpeed = 240f;
        
        private static readonly int Forward = Animator.StringToHash("Forward");
        private static readonly int Turn = Animator.StringToHash("Turn");

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            playerData = userData as PlayerData;
            
            pathStatus.Subscribe(status =>
            {
                if (status == NavMeshStatus.Complete)
                {
                    var Event = GameEntry.GetComponent<EventComponent>();
                    Event.Fire(this, PlayerArriveEventArgs.Create());
                }
            });
            
            agent.updatePosition = false;
            agent.updateRotation = true;
            
            if (NavMesh.SamplePosition(playerData.Position, out var hit, 1.0f, NavMesh.AllAreas))
            {
                // 获取最近的NavMesh位置
                var nearestPos = hit.position;
 
                // 设置代理的目标位置为最近的NavMesh位置
                agent.Warp(nearestPos);
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) 
            {
                pathStatus.Value = NavMeshStatus.Complete;
                
                MovePlayer(Vector3.zero);
            }
            else
            {
                pathStatus.Value = NavMeshStatus.Running;
                
                MovePlayer(agent.velocity);
            }
        }
        
        private void MovePlayer(Vector3 move)
        {
            if (move.magnitude > 1f) {
                move.Normalize();
            }

            move = transform.InverseTransformDirection(move);

            var turningAmount = Mathf.Atan2(move.x, move.z);

            var moveAmount = move.z;

            if (pathStatus.Value == NavMeshStatus.Running)
            {
                transform.Rotate(0, turningAmount * TurningSpeed * Time.deltaTime, 0);
            }
            
            animator.SetFloat(Forward, moveAmount, 0.1f, Time.deltaTime);
            animator.SetFloat(Turn, turningAmount, 0.1f, Time.deltaTime);
        }

        private void OnAnimatorMove()
        {
            var rootPosition = animator.rootPosition;
            rootPosition.y = agent.nextPosition.y;
            transform.position = rootPosition;
            agent.nextPosition = rootPosition;
        }

        public void FootL()
        {
            
        }

        public void FootR()
        {
            
        }

        public void SetDestination(Vector3 position)
        {
            agent.SetDestination(position);
        }
    }
}
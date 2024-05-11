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

        public Animator animator;
        public NavMeshAgent agent;

        public ReactiveProperty<NavMeshStatus> pathStatus = new ReactiveProperty<NavMeshStatus>()
            { Value = NavMeshStatus.Complete };
        
        private static readonly int Speed = Animator.StringToHash("Speed");
        
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
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            
            animator.SetFloat(Speed, agent.velocity.magnitude);

            if (!agent.pathPending && agent.remainingDistance < 0.1f) 
            {
                pathStatus.Value = NavMeshStatus.Complete;
            }
            else
            {
                pathStatus.Value = NavMeshStatus.Running;
            }
        }

        public void SetDestination(Vector3 position)
        {
            agent.SetDestination(position);
        }
    }
}
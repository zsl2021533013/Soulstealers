using GameMain.Scripts.Entity.EntityData;
using UnityEngine;
using UnityEngine.AI;
using UnityGameFramework.Runtime;

namespace GameMain.Scripts.Entity.EntityLogic
{
    public class Player : Entity
    {
        [SerializeField, HideInInspector] 
        private PlayerData playerData;

        public Animator animator;
        public NavMeshAgent agent;
        
        private static readonly int Speed = Animator.StringToHash("Speed");

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            playerData = userData as PlayerData;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            
            animator.SetFloat(Speed, agent.velocity.magnitude);
        }

        public bool SetDestination(Vector3 position)
        {
            return agent.SetDestination(position);
        }
    }
}
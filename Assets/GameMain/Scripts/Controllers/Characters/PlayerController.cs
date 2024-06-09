using GameMain.Scripts.Controller;
using GameMain.Scripts.Event;
using GameMain.Scripts.Model;
using GameMain.Scripts.Utility;
using QFramework;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.AI;

namespace GameMain.Scripts.Entity.EntityLogic
{
    public class PlayerController : ControllerBase
    {
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private NavMeshAgent agent;
        [SerializeField]
        private Transform cameraPoint;

        private Vector2 smoothDeltaPosition;
        private Vector2 velocity;
        
        public ReactiveProperty<NavMeshStatus> pathStatus = new ReactiveProperty<NavMeshStatus>()
        {
            Value = NavMeshStatus.Complete
        };

        private const float TurningSpeed = 240f;
        
        private static readonly int Forward = Animator.StringToHash("Forward");
        private static readonly int Turn = Animator.StringToHash("Turn");
        
        public override void OnGameInit()
        {
            cameraPoint.Parent(null);
            cameraPoint.UpdateAsObservable().Subscribe(unit =>
            {
                cameraPoint.transform.position = 
                    Vector3.Lerp(cameraPoint.transform.position, transform.position, 0.02f);
            });
            
            agent.updatePosition = false;
            agent.updateRotation = true;
            
            if (NavMesh.SamplePosition(transform.position, out var hit, 1.0f, NavMesh.AllAreas))
            {
                var nearestPos = hit.position;
 
                agent.Warp(nearestPos);
            }
            
            pathStatus.Subscribe(status =>
            {
                var model = this.GetModel<PlayerModel>();
                model.pathStatus.Value = pathStatus.Value; // 同步
            });
        }

        public override void OnUpdate(float elapse)
        {
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
            animator.applyRootMotion = true;
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
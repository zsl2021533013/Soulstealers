using System.Collections.Generic;
using EPOOutline;
using GameMain.Scripts.Controller;
using GameMain.Scripts.Utility;
using Newtonsoft.Json;
using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using QFramework;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

namespace GameMain.Scripts.Entity.EntityLogic
{
    public class NPCController : ControllerBase
    {
        [SerializeField] private DialogueTreeController controller;
        [SerializeField] private Blackboard blackboard;
        [SerializeField] private Transform dialoguePoint;
        [SerializeField] private Outlinable outline;

        [SerializeField, BoxGroup("Movable")] private bool isMovable = true;
        
        [SerializeField, ShowIf("isMovable", true), BoxGroup("Movable")] private Animator animator;
        [SerializeField, ShowIf("isMovable", true), BoxGroup("Movable")] private NavMeshAgent agent;
        
        public ReactiveProperty<NavMeshStatus> pathStatus = new ReactiveProperty<NavMeshStatus>()
        {
            Value = NavMeshStatus.Complete
        };
        
        private const float TurningSpeed = 240f;
        
        private static readonly int Forward = Animator.StringToHash("Forward");
        private static readonly int Turn = Animator.StringToHash("Turn");
        
        
        public void OnGameInit()
        {
        }

        public void OnUpdate(float elapse)
        {
            if (!isMovable)
            {
                return;
            }
            
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

        public void OnFixedUpdate(float elapse)
        {
        }

        public void OnGameShutdown()
        {
        }

        public Dictionary<string, object> GetData()
        {
            var blackboardSerialize = blackboard.Serialize(null, true);
            var active = gameObject.activeSelf; 
            var position = transform.position;
            var rotation = transform.rotation;
            var tag = transform.tag;
            
            var data = new Dictionary<string, object>
            {
                { "blackboard", blackboardSerialize },
                { "active", active},
                { "position", position},
                { "rotation", rotation},
                { "tag", tag }
            };

            return data;
        }

        public void LoadData(Dictionary<string, object> data)
        {
            var a = data["blackboard"];
            Debug.Log(a.GetType());
            
            var blackboardSerialize = (string)data["blackboard"];
            blackboard.Deserialize(blackboardSerialize, null, false);
            
            var active = (bool)data["active"];
            gameObject.SetActive(active);

            var position = (Vector3)data["position"];
            transform.position = position;

            var rotation = (Quaternion)data["rotation"];
            transform.rotation = rotation;

            transform.tag = (string)data["tag"];
        }
        
        public void StartDialogue()
        {
            if (!controller.isRunning)
            {
                controller.StartDialogue();
            }
        }

        public void EnableOutline()
        {
            if (outline)
            {
                outline.enabled = true;
            }
        }

        public void DisableOutline()
        {
            if (outline)
            {
                outline.enabled = false;
            }
        }

        public Vector3 GetDialoguePoint()
        {
            return dialoguePoint.position;
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
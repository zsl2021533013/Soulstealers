using System.Collections.Generic;
using EPOOutline;
using GameMain.Scripts.Controller;
using GameMain.Scripts.Game;
using GameMain.Scripts.Model;
using GameMain.Scripts.Scriptable_Object;
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
using UnityEngine.Serialization;

namespace GameMain.Scripts.Entity.EntityLogic
{
    public class NPCController : ControllerBase
    {
        [SerializeField] private DialogueTreeController dialogueTree;
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
        
        public override void OnGameInit()
        {
            if (isMovable)
            {
                agent.updatePosition = false;
                agent.updateRotation = true;
            }
        }

        public override void OnUpdate(float elapse)
        {
            if (!isMovable || !gameObject.activeInHierarchy)
            {
                return;
            }
            
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) 
            {
                pathStatus.Value = NavMeshStatus.Complete;
                
                MoveNPC(Vector3.zero);
            }
            else
            {
                pathStatus.Value = NavMeshStatus.Running;
                
                MoveNPC(agent.velocity);
            }
        }
        
        private void MoveNPC(Vector3 move)
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
        
        public NPCData GetData()
        {
            var dialogueTreeData = dialogueTree.GetData();
            var blackboardData = blackboard.GetData();
            var active = gameObject.activeSelf; 
            var position = transform.position;
            var rotation = transform.rotation;
            var tag = transform.tag;
            
            var data = new NPCData
            {
                dialogueTreeData = dialogueTreeData,
                blackboardData = blackboardData,
                active = active,
                position = position,
                rotation = rotation,
                tag = tag
            };

            return data;
        }

        public void LoadData(NPCData data)
        {
            var dialogueTreeData = data.dialogueTreeData;
            dialogueTree.LoadData(dialogueTreeData);
            
            var blackboardData = data.blackboardData;
            blackboard.LoadData(blackboardData);
            
            var active = data.active;
            gameObject.SetActive(active);

            var position = data.position;
            transform.position = position;

            var rotation = data.rotation;
            transform.rotation = rotation;

            transform.tag = data.tag;
        }
        
        public void StartDialogue()
        {
            if (!dialogueTree.isRunning)
            {
                dialogueTree.StartDialogue();
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
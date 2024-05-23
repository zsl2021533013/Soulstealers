using EPOOutline;
using GameMain.Scripts.Controller;
using GameMain.Scripts.Utility;
using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using QFramework;
using UnityEngine;

namespace GameMain.Scripts.Entity.EntityLogic
{
    public class NPCController : MonoBehaviour, IController, ISoulstealersGameController
    {
        [SerializeField]
        private DialogueTreeController controller;

        [SerializeField] 
        private Blackboard blackboard;

        [SerializeField] 
        private Transform dialoguePoint;

        [SerializeField]
        private Outlinable outline;
        
        public void OnGameInit()
        {
        }

        public void OnUpdate(float elapse)
        {
        }

        public void OnFixedUpdate(float elapse)
        {
        }

        public void OnGameShutdown()
        {
        }

        public string Serialize()
        {
            return blackboard.Serialize(null);
        }

        public void Deserialize(string t)
        {
            blackboard.Deserialize(t, null);
        }
        
        public void StartDialogue()
        {
            if (!controller.isRunning)
            {
                controller.StartDialogue();
            }
        }

        public Vector3 GetDialoguePoint()
        {
            return dialoguePoint.position;
        }
        
        public IArchitecture GetArchitecture()
        {
            return Soulstealers.Interface;
        }
    }
}
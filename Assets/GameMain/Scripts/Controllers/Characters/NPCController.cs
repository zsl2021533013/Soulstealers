using EPOOutline;
using GameMain.Scripts.Utility;
using NodeCanvas.DialogueTrees;
using QFramework;
using UnityEngine;

namespace GameMain.Scripts.Entity.EntityLogic
{
    public class NPCController : MonoBehaviour, IController
    {
        [SerializeField]
        private DialogueTreeController controller;

        [SerializeField] 
        private Transform dialoguePoint;

        [SerializeField]
        private Outlinable outline;
        
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
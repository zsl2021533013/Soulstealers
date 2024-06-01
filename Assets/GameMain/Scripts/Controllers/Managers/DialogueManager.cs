using System;
using DG.Tweening;
using GameMain.Scripts.Controller;
using GameMain.Scripts.Model;
using GameMain.Scripts.UI;
using GameMain.Scripts.Utility;
using NodeCanvas.DialogueTrees;
using QFramework;
using UniRx;
using UnityEngine;

namespace GameMain.Scripts.Entity.EntityLogic
{
    public enum DialogueState
    {
        Open,
        Close
        
    }
    
    public class DialogueManager : MonoSingleton<DialogueManager>, ISoulstealersGameController
    {
        public ReactiveProperty<DialogueState> dialogueState;

        private DialoguePanel panel;
        
        public void OnGameInit()
        {
            Subscribe();
            
            panel = UIKit.OpenPanel<DialoguePanel>();

            dialogueState = new ReactiveProperty<DialogueState>(DialogueState.Close);
            dialogueState.Subscribe(value =>
            {
                if (value == DialogueState.Open)
                {
                    if (InputManager.Instance.mouseInteractType.Value == InputManager.MouseInteractType.Ground)
                    {
                        InputManager.Instance.mouseInteractType.Value = InputManager.MouseInteractType.Dialogue;
                    }
                }

                if (value == DialogueState.Close)
                {
                    if (InputManager.Instance.mouseInteractType.Value == InputManager.MouseInteractType.Dialogue)
                    {
                        InputManager.Instance.mouseInteractType.Value = InputManager.MouseInteractType.Ground;
                    }
                }
            });
        }

        public void OnUpdate(float elapse) { }

        public void OnFixedUpdate(float elapse) { }

        public void OnGameShutdown()
        {
            UnSubscribe();
        }

        public void Subscribe()
        {
            DialogueTree.OnDialogueStarted += OnDialogueStarted;
            DialogueTree.OnDialogueFinished += OnDialogueFinished;
        }
        
        public void UnSubscribe()
        {
            DialogueTree.OnDialogueStarted -= OnDialogueStarted;
            DialogueTree.OnDialogueFinished -= OnDialogueFinished;
        }

        public void StartDialogue(NPCController npc)
        {
            var player = this.GetModel<PlayerModel>();
            npc.StartDialogue();
            var lookAtPos = new Vector3(npc.transform.position.x, player.transform.position.y,
                npc.transform.position.z);
            player.transform.DOLookAt(lookAtPos, 1f);
        }

        public void OnDialogueStarted(DialogueTree dlg)
        {
            dialogueState.Value = DialogueState.Open;
            var player = this.GetModel<PlayerModel>();
            player.agent.isStopped = true;
            player.agent.updateRotation = false;
        }
        
        public void OnDialogueFinished(DialogueTree dlg)
        {
            dialogueState.Value = DialogueState.Close;
            var player = this.GetModel<PlayerModel>();
            player.agent.isStopped = false;
            player.agent.updateRotation = true;
        }

        public void SkipDialogue()
        {
            if (dialogueState.Value == DialogueState.Open)
            {
                panel.SkipDialogue();
            }
        }

        public IArchitecture GetArchitecture()
        {
            return Soulstealers.Interface;
        }
    }
}
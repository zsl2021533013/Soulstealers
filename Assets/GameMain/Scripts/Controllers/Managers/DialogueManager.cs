using System;
using GameMain.Scripts.Controller;
using GameMain.Scripts.UI;
using GameMain.Scripts.Utility;
using NodeCanvas.DialogueTrees;
using QFramework;
using UniRx;

namespace GameMain.Scripts.Entity.EntityLogic
{
    public enum DialogueState
    {
        Open,
        Close
    }
    
    public class DialogueManager : MonoSingleton<DialogueManager>, ISoulstealersGameController
    {
        public ReactiveProperty<DialogueState> dialogueState = new ReactiveProperty<DialogueState>();

        private DialoguePanel panel;
        
        public void OnGameInit()
        {
            Subscribe();
            
            panel = UIKit.OpenPanel<DialoguePanel>();

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

        public void OnDialogueStarted(DialogueTree dlg)
        {
            dialogueState.Value = DialogueState.Open;
        }
        
        public void OnDialogueFinished(DialogueTree dlg)
        {
            dialogueState.Value = DialogueState.Close;
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
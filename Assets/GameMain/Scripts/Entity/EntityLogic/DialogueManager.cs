using System.Collections.Generic;
using DG.Tweening;
using GameMain.Scripts.UI;
using GameMain.Scripts.Utility;
using NodeCanvas.DialogueTrees;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain.Scripts.Entity.EntityLogic
{
    public class DialogueManager : Entity
    {
        [System.Serializable]
        public class SubtitleDelays
        {
            public float characterDelay = 0.05f;
            public float sentenceDelay = 0.5f;
            public float commaDelay = 0.1f;
            public float finalDelay = 1.2f;
        }
        
        [Header("Subtitles")] 
        public ScrollRect scrollView;
        public RectTransform subtitlesGroup;
        public GameObject actorSpeechTemplate;
        public Image actorPortrait;
        public CommonButton continueBtn;
        public SubtitleDelays subtitleDelays = new SubtitleDelays();
        private AudioSource playSource;
        private bool questReady = true;

        [Header("Multiple Choice")]
        public RectTransform optionsGroup;
        public Button optionBtn;
        private Dictionary<Button, int> cachedBtns;
        private Vector2 originalSubsPosition;
        private bool isWaitingChoice;

        private AudioSource _localSource;
        private AudioSource localSource {
            get { return _localSource != null ? _localSource : _localSource = gameObject.AddComponent<AudioSource>(); }
        }
        
        private InputManager inputManager;

        public InputManager InputManager
        {
            get
            {
                if (inputManager == null)
                {
                    var Entity = GameEntry.GetComponent<EntityComponent>();
                    inputManager = Entity.GetEntity(AssetUtility.GetEntityAsset("Input Manager")).Logic as InputManager;
                }

                return inputManager;
            }
        }
        
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            
            Hide();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            
            Subscribe();
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            
            UnSubscribe();
        }

        void Subscribe() {
            DialogueTree.OnDialogueStarted += OnDialogueStarted;
            DialogueTree.OnDialoguePaused += OnDialoguePaused;
            DialogueTree.OnDialogueFinished += OnDialogueFinished;
            DialogueTree.OnSubtitlesRequest += OnSubtitlesRequest;
        }

        void UnSubscribe() {
            DialogueTree.OnDialogueStarted -= OnDialogueStarted;
            DialogueTree.OnDialoguePaused -= OnDialoguePaused;
            DialogueTree.OnDialogueFinished -= OnDialogueFinished;
            DialogueTree.OnSubtitlesRequest -= OnSubtitlesRequest;
        }

        void Hide() {
            subtitlesGroup.gameObject.SetActive(false);
            optionsGroup.gameObject.SetActive(false);
            optionBtn.gameObject.SetActive(false);
            originalSubsPosition = subtitlesGroup.transform.position;
        }

        void OnDialogueStarted(DialogueTree dlg) {
            actorSpeechTemplate.SetActive(false);
            
            InputManager.mouseInteractType.Value = InputManager.MouseInteractType.UI;
        }

        void OnDialoguePaused(DialogueTree dlg) {
            subtitlesGroup.gameObject.SetActive(false);
            optionsGroup.gameObject.SetActive(false);
            StopAllCoroutines();
            if ( playSource != null ) playSource.Stop();
        }

        void OnDialogueFinished(DialogueTree dlg) {
            subtitlesGroup.gameObject.SetActive(false);
            optionsGroup.gameObject.SetActive(false);
            if ( cachedBtns != null ) {
                foreach ( var tempBtn in cachedBtns.Keys ) {
                    if ( tempBtn != null ) {
                        Destroy(tempBtn.gameObject);
                    }
                }
                cachedBtns = null;
            }
            StopAllCoroutines();
            if ( playSource != null ) playSource.Stop();
            
            InputManager.mouseInteractType.Value = InputManager.MouseInteractType.Ground;
        }
        
        public void OnSubtitlesRequest(SubtitlesRequestInfo info) {
            var text = info.statement.text;
            var audio = info.statement.audio;
            var actor = info.actor;
            
            text = text.Replace("，", ","); // 字库中缺乏中文逗号
            text = text.Replace("！", "!"); // 字库中缺乏中文感叹号

            questReady = false;

            subtitlesGroup.gameObject.SetActive(true);
            subtitlesGroup.position = originalSubsPosition;

            var actorSpeech = Instantiate(actorSpeechTemplate, actorSpeechTemplate.transform.parent);
            actorSpeech.SetActive(true);
            var speechText = actorSpeech.GetComponent<TMP_Text>();
            speechText.text = new string('口', text.Length);
            
            Canvas.ForceUpdateCanvases();

            scrollView.verticalNormalizedPosition = 0f;
            
            actorPortrait.gameObject.SetActive(actor.portraitSprite != null);
            actorPortrait.sprite = actor.portraitSprite;

            if ( audio != null ) {
                var actorSource = actor.transform != null ? actor.transform.GetComponent<AudioSource>() : null;
                playSource = actorSource != null ? actorSource : localSource;
                playSource.clip = audio;
                playSource.Play();
            }

            continueBtn.HideImmediately();
            speechText
                .DOText(text, subtitleDelays.characterDelay * text.Length)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    questReady = true;
                    continueBtn.Show();
                });
            
            continueBtn.onClick.RemoveAllListeners();
            continueBtn.onClick.AddListener(() =>
            {
                if (questReady)
                {
                    info.Continue();
                }
            });
        }
    }
}
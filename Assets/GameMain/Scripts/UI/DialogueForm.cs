using System.Collections.Generic;
using DG.Tweening;
using GameMain.Scripts.Entity.EntityLogic;
using NodeCanvas.DialogueTrees;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain.Scripts.UI
{
    public class DialogueForm : UGuiForm
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
        public RectTransform subtitlesGroup;
        public Image actorPortrait;
        public ScrollRect scrollView;
        public GameObject actorSpeechTemplate;
        public CommonButton continueBtn;
        public SubtitleDelays subtitleDelays = new SubtitleDelays();
        private AudioSource playSource;
        private bool questReady = true;

        [Header("Multiple Choice")]
        public RectTransform optionsGroup;
        public Button optionBtn;
        private Dictionary<Button, int> cachedBtns;
        
        private bool isWaitingChoice;

        private AudioSource _localSource;
        private AudioSource localSource {
            get { return _localSource != null ? _localSource : _localSource = gameObject.AddComponent<AudioSource>(); }
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            
            Hide();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            
            Subscribe();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            
            UnSubscribe();
        }

        private void Subscribe() 
        {
            DialogueTree.OnDialogueStarted += OnDialogueStarted;
            DialogueTree.OnDialoguePaused += OnDialoguePaused;
            DialogueTree.OnDialogueFinished += OnDialogueFinished;
            DialogueTree.OnSubtitlesRequest += OnSubtitlesRequest;
        }

        private void UnSubscribe() 
        {
            DialogueTree.OnDialogueStarted -= OnDialogueStarted;
            DialogueTree.OnDialoguePaused -= OnDialoguePaused;
            DialogueTree.OnDialogueFinished -= OnDialogueFinished;
            DialogueTree.OnSubtitlesRequest -= OnSubtitlesRequest;
        }

        private void Hide() 
        {
            subtitlesGroup.gameObject.SetActive(false);
            optionsGroup.gameObject.SetActive(false);
        }
        
        private void Show() 
        {
            subtitlesGroup.gameObject.SetActive(true);
            /*optionsGroup.gameObject.SetActive(true);
            optionBtn.gameObject.SetActive(true);*/
        }

        void OnDialogueStarted(DialogueTree dlg) 
        {
            Show();
            actorSpeechTemplate.SetActive(false);
        }

        void OnDialoguePaused(DialogueTree dlg)
        {
            Hide();
            StopAllCoroutines();
            if ( playSource != null ) playSource.Stop();
        }

        void OnDialogueFinished(DialogueTree dlg) {
            Hide();
            
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
        }
        
        public void OnSubtitlesRequest(SubtitlesRequestInfo info) {
            var text = info.statement.text;
            var audio = info.statement.audio;
            var actor = info.actor;
            
            text = text.Replace("，", ","); // 字库中缺乏中文逗号
            text = text.Replace("！", "!"); // 字库中缺乏中文感叹号
            text = actor.name + ":\n" + text;

            questReady = false;

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
using System.Collections.Generic;
using DG.Tweening;
using GameMain.Scripts.Entity.EntityLogic;
using GameMain.Scripts.Utility;
using NodeCanvas.DialogueTrees;
using QFramework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain.Scripts.UI
{
    public class DialoguePanelData : UIPanelData
    {
    }
    
    public class DialoguePanel : UIPanel
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
        public ScrollRect content;
        public GameObject actorSpeechTemplate;
        public CommonButton continueBtn;
        public SubtitleDelays subtitleDelays = new SubtitleDelays();
        private AudioSource playSource;
        private bool questReady = true;

        [Header("Multiple Choice")]
        public RectTransform optionsGroup;
        public GameObject optionBtnTemplate;

        private List<GameObject> cachedSpeechs = new List<GameObject>();
        private Dictionary<CommonButton, int> cachedBtns = new Dictionary<CommonButton, int>();
        private bool isWaitingChoice;

        private AudioSource _localSource;

        private AudioSource localSource =>
            _localSource != null ? _localSource : _localSource = gameObject.AddComponent<AudioSource>();

        protected override void OnInit(IUIData uiData = null)
        {
            base.OnInit(uiData);
            
            HideSubPanel();
        }

        protected override void OnOpen(IUIData uiData = null)
        {
            Subscribe();
        }
        
        protected override void OnClose()
        {
            UnSubscribe();
        }

        private void Subscribe() 
        {
            DialogueTree.OnDialogueStarted += OnDialogueStarted;
            DialogueTree.OnDialoguePaused += OnDialoguePaused;
            DialogueTree.OnDialogueFinished += OnDialogueFinished;
            DialogueTree.OnSubtitlesRequest += OnSubtitlesRequest;
            DialogueTree.OnMultipleChoiceRequest += OnMultipleChoiceRequest;
        }

        private void UnSubscribe() 
        {
            DialogueTree.OnDialogueStarted -= OnDialogueStarted;
            DialogueTree.OnDialoguePaused -= OnDialoguePaused;
            DialogueTree.OnDialogueFinished -= OnDialogueFinished;
            DialogueTree.OnSubtitlesRequest -= OnSubtitlesRequest;
            DialogueTree.OnMultipleChoiceRequest -= OnMultipleChoiceRequest;
        }

        private void HideSubPanel() 
        {
            subtitlesGroup.gameObject.SetActive(false);
        }

        private void ShowSubPanel() 
        {
            subtitlesGroup.gameObject.SetActive(true);
        }

        void OnDialogueStarted(DialogueTree dlg) 
        {
            ShowSubPanel();
            actorSpeechTemplate.SetActive(false);
            optionBtnTemplate.SetActive(false);
        }

        void OnDialoguePaused(DialogueTree dlg)
        {
            HideSubPanel();
            
            if (playSource != null)
            {
                playSource.Stop();
            }
        }

        void OnDialogueFinished(DialogueTree dlg) {
            HideSubPanel();

            if (cachedSpeechs != null)
            {
                foreach ( var speech in cachedSpeechs )
                {
                    if ( speech != null )
                    {
                        Destroy(speech);
                    }
                }
            }
            
            if (cachedBtns != null)
            {
                foreach ( var tempBtn in cachedBtns.Keys )
                {
                    if ( tempBtn != null )
                    {
                        Destroy(tempBtn.gameObject);
                    }
                }
            }

            if (playSource != null)
            {
                playSource.Stop();
            }
        }
        
        public void OnSubtitlesRequest(SubtitlesRequestInfo info) {
            subtitlesGroup.gameObject.SetActive(true);
            optionsGroup.gameObject.SetActive(false);
            actorPortrait.gameObject.SetActive(true);
            Debug.Log(1);
            var text = info.statement.text;
            var audio = info.statement.audio;
            var actor = info.actor;
            
            text = actor.name + ":\n" + text.Convert2E();

            questReady = false;

            var actorSpeech = Instantiate(actorSpeechTemplate, actorSpeechTemplate.transform.parent);
            cachedSpeechs.Add(actorSpeech);
            actorSpeech.SetActive(true);
            
            var speechText = actorSpeech.GetComponent<TMP_Text>();
            speechText.text = new string('口', text.Length);
            
            Canvas.ForceUpdateCanvases();

            content.verticalNormalizedPosition = 0f;
            
            actorPortrait.gameObject.SetActive(actor.portraitSprite != null);
            actorPortrait.sprite = actor.portraitSprite;

            if ( audio != null ) {
                var actorSource = actor.transform != null ? actor.transform.GetComponent<AudioSource>() : null;
                playSource = actorSource != null ? actorSource : localSource;
                playSource.clip = audio;
                playSource.Play();
            }

            continueBtn.gameObject.SetActive(true);
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

        public void OnMultipleChoiceRequest(MultipleChoiceRequestInfo info)
        {
            cachedBtns = new Dictionary<CommonButton, int>();
            
            optionsGroup.gameObject.SetActive(true);
            continueBtn.gameObject.SetActive(false);
            actorPortrait.gameObject.SetActive(false);

            var index = 0;
            foreach (var pair in info.options)
            {
                var text = $"{index + 1}.{pair.Key.text.Convert2E()}";
                
                var btn = Instantiate(optionBtnTemplate, optionBtnTemplate.transform.parent).GetComponent<CommonButton>();
                btn.gameObject.SetActive(true);
                btn.GetComponentInChildren<TMP_Text>()
                    .DOText(text, text.Length * subtitleDelays.characterDelay);
                
                cachedBtns.Add(btn, index);
                btn.onClick.AddListener(() =>
                {
                    optionsGroup.gameObject.SetActive(false);
                    subtitlesGroup.gameObject.SetActive(false);
                    foreach ( var (btn, i) in cachedBtns ) {
                        Destroy(btn.gameObject);
                    }
                    info.SelectOption(cachedBtns[btn]);
                });
                
                index++;
            }
        }
    }
}
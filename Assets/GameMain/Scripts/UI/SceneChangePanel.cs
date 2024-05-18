using System;
using DG.Tweening;
using QFramework;
using UnityEngine;

namespace GameMain.Scripts.UI
{
    public class SceneChangePanel : UIPanel
    {
        private const float FadeTime = 0.6f;
        
        [SerializeField] 
        private CanvasGroup canvasGroup;

        protected override void OnOpen(IUIData uiData = null)
        {
            base.OnOpen(uiData);
        }
        
        protected override void OnClose()
        {
            
        }

        public void FadeIn(Action endAction = null)
        {
            canvasGroup.DOKill();
            canvasGroup.DOFade(0f, FadeTime).OnComplete(() => endAction?.Invoke());
        }

        public void FadeOut(Action endAction = null)
        {
            canvasGroup.DOKill();
            canvasGroup.DOFade(1f, FadeTime).OnComplete(() => endAction?.Invoke());
        }
        
        public void FadeInImmediately()
        {
            canvasGroup.alpha = 0f;
        }

        public void FadeOutImmediately()
        {
            canvasGroup.alpha = 1f;
        }
    }
}
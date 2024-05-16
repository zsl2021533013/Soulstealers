using DG.Tweening;
using UnityEngine;

namespace GameMain.Scripts.UI
{
    public class SceneChangeForm : UGuiForm
    {
        private const float FadeTime = 0.6f;
        
        [SerializeField] 
        private CanvasGroup canvasGroup;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            
            Debug.Log(1);
        }

        public void FadeIn()
        {
            canvasGroup.DOKill();
            canvasGroup.DOFade(0f, FadeTime);
        }

        public void FadeOut()
        {
            canvasGroup.DOKill();
            canvasGroup.DOFade(1f, FadeTime);
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
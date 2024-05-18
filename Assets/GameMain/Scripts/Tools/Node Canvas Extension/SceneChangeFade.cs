using GameMain.Scripts.Entity.EntityLogic;
using GameMain.Scripts.UI;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using QFramework;

namespace GameMain.Scripts.Tools.Node_Canvas_Extension
{
    public enum SceneChangeType
    {
        FadeIn,
        FadeOut
    }
    
    [Category("Task")]
    [Description("Scene change fade")]
    public class SceneChangeFade : ActionTask
    {
        public SceneChangeType sceneChangeType;
        
        protected override string info {
            get { return $"Scene Change {sceneChangeType}"; }
        }

        protected override void OnExecute()
        {
            var panel = UIKit.OpenPanel<SceneChangePanel>();
            if (sceneChangeType == SceneChangeType.FadeIn)
            {
                panel.FadeIn();
            }
            else
            {
                panel.FadeOut();
            }
            
            EndAction();
        }
    }
}
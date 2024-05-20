using DG.Tweening;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace GameMain.Scripts.Tools.Node_Canvas_Extension
{
    [Category("Task")]
    [Description("Rotate object")]
    public class Rotate : ActionTask<Transform>
    {
        public BBParameter<float> duration = 1;
        [SliderField(-180, 180)] public BBParameter<float> angle = 90;

        private float currentAngle;
        private float targetAngle;

        protected override void OnExecute()
        {
            base.OnExecute();

            currentAngle = agent.eulerAngles.y;
            targetAngle = currentAngle + angle.value;

            agent.DOKill();
            agent
                .DORotate(new Vector3(agent.eulerAngles.x, targetAngle, agent.eulerAngles.z), duration.value)
                .SetEase(Ease.Linear);
            
            EndAction();
        }
    }
}
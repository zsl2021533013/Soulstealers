using GameMain.Scripts.Entity.EntityLogic;
using GameMain.Scripts.Utility;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UniRx;
using UnityEngine;

namespace GameMain.Scripts.Tools.Node_Canvas_Extension
{
    [Category("Task")]
    [Description("Move NPC")]
    public class MoveNPC : ActionTask
    {
        public NPCController controller;
        public Transform targetPos;
        
        protected override string info
        {
            get { return $"Move NPC"; }
        }

        protected override void OnExecute()
        {
            controller.SetDestination(targetPos.position);
            controller.pathStatus
                .First()
                .Subscribe(value =>
                {
                    if (value == NavMeshStatus.Complete)
                    {
                        EndAction();
                    }
                });
        }
    }
}
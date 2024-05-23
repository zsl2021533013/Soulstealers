using DG.Tweening;
using GameMain.Scripts.Controller;
using GameMain.Scripts.Model;
using GameMain.Scripts.Scriptable_Object;
using GameMain.Scripts.UI;
using GameMain.Scripts.Utility;
using QFramework;
using TMPro;
using UniRx;
using UnityEngine;

namespace GameMain.Scripts.Entity.EntityLogic
{
    public class TaskManager : MonoSingleton<TaskManager>, ISoulstealersGameController
    {
        public ReactiveProperty<bool> isNewTask = new ReactiveProperty<bool>();
        
        public void OnGameInit()
        {
            var model = this.GetModel<TaskModel>();
            UIKit.OpenPanel<TaskPanel>(new TaskPanelData() { tasks = model.tasks });

            isNewTask.Subscribe(value =>
            {
                if (value)
                {
                    var panel = UIKit.GetPanel<TaskPanel>();
                    if (panel is not null)
                    {
                        var text = panel.openBtn.GetComponentInChildren<TMP_Text>();
                        text.DOKill();
                        text.DOColor(Color.yellow, 0.4f).SetLoops(-1, LoopType.Yoyo);
                    }
                }
                else
                {
                    var panel = UIKit.GetPanel<TaskPanel>();
                    if (panel is not null)
                    {
                        var text = panel.openBtn.GetComponentInChildren<TMP_Text>();
                        text.DOKill();
                        text.DOColor(Color.white, 1f);
                    }
                }
            });
        }

        public void OnUpdate(float elapse)
        {
        }

        public void OnFixedUpdate(float elapse)
        {
        }

        public void OnGameShutdown()
        {
        }

        public void ActivateTask(int id)
        {
            var model = this.GetModel<TaskModel>();
            model.ActivateTask(id);
        }
        
        public void CompleteTask(int id)
        {
            var model = this.GetModel<TaskModel>();
            model.CompleteTask(id);
        }
        
        public IArchitecture GetArchitecture()
        {
            return Soulstealers.Interface;
        }
    }
}
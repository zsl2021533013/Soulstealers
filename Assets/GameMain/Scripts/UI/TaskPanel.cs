using System.Collections.Generic;
using DG.Tweening;
using GameMain.Scripts.Entity.EntityLogic;
using GameMain.Scripts.Scriptable_Object;
using GameMain.Scripts.Utility;
using QFramework;
using TMPro;
using UnityEngine;

namespace GameMain.Scripts.UI
{
    public class TaskPanelData : UIPanelData
    {
        public List<Task> tasks;
    }
    
    public class TaskPanel : UIPanel
    {
        public CommonButton openBtn;
        public CommonButton closeBtn;
        public GameObject taskContent;
        public GameObject activeTaskTemple;
        public GameObject completeTaskTemple;

        private List<Task> tasks;
        
        private List<GameObject> activeTaskCache = new List<GameObject>();
        private List<GameObject> completeTaskCache = new List<GameObject>();

        protected override void OnInit(IUIData uiData = null)
        {
            base.OnInit(uiData);
            
            taskContent.SetActive(false);
            
            openBtn.onClick.AddListener(() =>
            {
                openBtn.gameObject.SetActive(false);
                taskContent.SetActive(true);

                if (InputManager.Instance.mouseInteractType.Value == InputManager.MouseInteractType.Ground)
                {
                    InputManager.Instance.mouseInteractType.Value = InputManager.MouseInteractType.UI;
                }
                
                TaskManager.Instance.isNewTask.Value = false;
                
                UpdateTaskPanel();
            });
            
            closeBtn.onClick.AddListener(() =>
            {
                openBtn.gameObject.SetActive(true);
                taskContent.SetActive(false);
                
                if (InputManager.Instance.mouseInteractType.Value == InputManager.MouseInteractType.UI)
                {
                    InputManager.Instance.mouseInteractType.Value = InputManager.MouseInteractType.Ground;
                }

                activeTaskCache.ForEach(Destroy);
                completeTaskCache.ForEach(Destroy);
            });
        }

        protected override void OnOpen(IUIData uiData = null)
        {
            base.OnOpen(uiData);

            tasks = (uiData as TaskPanelData)?.tasks;
            
            activeTaskTemple.SetActive(false);
            completeTaskTemple.SetActive(false);
        }

        protected override void OnClose()
        {
        }

        private void UpdateTaskPanel()
        {
            activeTaskCache.ForEach(Destroy);
            completeTaskCache.ForEach(Destroy);

            int activeIndex = 1;
            int completeIndex = 1;
            foreach (var task in tasks)
            {
                if (task.state == Task.TaskState.Active)
                {
                    var t = Instantiate(activeTaskTemple, activeTaskTemple.transform.parent);
                    t.SetActive(true);
                    activeTaskCache.Add(t);
                    t.GetComponent<TMP_Text>().text = $"{activeIndex}. " + task.activeText;
                    activeIndex++;
                }
                if (task.state == Task.TaskState.Complete)
                {
                    var t = Instantiate(completeTaskTemple, completeTaskTemple.transform.parent);
                    t.SetActive(true);
                    completeTaskCache.Add(t);
                    t.GetComponent<TMP_Text>().text = $"{completeIndex}. " + task.completeText;
                    completeIndex++;
                }
            }
        }
    }
}
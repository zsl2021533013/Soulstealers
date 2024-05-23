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
        public TaskData taskData;
    }
    
    public class TaskPanel : UIPanel
    {
        public CommonButton openBtn;
        public CommonButton closeBtn;
        public GameObject taskContent;
        public GameObject activeTaskTemple;
        public GameObject completeTaskTemple;

        private TaskData taskData;
        
        private List<GameObject> activeTaskCache = new List<GameObject>();
        private List<GameObject> completeTaskCache = new List<GameObject>();

        protected override void OnInit(IUIData uiData = null)
        {
            base.OnInit(uiData);
            
            taskContent.SetActive(false);
            
            openBtn.onClick.AddListener(() =>
            {
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

            taskData = (uiData as TaskPanelData)?.taskData;
            
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
            
            foreach (var task in taskData.tasks)
            {
                Debug.Log(task.state);
                if (task.state == Task.TaskState.Active)
                {
                    var t = Instantiate(activeTaskTemple, activeTaskTemple.transform.parent);
                    t.SetActive(true);
                    activeTaskCache.Add(t);
                    t.GetComponent<TMP_Text>().text = task.activeText;
                }
                if (task.state == Task.TaskState.Complete)
                {
                    var t = Instantiate(completeTaskTemple, completeTaskTemple.transform.parent);
                    t.SetActive(true);
                    completeTaskCache.Add(t);
                    t.GetComponent<TMP_Text>().text = task.completeText;
                }
            }
        }
    }
}
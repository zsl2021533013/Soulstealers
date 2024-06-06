﻿using System.Collections.Generic;
using EPOOutline;
using GameMain.Scripts.Controller;
using GameMain.Scripts.Utility;
using Newtonsoft.Json;
using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using QFramework;
using UnityEngine;
using UnityEngine.Rendering;

namespace GameMain.Scripts.Entity.EntityLogic
{
    public class NPCController : MonoBehaviour, ISoulstealersGameController
    {
        [SerializeField]
        private DialogueTreeController controller;

        [SerializeField] 
        private Blackboard blackboard;

        [SerializeField] 
        private Transform dialoguePoint;

        [SerializeField]
        private Outlinable outline;
        
        public void OnGameInit()
        {
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

        public Dictionary<string, object> GetData()
        {
            var blackboardSerialize = blackboard.Serialize(null, true);
            var position = transform.position;
            var rotation = transform.rotation;
            var tag = transform.tag;
            
            var data = new Dictionary<string, object>
            {
                { "blackboard", blackboardSerialize },
                { "position", position},
                { "rotation", rotation},
                { "tag", tag }
            };

            return data;
        }

        public void LoadData(Dictionary<string, object> data)
        {
            var a = data["blackboard"];
            Debug.Log(a.GetType());
            
            var blackboardSerialize = (string)data["blackboard"];
            blackboard.Deserialize(blackboardSerialize, null, false);

            var position = (Vector3)data["position"];
            transform.position = position;

            var rotation = (Quaternion)data["rotation"];
            transform.rotation = rotation;

            transform.tag = (string)data["tag"];
        }
        
        public void StartDialogue()
        {
            if (!controller.isRunning)
            {
                controller.StartDialogue();
            }
        }

        public void EnableOutline()
        {
            if (outline)
            {
                outline.enabled = true;
            }
        }

        public void DisableOutline()
        {
            if (outline)
            {
                outline.enabled = false;
            }
        }

        public Vector3 GetDialoguePoint()
        {
            return dialoguePoint.position;
        }
        
        public IArchitecture GetArchitecture()
        {
            return Soulstealers.Interface;
        }
    }
}
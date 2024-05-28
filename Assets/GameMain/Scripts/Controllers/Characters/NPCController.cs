using System.Collections.Generic;
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
    public class NPCController : MonoBehaviour, IController, ISoulstealersGameController
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

        public string Serialize()
        {
            var blackboardSerialize = blackboard.Serialize(null);
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

            return JsonConvert.SerializeObject(data,
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        public void Deserialize(string t)
        {
            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(t);

            var blackboardSerialize = data["blackboard"].ToString();
            blackboard.Deserialize(blackboardSerialize, null);

            var position = JsonConvert.DeserializeObject<Vector3>(data["position"].ToString());
            transform.position = position;

            var rotation = JsonConvert.DeserializeObject<Quaternion>(data["rotation"].ToString());
            transform.rotation = rotation;

            transform.tag = data["tag"].ToString();
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
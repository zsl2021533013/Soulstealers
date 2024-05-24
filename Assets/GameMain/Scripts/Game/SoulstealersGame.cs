using System.Collections.Generic;
using GameMain.Scripts.Controller;
using GameMain.Scripts.Entity.EntityLogic;
using GameMain.Scripts.Event;
using GameMain.Scripts.Model;
using GameMain.Scripts.Scriptable_Object;
using GameMain.Scripts.UI;
using GameMain.Scripts.Utility;
using NodeCanvas.Framework;
using QFramework;
using UnityEditor;
using UnityEngine;

namespace GameMain.Scripts.Game
{
    public class SoulstealersGame : GameBase, IController
    {
        private List<ISoulstealersGameController> managers = new List<ISoulstealersGameController>();
        private List<ISoulstealersGameController> characters = new List<ISoulstealersGameController>();

        private bool isNewGame;
        
        public override void Initialize()
        {
            base.Initialize();

            isNewGame = false;

            MakeSureData();
            
            if (isNewGame)
            {
                var panel = UIKit.GetPanel<SceneChangePanel>();
                panel.FadeOut();
            }

            _ = Soulstealers.Interface;

            LoadManager();
            LoadPlayer();
            LoadNPC();
            LoadTask();

            managers.ForEach(manager => manager.OnGameInit());
            characters.ForEach(character => character.OnGameInit());
            
            if (isNewGame)
            {
                var opening = GameObject.Find("Opening");
                var controller = opening.GetComponent<NPCController>();
                controller.StartDialogue();
            }
        }

        public override void Update(float elapse)
        {
            base.Update(elapse);
            
            managers.ForEach(manager => manager.OnUpdate(elapse));
            characters.ForEach(character => character.OnUpdate(elapse));
        }

        public override void FixedUpdate(float elapse)
        {
            base.FixedUpdate(elapse);
            
            managers.ForEach(manager => manager.OnFixedUpdate(elapse));
            characters.ForEach(character => character.OnFixedUpdate(elapse));
        }

        public override void Shutdown()
        {
            base.Shutdown();
            
            managers.ForEach(manager => manager.OnGameShutdown());
            characters.ForEach(character => character.OnGameShutdown());
        }

        private void MakeSureData()
        {
            var data = Resources.Load<GameData>(AssetUtility.GetSaveAsset("GameData"));
            if (data == null)
            {
                isNewGame = true;
                data = ScriptableObject.CreateInstance<GameData>();
                
                var playerStart = Object.FindObjectOfType<PlayerStart>().transform;
                data.playerData.position = playerStart.position;
                data.playerData.rotation = playerStart.rotation;

                var NPCs = Object.FindObjectsOfType<NPCController>();
                NPCs.ForEach(npc =>
                {
                    data.dialogueData.Add(npc.name, npc.Serialize());
                });
                
                data.tasks = Resources.Load<TaskData>(AssetUtility.GetSOAsset("TaskDataTemplate")).tasks;
                
                if (!AssetDatabase.Contains(data))
                {
                    AssetDatabase.CreateAsset(data, "Assets/GameMain/Resources/" + AssetUtility.GetSaveAsset("GameData") + ".asset");
                    AssetDatabase.SaveAssets();
                }
            }

            this.RegisterEvent<ModelChangeEvent>(e =>
            {
                Save();
            });
        }
        
        private void LoadManager()
        {
            var model = this.GetModel<ManagerModel>();
            managers.AddRange(model.managers);
        }

        private void LoadPlayer()
        {
            var model = this.GetModel<PlayerModel>();
            characters.Add(model.controller);
        }

        private void LoadNPC()
        {
            var model = this.GetModel<NPCModel>();
            characters.AddRange(model.NPCs);
        }

        private void LoadTask()
        {
            
        }

        private void Save()
        {
            var data = Resources.Load<GameData>(AssetUtility.GetSaveAsset("GameData"));
            var playerData = data.playerData;
            var dialogueData = data.dialogueData;
            var taskData = data.tasks;
            
            var player = this.GetModel<PlayerModel>().transform;
            playerData.position = player.position;
            playerData.rotation = player.rotation;

            dialogueData.Clear();
            var NPCs = this.GetModel<NPCModel>().NPCs;
            NPCs.ForEach(npc =>
            {
                var blackboard = npc.GetComponent<Blackboard>();
                dialogueData.Add(npc.name, blackboard.Serialize(null));
            });

            taskData.Clear();
            var tasks = this.GetModel<TaskModel>().tasks;
            taskData.AddRange(tasks);
        }

        public IArchitecture GetArchitecture()
        {
            return Soulstealers.Interface;
        }
    }
}
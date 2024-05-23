using System.Collections.Generic;
using System.Linq;
using GameMain.Scripts.Controller;
using GameMain.Scripts.Entity.EntityLogic;
using GameMain.Scripts.Model;
using GameMain.Scripts.Scriptable_Object;
using GameMain.Scripts.UI;
using GameMain.Scripts.Utility;
using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using QFramework;
using UnityEngine;

namespace GameMain.Scripts.Game
{
    public class SoulstealersGame : GameBase, IController
    {
        private List<ISoulstealersGameController> managers = new List<ISoulstealersGameController>();
        private List<ISoulstealersGameController> characters = new List<ISoulstealersGameController>();
        
        public override void Initialize()
        {
            base.Initialize();

            var panel = UIKit.OpenPanel<SceneChangePanel>();
            panel.FadeOut();

            LoadManager();
            LoadPlayer();
            LoadNPC();

            managers.ForEach(manager => manager.OnGameInit());
            characters.ForEach(character => character.OnGameInit());
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
        
        private void LoadManager()
        {
            var managerHolder = new GameObject("Manager Holder");
            managerHolder.DontDestroyOnLoad();
            
            var managerAssets = Resources.LoadAll<GameObject>(AssetUtility.GetManagerAsset(""));
            foreach (var manager in managerAssets)
            {
                var m = Object.Instantiate(manager, managerHolder.transform);
                managers.Add(m.GetComponent<ISoulstealersGameController>());
            }
        }

        private void LoadPlayer()
        {
            var data = Resources.Load<GameData>(AssetUtility.GetSOAsset("Game Data"))?.playerData;
            var startPoint = Object.FindObjectOfType<PlayerStart>().transform;
            
            var player = Resources.Load<GameObject>(AssetUtility.GetCharacterAsset("Player"));
            GameObject c;
            if (data == null)
            {
                c = player.Instantiate(startPoint.position, Quaternion.identity);
            }
            else
            {
                c = player.Instantiate(data.position, data.rotation);
            }
            
            characters.Add(c.GetComponent<ISoulstealersGameController>());
            
            this.GetModel<PlayerModel>().transform = c.transform;
            this.GetModel<PlayerModel>().cameraPoint = c.transform.Find("Camera Point");
            this.GetModel<PlayerModel>().controller = c.GetComponent<PlayerController>();
        }

        private void LoadNPC()
        {
            var NPCs = Object.FindObjectsOfType<NPCController>();
            NPCs.ForEach(npc => characters.Add(npc.GetComponent<ISoulstealersGameController>()));
            
            var data = Resources.Load<GameData>(AssetUtility.GetSOAsset("Game Data"))?.dialogueData;
            
            if (data == null)
            {
                var opening = GameObject.Find("Opening");
                opening.GetComponent<NPCController>().StartDialogue();
            }
            else
            {
                var blackboards = NPCs.Select(npc => npc.GetComponent<Blackboard>()).ToList();
                blackboards.ForEach(blackboard =>
                {
                    blackboard.Deserialize(data[blackboard], null);
                });
            }
        }

        private void Save()
        {
            var data = ScriptableObject.CreateInstance<GameData>();
            var playerData = data.playerData;
            var dialogueData = data.dialogueData;
            
            var player = this.GetModel<PlayerModel>().transform;
            playerData.position = player.position;
            playerData.rotation = player.rotation;

            var NPCs = characters.Where(character);
        }

        public IArchitecture GetArchitecture()
        {
            return Soulstealers.Interface;
        }
    }
}
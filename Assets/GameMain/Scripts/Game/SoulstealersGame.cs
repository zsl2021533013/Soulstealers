using System.Collections.Generic;
using GameMain.Scripts.Controller;
using GameMain.Scripts.Entity.EntityLogic;
using GameMain.Scripts.Model;
using GameMain.Scripts.UI;
using GameMain.Scripts.Utility;
using NodeCanvas.DialogueTrees;
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
            
            LoadCharacter();

            managers.ForEach(manager => manager.OnGameInit());
            characters.ForEach(character => character.OnGameInit());
            
            var opening = Resources.Load<GameObject>(AssetUtility.GetCharacterAsset("Opening")).Instantiate();
            opening.GetComponent<NPCController>().StartDialogue();
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

        private void LoadCharacter()
        {
            var startPoint = Object.FindObjectOfType<PlayerStart>().transform;
            
            var player = Resources.Load<GameObject>(AssetUtility.GetCharacterAsset("Player"));
            var c = player.Instantiate(startPoint.position, Quaternion.identity);
            
            characters.Add(c.GetComponent<ISoulstealersGameController>());
            
            this.GetModel<PlayerModel>().transform = c.transform;
            this.GetModel<PlayerModel>().cameraPoint = c.transform.Find("Camera Point");
            this.GetModel<PlayerModel>().controller = c.GetComponent<PlayerController>();
        }

        public IArchitecture GetArchitecture()
        {
            return Soulstealers.Interface;
        }
    }
}
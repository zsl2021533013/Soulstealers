using System.Collections.Generic;
using GameMain.Scripts.Controller;
using GameMain.Scripts.Utility;
using QFramework;
using UnityEngine;

namespace GameMain.Scripts.Model
{
    public class ManagerModel : AbstractModel
    {
        public List<ISoulstealersGameController> managers = new List<ISoulstealersGameController>();
        
        protected override void OnInit()
        {
            managers = new List<ISoulstealersGameController>();
            
            var managerHolder = new GameObject("Manager Holder");
            managerHolder.DontDestroyOnLoad();
            
            var managerAssets = Resources.LoadAll<GameObject>(AssetUtility.GetManagerAsset(""));
            foreach (var manager in managerAssets)
            {
                var m = Object.Instantiate(manager, managerHolder.transform);
                managers.Add(m.GetComponent<ISoulstealersGameController>());
            }
        }
    }
}
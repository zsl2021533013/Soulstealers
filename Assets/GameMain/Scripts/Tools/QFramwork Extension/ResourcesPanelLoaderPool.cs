using System;
using GameMain.Scripts.Utility;
using QFramework;
using UnityEngine;

namespace GameMain.Scripts.Tools
{
    public class ResourcesPanelLoaderPool : AbstractPanelLoaderPool
    {
        /// <summary>
        /// Load Panel from Resources
        /// </summary>
        public class ResourcesPanelLoader : IPanelLoader
        {
            private GameObject mPanelPrefab;

            public GameObject LoadPanelPrefab(PanelSearchKeys panelSearchKeys)
            {
                mPanelPrefab = Resources.Load<GameObject>(AssetUtility.GetUIAsset(panelSearchKeys.PanelType.Name));
                return mPanelPrefab;
            }

            public void LoadPanelPrefabAsync(PanelSearchKeys panelSearchKeys, Action<GameObject> onPanelLoad)
            {
                var request = Resources.LoadAsync<GameObject>(panelSearchKeys.GameObjName);

                request.completed += operation => { onPanelLoad(request.asset as GameObject); };
            }

            public void Unload()
            {
                mPanelPrefab = null;
            }
        }

        protected override IPanelLoader CreatePanelLoader()
        {
            return new ResourcesPanelLoader();
        }
    }
}
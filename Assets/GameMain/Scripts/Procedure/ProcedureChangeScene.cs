using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using GameMain.Scripts.DataTable;
using GameMain.Scripts.Definition.Constant;
using UnityGameFramework.Runtime;
using AssetUtility = GameMain.Scripts.Utility.AssetUtility;

namespace GameMain.Scripts.Procedure
{
    public class ProcedureChangeScene : ProcedureBase
    {
        private const int MenuSceneId = 1;

        private bool m_ChangeToMenu = false;
        private bool m_IsChangeSceneComplete = false;

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            m_IsChangeSceneComplete = false;

            var Event = GameEntry.GetComponent<EventComponent>();

            Event.Subscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            Event.Subscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
            Event.Subscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
            Event.Subscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);
            
            var Sound = GameEntry.GetComponent<SoundComponent>();

            // 停止所有声音
            Sound.StopAllLoadingSounds();
            Sound.StopAllLoadedSounds();

            var Entity = GameEntry.GetComponent<EntityComponent>();
            
            // 隐藏所有实体
            Entity.HideAllLoadingEntities();
            Entity.HideAllLoadedEntities();
            
            var Scene = GameEntry.GetComponent<SceneComponent>();

            // 卸载所有场景
            string[] loadedSceneAssetNames = Scene.GetLoadedSceneAssetNames();
            for (int i = 0; i < loadedSceneAssetNames.Length; i++)
            {
                Scene.UnloadScene(loadedSceneAssetNames[i]);
            }

            var Base = GameEntry.GetComponent<BaseComponent>();

            // 还原游戏速度
            Base.ResetNormalGameSpeed();

            var DataTable = GameEntry.GetComponent<DataTableComponent>();

            int sceneId = procedureOwner.GetData<VarInt32>("NextSceneId");
            m_ChangeToMenu = sceneId == MenuSceneId;
            IDataTable<DRScene> dtScene = DataTable.GetDataTable<DRScene>();
            DRScene drScene = dtScene.GetDataRow(sceneId);
            if (drScene == null)
            {
                Log.Warning("Can not load scene '{0}' from data table.", sceneId.ToString());
                return;
            }

            Scene.LoadScene(AssetUtility.GetSceneAsset(drScene.AssetName), Constant.AssetPriority.SceneAsset, this);
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            var Event = GameEntry.GetComponent<EventComponent>();
            
            Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            Event.Unsubscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
            Event.Unsubscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
            Event.Unsubscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!m_IsChangeSceneComplete)
            {
                return;
            }

            if (m_ChangeToMenu)
            {
                ChangeState<ProcedureMenu>(procedureOwner);
            }
            else
            {
                ChangeState<ProcedureMain>(procedureOwner);
            }
        }

        private void OnLoadSceneSuccess(object sender, GameEventArgs e)
        {
            LoadSceneSuccessEventArgs ne = (LoadSceneSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' OK.", ne.SceneAssetName);

            m_IsChangeSceneComplete = true;
        }

        private void OnLoadSceneFailure(object sender, GameEventArgs e)
        {
            LoadSceneFailureEventArgs ne = (LoadSceneFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Load scene '{0}' failure, error message '{1}'.", ne.SceneAssetName, ne.ErrorMessage);
        }

        private void OnLoadSceneUpdate(object sender, GameEventArgs e)
        {
            LoadSceneUpdateEventArgs ne = (LoadSceneUpdateEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' update, progress '{1}'.", ne.SceneAssetName, ne.Progress.ToString("P2"));
        }

        private void OnLoadSceneDependencyAsset(object sender, GameEventArgs e)
        {
            LoadSceneDependencyAssetEventArgs ne = (LoadSceneDependencyAssetEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' dependency asset '{1}', count '{2}/{3}'.", ne.SceneAssetName, ne.DependencyAssetName, ne.LoadedCount.ToString(), ne.TotalCount.ToString());
        }
    }
}
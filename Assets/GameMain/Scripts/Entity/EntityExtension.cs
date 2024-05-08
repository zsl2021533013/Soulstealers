using System;
using GameFramework.DataTable;
using GameMain.Scripts.DataTable;
using GameMain.Scripts.Utility;
using UnityGameFramework.Runtime;

namespace GameMain.Scripts.Entity
{
    public static class EntityExtension
    {
        private static int s_SerialId = 0;
        
        public static void HideEntity(this EntityComponent entityComponent, EntityLogic.Entity entity)
        {
            entityComponent.HideEntity(entity.Entity);
        }
        
        public static void ShowEntity(this EntityComponent entityComponent, Type logicType, string entityGroup, int priority, EntityData.EntityData data)
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }

            var DataTable = GameEntry.GetComponent<DataTableComponent>();
            IDataTable<DREntity> dtEntity = DataTable.GetDataTable<DREntity>();
            DREntity drEntity = dtEntity.GetDataRow(data.TypeId);
            if (drEntity == null)
            {
                Log.Warning("Can not load entity id '{0}' from data table.", data.TypeId.ToString());
                return;
            }

            entityComponent.ShowEntity(data.Id, logicType, AssetUtility.GetEntityAsset(drEntity.AssetName), entityGroup, priority, data);
        }
        
        public static int GenerateSerialId(this EntityComponent entityComponent)
        {
            return ++s_SerialId;
        }
    }
}
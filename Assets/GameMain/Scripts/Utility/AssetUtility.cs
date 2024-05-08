namespace GameMain.Scripts.Utility
{
    public static class AssetUtility
    {
        public static string GetConfigAsset(string assetName, bool fromBytes)
        {
            return GameFramework.Utility.Text.Format("Assets/GameMain/Configs/{0}.{1}", assetName, fromBytes ? "bytes" : "txt");
        }
        
        public static string GetDataTableAsset(string assetName, bool fromBytes)
        {
            return GameFramework.Utility.Text.Format("Assets/GameMain/DataTables/{0}.{1}", assetName, fromBytes ? "bytes" : "txt");
        }
        
        public static string GetSceneAsset(string assetName)
        {
            return GameFramework.Utility.Text.Format("Assets/GameMain/Scenes/{0}.unity", assetName);
        }
        
        public static string GetUIFormAsset(string assetName)
        {
            return GameFramework.Utility.Text.Format("Assets/GameMain/UI/UIForms/{0}.prefab", assetName);
        }
        
        public static string GetEntityAsset(string assetName)
        {
            return GameFramework.Utility.Text.Format("Assets/GameMain/Entities/{0}.prefab", assetName);
        }
    }
}
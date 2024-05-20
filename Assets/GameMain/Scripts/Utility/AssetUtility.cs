namespace GameMain.Scripts.Utility
{
    public static class AssetUtility
    {
        public static string GetSceneAsset(string assetName)
        {
            return $"Assets/GameMain/Scenes/{assetName}.unity";
        }

        public static string GetUIAsset(string assetName)
        {
            return $"UI/{assetName}";
        }
        
        public static string GetManagerAsset(string assetName)
        {
            return $"Prefabs/Managers/{assetName}";
        }
        
        public static string GetCharacterAsset(string assetName)
        {
            return $"Prefabs/Characters/{assetName}";
        }
        
        public static string GetSOAsset(string assetName)
        {
            return $"ScriptableObjects/{assetName}";
        }
        
        public static string GetCursorAsset(string assetName)
        {
            return $"Sprites/Cursor/{assetName}";
        }
    }
}
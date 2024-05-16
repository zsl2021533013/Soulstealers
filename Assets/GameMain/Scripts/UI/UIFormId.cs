namespace GameMain.Scripts.UI
{
    /// <summary>
    /// 界面编号。
    /// </summary>
    public enum UIFormId : byte
    {
        Undefined = 0,

        /// <summary>
        /// 主菜单
        /// </summary>
        MenuForm = 100,

        /// <summary>
        /// 转场淡入淡出
        /// </summary>
        SceneChangeForm = 101,
        
        /// <summary>
        /// 对话UI
        /// </summary>
        DialogueForm = 200
    }
}
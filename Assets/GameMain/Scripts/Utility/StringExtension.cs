namespace GameMain.Scripts.Utility
{
    public static class StringExtension
    {
        public static string Convert2E(this string text)
        {
            var t = text;
            
            t = t.Replace("：", ":"); // 字库中缺乏中文冒号
            t = t.Replace("；", ";"); // 字库中缺乏中文分号
            t = t.Replace("，", ","); // 字库中缺乏中文逗号
            t = t.Replace("！", "!"); // 字库中缺乏中文感叹号
            t = t.Replace("？", "?"); // 字库中缺乏中文问号
            
            return t;
        }
    }
}
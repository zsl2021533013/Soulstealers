
using UnityEngine;

namespace GameMain.Scripts.Utility
{
    public static class StringExtension
    {
        /// <summary> hex转换到color </summary>
        /// <param name="hex">十六进制字符串</param>
        /// <returns>Color对象</returns>
        public static Color HexToColor(this string hex)
        {
            byte br = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte bg = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte bb = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            byte cc = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            float r = br / 255f;
            float g = bg / 255f;
            float b = bb / 255f;
            float a = cc / 255f;
            return new Color(r, g, b, a);
        }
 
    }
}
using UnityEngine;

namespace GameMain.Scripts.Utility
{
    public class PlayerStart : MonoBehaviour
    {
        private string iconPath = "GameMain/Gizmos/PlayerStart.png"; // 图片相对于项目文件夹的路径

        // 当物体被选中时，绘制Gizmos图标
        private void OnDrawGizmos()
        {
            // 获取图片的完整路径
            string fullPath = "Assets/" + iconPath; // 假设图片格式为png

            // 加载图片
            Texture2D gizmosIcon = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>(fullPath);

            // 检查图片是否存在
            if (gizmosIcon != null)
            {
                // 设置Gizmos的颜色为白色
                Gizmos.color = Color.white;

                // 绘制指定纹理作为Gizmos图标，位于物体的位置上
                Gizmos.DrawIcon(transform.position, fullPath, true);
            }
            else
            {
                Debug.LogWarning("Failed to load Gizmos icon at path: " + fullPath);
            }
        }
    }
}
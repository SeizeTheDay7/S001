using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class ApplyTpsheetTool : EditorWindow
{
    [MenuItem("Tools/Apply TPSheet to Selected PNG")]
    public static void ApplyTPSheetToSelected()
    {
        Object selected = Selection.activeObject;
        if (selected == null)
        {
            UnityEngine.Debug.LogWarning("No PNG file selected.");
            return;
        }

        string pngPath = AssetDatabase.GetAssetPath(selected);
        if (!pngPath.EndsWith(".png"))
        {
            UnityEngine.Debug.LogWarning("Selected file is not a PNG.");
            return;
        }

        string fullPngPath = Path.GetFullPath(pngPath);
        string tpsheetPath = Path.ChangeExtension(fullPngPath, ".tpsheet");

        if (!File.Exists(tpsheetPath))
        {
            UnityEngine.Debug.LogError($"TPSheet not found for {Path.GetFileName(pngPath)}");
            return;
        }

        // 파싱 시작
        string[] lines = File.ReadAllLines(tpsheetPath);
        int imageHeight = 0;
        List<SpriteMetaData> spriteMetaDataList = new List<SpriteMetaData>();

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                continue;

            if (line.StartsWith(":size="))
            {
                var sizeParts = line.Replace(":size=", "").Split('x');
                if (sizeParts.Length == 2)
                    int.TryParse(sizeParts[1], out imageHeight);
            }
            else if (line.Contains(";"))
            {
                var parts = line.Split(';');
                if (parts.Length < 7)
                    continue;

                string name = Path.GetFileNameWithoutExtension(parts[0]);
                int.TryParse(parts[1], out int x);
                int.TryParse(parts[2], out int y);
                int.TryParse(parts[3], out int w);
                int.TryParse(parts[4], out int h);
                float.TryParse(parts[5], out float pivotX);
                float.TryParse(parts[6], out float pivotY);

                SpriteMetaData smd = new SpriteMetaData
                {
                    name = name,
                    rect = new Rect(x, y, w, h),
                    pivot = new Vector2(pivotX, pivotY),
                    alignment = (int)SpriteAlignment.Custom
                };

                spriteMetaDataList.Add(smd);
            }
        }

        if (spriteMetaDataList.Count == 0 || imageHeight == 0)
        {
            UnityEngine.Debug.LogWarning("TPSheet parsed but contains no valid sprite data.");
            return;
        }

        // 텍스처 임포터에 메타데이터 적용
        TextureImporter importer = AssetImporter.GetAtPath(pngPath) as TextureImporter;
        if (importer == null)
        {
            UnityEngine.Debug.LogError("TextureImporter not found.");
            return;
        }

#pragma warning disable 0618
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.spritesheet = spriteMetaDataList.ToArray();
#pragma warning restore 0618

        EditorUtility.SetDirty(importer);
        AssetDatabase.ImportAsset(pngPath, ImportAssetOptions.ForceUpdate);

        UnityEngine.Debug.Log($"✅ Applied {spriteMetaDataList.Count} sprites to {Path.GetFileName(pngPath)}");
    }
}

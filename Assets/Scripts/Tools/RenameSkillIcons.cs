using UnityEditor;
using UnityEngine;
using System.IO;

public class RenameSkillIcons
{
    [MenuItem("Tools/Rename Skill Icons")]
    public static void RenameIcons()
    {
        string folderPath = "Assets/Resources/Icons/Skills"; 

        string[] files = Directory.GetFiles(folderPath, "*.png");

        foreach (var path in files)
        {
            string filename = Path.GetFileName(path);

            if (!filename.StartsWith("Skill icon - "))
            {
                string newName = "Skill icon - " + filename;
                string newPath = Path.Combine(folderPath, newName);

                AssetDatabase.RenameAsset(path, newName);
                Debug.Log($"Renamed {filename} -> {newName}");
            }
        }

        AssetDatabase.Refresh();
    }
}
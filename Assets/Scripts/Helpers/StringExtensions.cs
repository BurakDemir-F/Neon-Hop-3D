using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Game.Shared.AddressablesLocal.Scripts.Utilities
{
    public static class StringExtensions
    {
        public static string LocateAtPersistentDataPath(this string fileName)
        {
            var pDataPath = Application.persistentDataPath;
            return Path.Combine(pDataPath, fileName);
        }
        
        public static string GetResourcesPath()
        {
            return $"{Application.dataPath}/Resources";
        }
        
        public static string LocateAtDataPath(this string fileName)
        {
            var dataPath = Application.dataPath;
            return Path.Combine(dataPath, fileName);
        }
        
        public static string GetLocalPathFromAbsolute(this string absolutePath)
        {
            var startIndex = absolutePath.IndexOf("Assets", StringComparison.Ordinal);
            var localPath = absolutePath.Substring(startIndex);
            return localPath;
        }

        public static string GetTypeName(this Type type)
        {
            var nameWithAssembly = type.ToString();
            var name = nameWithAssembly.Split('.')[^1];
            return name;
        }
        
        public static string GetColored(this string str,Color color)
        {
            var hexColor = ColorUtility.ToHtmlStringRGBA(color);
            return $"<color=#{hexColor}>{str}</color>";
        }
        
        public static bool TryParseToVector(this string vectorStr, out Vector3 resultVec)
        {
            var defaultFailVec = Vector3.one * -1f; 
            if (string.IsNullOrEmpty(vectorStr))
            {
                resultVec = defaultFailVec;
                return false;
            }

            vectorStr = vectorStr.TrimStart('(');
            vectorStr = vectorStr.TrimEnd(')');
            
            var axisArray = vectorStr.Split(',');
            if (axisArray.Length != 3)
            {
                resultVec = defaultFailVec;
                return false;
            }

            var hasX = float.TryParse(axisArray[0], out var xAxis);
            var hasY = float.TryParse(axisArray[1], out var yAxis);
            var hasZ = float.TryParse(axisArray[2], out var zAxis);

            if (!hasX || !hasY || !hasZ)
            {
                resultVec = defaultFailVec;
                return false;
            }

            resultVec = new Vector3(xAxis, yAxis, zAxis);
            return true;
        }
        
        public static void PrintColored(this string str, Color color)
        {
            str.GetColored(color).Print();
        }
        
        public static void PrintColored(this string str, Color color, GameObject obj)
        {
            str.GetColored(color).Print(obj);
        }
        
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool CreateFolderInside(this string rootPath, string folderName, out string resultFolderPath)
        {
            if (!Directory.Exists(rootPath))
            {
                resultFolderPath = null;
                return false;
            }

            var combined = Path.Combine(rootPath, folderName);
            
            if (Directory.Exists(combined))
            {
                resultFolderPath = combined;
                return true;
            }

            var createdInfo = Directory.CreateDirectory(combined);
            resultFolderPath = combined; 
            return true;
        }
    }
}
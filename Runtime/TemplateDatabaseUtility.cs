using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TemplateDatabase2.Packages.TemplateDatabase.Runtime;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TemplateDatabase.Packages.TemplateDatabase.Runtime
{
    public static class TemplateDatabaseUtility
    {
        public const string BackendString = "Backend";
        private static string RefreshMethodName => "Refresh";
        
#if UNITY_EDITOR
        [MenuItem("Tools/Data/Refresh all Template Databases")]
        public static void RefreshAllDatabases()
        {
            var databaseType = typeof(TemplateDatabase<>);
            var databaseGuids = AssetDatabase.FindAssets("t:ScriptableObject");
            foreach (var guid in databaseGuids)
            {
                var sObj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetDatabase.GUIDToAssetPath(guid));
                if (sObj == null || !IsSubclassOfGeneric(sObj.GetType(), databaseType)) continue;
                var method = sObj.GetType().GetMethod(RefreshMethodName, BindingFlags.Public | BindingFlags.Instance);
                if (method == null) continue;
                method.Invoke(sObj, null);
            }
        }
        
        public static IEnumerable<T> FindAllObjects<T>() where T : ScriptableObject
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}").ToList();
            var assetPaths = guids.Select(AssetDatabase.GUIDToAssetPath);
            return assetPaths.Select(AssetDatabase.LoadAssetAtPath<T>).Where(obj => obj != null);
        }
        
        public static void RefreshAsset<T>(this T asset) where T : Object
        {
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssetIfDirty(asset);
        }
#endif

        private static bool IsSubclassOfGeneric(Type type, Type genericBase)
        {
            while (type != null && type != typeof(object))
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == genericBase)
                    return true;
                type = type.BaseType;
            }

            return false;
        }
    }
}
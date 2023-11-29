using System;
using System.Reflection;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.Helpers {
    internal static class TypeExtensions {
        public static bool IsInheritorOf<T>(this Type type) {
            return type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(T));
        }

        public static bool HasAttribute<T>(this Type type) where T : Attribute {
            return type.GetCustomAttribute<T>() != null;
        }

        public static string GetAssetFileName(this Type type) {
            var attribute = type.GetCustomAttribute(typeof(CreateAssetMenuAttribute)) as CreateAssetMenuAttribute;
            return attribute == null ? string.Empty : attribute.fileName;
        }
    }
}
using NaughtyAttributes;
using TemplateDatabase.Packages.TemplateDatabase.Runtime;
using UnityEditor;
using UnityEngine;

namespace TemplateDatabase
{
    public class TemplateObject : ScriptableObject
    {
        [field: SerializeField]
        [field: Foldout(TemplateDatabaseUtility.BackendString)]
        public bool Enabled { get; private set; } = true;

        [SerializeField] [Foldout(TemplateDatabaseUtility.BackendString)] [ReadOnly]
        private string _id;

        public string ID => _id;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_id == name)
                return;
            _id = name.
                Replace(" ", string.Empty).
                Replace("_", string.Empty).
                Replace("[", string.Empty).
                Replace("]", "_").
                Replace("(", string.Empty).
                Replace(")", "_").
                Replace("+", "_").
                Replace("-", "_");
            EditorUtility.SetDirty(this);
        }
#endif
        public static bool operator ==(TemplateObject first, TemplateObject second)
        {
            if (ReferenceEquals(first, second)) return true;
            if (first is null || second is null) return false;
            return first.Equals(second);
        }

        public static bool operator !=(TemplateObject first, TemplateObject second) => !(first == second);
        public override bool Equals(object obj) => obj is TemplateObject other && Equals(other);
        private bool Equals(TemplateObject other) => other != null && ID == other.ID;
        public override int GetHashCode() => ID?.GetHashCode() ?? 0;
    }
}
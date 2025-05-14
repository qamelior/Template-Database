using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using TemplateDatabase;
using TemplateDatabase.Packages.TemplateDatabase.Runtime;
using UnityEngine;

namespace TemplateDatabase2.Packages.TemplateDatabase.Runtime
{
    public class TemplateDatabase<T> : ScriptableObject where T : TemplateObject
    {
        [SerializeField] [ReadOnly] protected List<T> _templates = new();
        [SerializeField] [field: Foldout(TemplateDatabaseUtility.BackendString)]
        private bool _throwErrorsWhenMissingData = true;
        public IReadOnlyList<T> Templates => _templates.AsReadOnly();

        public T GetTemplate(string id)
        {
            var ret = _templates.FirstOrDefault(el => el.ID == id);
            if (ret == null && _throwErrorsWhenMissingData)
                Debug.LogError($"[SaveLoad] Object with id ({id}) was not present in the database ({name})");
            return ret;
        }

#if UNITY_EDITOR
        //keep this public for reflection
        [Button]
        public void Refresh() => GatherElements();

        protected virtual void GatherElements()
        {
            var templates = TemplateDatabaseUtility.FindAllObjects<T>();
            _templates.Clear();
            foreach (var t in templates)
            {
                if (!t.Enabled) continue;
                var elementWithSameID = _templates.FirstOrDefault(el => el.ID == t.ID);
                if (elementWithSameID != null)
                {
                    Debug.LogError($"[{name}] Duplicate ID found: {t.name} and {elementWithSameID.name}");
                    return;
                }

                t.RefreshAsset();
                _templates.Add(t);
            }

            SortTemplates();
            this.RefreshAsset();
        }

        protected virtual void SortTemplates() =>
            _templates.Sort((x, y) => string.Compare(x.name, y.name, StringComparison.CurrentCulture));
#endif
    }
}
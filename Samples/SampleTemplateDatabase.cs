using TemplateDatabase.Packages.TemplateDatabase.Runtime;
using TemplateDatabase2.Packages.TemplateDatabase.Runtime;
using UnityEngine;

namespace TemplateDatabase.Packages.TemplateDatabase.Samples
{
    [CreateAssetMenu(fileName = "(DB) NewTemplateDatabase", menuName = "Templates/Samples/Database")]
    public class SampleTemplateDatabase : TemplateDatabase<SampleTemplate>
    {
        
    }
}
using System;

namespace Sitecore.Datalift
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DatasourceOrSelfAttribute : Attribute, IDataliftAttribute
    {
        public DatasourceOrSelfAttribute(string templateIdentifier)
        {
            TemplateIdentifier = templateIdentifier;
            Strategy = new DatasourceOrSelfStrategy();
        }

        public string TemplateIdentifier { get; set; }
        public IDataliftStrategy Strategy { get; set; }
    }
}
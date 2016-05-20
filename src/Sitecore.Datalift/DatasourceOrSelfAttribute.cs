using System;

namespace Sitecore.Datalift
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class DatasourceOrSelfAttribute : Attribute, IDataliftAttribute
    {
        public DatasourceOrSelfAttribute(string templateIdentifier)
        {
            TemplateIdentifier = templateIdentifier;
            Strategy = new DatasourceOrSelfStrategy();
        }

        public string TemplateIdentifier { get; }
        public IStrategy Strategy { get; set; }
    }
}
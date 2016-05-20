using System;

namespace Sitecore.Datalift
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class DatasourceOrAscendantAttribute : Attribute, IDataliftAttribute
    {
        public DatasourceOrAscendantAttribute(string templateIdentifier)
        {
            TemplateIdentifier = templateIdentifier;
            Strategy = new DatasourceOrAscendantStrategy();
        }

        public string TemplateIdentifier { get; }
        public IStrategy Strategy { get; set; }
    }
}
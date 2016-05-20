using System;

namespace Sitecore.Datalift
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class DatasourceOrSiteRootAttribute : Attribute, IDataliftAttribute
    {
        public DatasourceOrSiteRootAttribute(string templateIdentifier)
        {
            TemplateIdentifier = templateIdentifier;
            Strategy = new DatasourceOrSiteRootStrategy();
        }

        public string TemplateIdentifier { get; }
        public IStrategy Strategy { get; set; }
    }
}
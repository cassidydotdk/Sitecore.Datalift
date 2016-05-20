using System;

namespace Sitecore.Datalift
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DatasourceOrSiteRootAttribute : Attribute, IDataliftAttribute
    {
        public DatasourceOrSiteRootAttribute(string templateIdentifier)
        {
            TemplateIdentifier = templateIdentifier;
            Strategy = new DatasourceOrSiteRootStrategy();
        }

        public string TemplateIdentifier { get; }
        public IDataliftStrategy Strategy { get; set; }
    }
}
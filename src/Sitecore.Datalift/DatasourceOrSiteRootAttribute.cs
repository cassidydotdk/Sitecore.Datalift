using System;

namespace Sitecore.Datalift
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DatasourceOrSiteRootAttribute : Attribute, IDataliftAttribute
    {
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public DatasourceOrSiteRootAttribute(string templateIdentifier)
        {
            TemplateIdentifier = templateIdentifier;
            Strategy = new DatasourceOrSiteRootStrategy();
        }

        public string TemplateIdentifier { get; }
        public IDataliftStrategy Strategy { get; set; }
    }
}
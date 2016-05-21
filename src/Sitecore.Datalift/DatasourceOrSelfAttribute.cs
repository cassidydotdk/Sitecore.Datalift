using System;

namespace Sitecore.Datalift
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DatasourceOrSelfAttribute : Attribute, IDataliftAttribute
    {
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public DatasourceOrSelfAttribute(string templateIdentifier)
        {
            TemplateIdentifier = templateIdentifier;
            Strategy = new DatasourceOrSelfStrategy();
        }

        public string TemplateIdentifier { get; }
        public IDataliftStrategy Strategy { get; set; }
    }
}
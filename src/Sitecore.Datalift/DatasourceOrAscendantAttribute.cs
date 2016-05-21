using System;

namespace Sitecore.Datalift
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DatasourceOrAscendantAttribute : Attribute, IDataliftAttribute
    {
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
        public DatasourceOrAscendantAttribute(string templateIdentifier)
        {
            TemplateIdentifier = templateIdentifier;
            Strategy = new DatasourceOrAscendantStrategy();
        }

        public string TemplateIdentifier { get; }
        public IDataliftStrategy Strategy { get; set; }
    }
}
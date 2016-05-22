using System;
using Sitecore.Datalift.Strategies;

namespace Sitecore.Datalift
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DatasourceOrAscendantAttribute : Attribute, IDataliftAttribute
    {
        public DatasourceOrAscendantAttribute(string templateIdentifier)
        {
            TemplateIdentifier = templateIdentifier;
            Strategy = new DatasourceOrAscendantStrategy();
        }

        public string TemplateIdentifier { get; set; }
        public IDataliftStrategy Strategy { get; set; }
    }
}
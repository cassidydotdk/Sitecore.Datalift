using System;

namespace Sitecore.Datalift
{
    public interface IDataliftAttribute
    {
        string TemplateIdentifier { get; }
        IDataliftStrategy Strategy { get; }
    }
}
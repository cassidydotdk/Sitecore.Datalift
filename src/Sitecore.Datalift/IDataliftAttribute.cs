namespace Sitecore.Datalift
{
    public interface IDataliftAttribute
    {
        string TemplateIdentifier { get; set; }
        IDataliftStrategy Strategy { get; set; }
    }
}
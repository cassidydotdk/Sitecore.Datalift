using System.Web.Mvc;
using Sitecore.Data.Items;

namespace Sitecore.Datalift
{
    public class DataliftController : Controller
    {
        protected virtual Item GetActionItem(string datasourceString, string templateIdentifier = null, Item contextItem = null, IDataliftStrategy strategy = null)
        {
            return ((IController) this).GetActionItem(datasourceString, templateIdentifier, contextItem, strategy);
        }
    }
}
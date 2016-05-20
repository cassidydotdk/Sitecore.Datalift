using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Data.Items;

namespace Sitecore.Datalift
{
    public class DataliftController : Controller
    {
        protected virtual Item GetActionItem(string datasourceString, Item contextItem = null, IStrategy strategy = null)
        {
            return ((IController)this).GetActionItem(datasourceString, );
        }
    }
}
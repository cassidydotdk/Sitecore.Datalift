using System.Web.Mvc;
using Sitecore.Data.Items;
using Sitecore.Datalift.Strategies;
using Sitecore.Diagnostics;

namespace Sitecore.Datalift
{
    public class DataliftController : Controller
    {
        protected virtual IDataliftAttribute GetStrategyAttribute()
        {
            var customAttributes = GetType().GetCustomAttributes(typeof (IDataliftAttribute), false);
            if (customAttributes.Length > 0)
            {
                return customAttributes[0] as IDataliftAttribute;
            }

            return null;
        }

        protected virtual IDataliftStrategy GetDefaultStrategy()
        {
            return new DatasourceOrSelfStrategy();
        }

        protected virtual Item GetContextItem()
        {
            return Context.Item;
        }

        protected virtual Item GetActionItem(string datasourceString = null, string templateIdentifier = null, Item contextItem = null, IDataliftStrategy strategy = null)
        {
            var att = GetStrategyAttribute();

            if (strategy == null && att != null)
            {
                strategy = att.Strategy;
            }

            if (templateIdentifier == null && att != null)
            {
                templateIdentifier = att.TemplateIdentifier;
            }

            if (strategy == null)
                strategy = GetDefaultStrategy();

            if (contextItem == null)
                contextItem = GetContextItem();

            return strategy.Resolve(datasourceString, contextItem, templateIdentifier);
        }
    }
}
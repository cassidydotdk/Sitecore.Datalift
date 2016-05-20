using System.Web.Mvc;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Sitecore.Datalift
{
    public static class Extensions
    {
        public static IDataliftAttribute GetStrategyAttribute(this IController controller)
        {
            var customAttributes = controller.GetType().GetCustomAttributes(typeof (IDataliftAttribute), false);
            if (customAttributes.Length > 0)
            {
                return customAttributes[0] as IDataliftAttribute;
            }

            return null;
        }

        public static Item GetActionItem(this IController controller, string datasourceString, string templateIdentifier = null, Item contextItem = null, IStrategy strategy = null)
        {
            Assert.ArgumentNotNullOrEmpty(datasourceString, nameof(datasourceString));
            Assert.ArgumentNotNullOrEmpty(templateIdentifier, nameof(templateIdentifier));

            var att = GetStrategyAttribute(controller);
            if (strategy == null && att != null)
            {
                strategy = att.Strategy;
            }

            if (templateIdentifier == null && att != null)
            {
                templateIdentifier = att.TemplateIdentifier;
            }

            if (strategy == null)
                strategy = new DatasourceOrSelfStrategy();

            if (contextItem == null)
                contextItem = Context.Item;

            return strategy.Resolve(datasourceString, templateIdentifier, contextItem);
        }
    }
}
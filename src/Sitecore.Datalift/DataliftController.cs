using System;
using System.Web.Mvc;
using Sitecore.Data.Items;
using Sitecore.Datalift.Strategies;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Presentation;

namespace Sitecore.Datalift
{
	public class DataliftController : Controller
	{
		protected virtual IDataliftAttribute GetStrategyAttribute()
		{
			var customAttributes = GetType().GetCustomAttributes(typeof(IDataliftAttribute), false);
			if (customAttributes.Length > 0)
				return customAttributes[0] as IDataliftAttribute;

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

		protected virtual string GetDatasource()
		{
			return RenderingContext.CurrentOrNull?.Rendering.DataSource;
		}

		/// <summary>
		///     Looks at the current class attributes for a Strategy attribute and uses it to resolve the ActionItem. If no
		///     attribute
		///     is found, <see cref="GetDefaultStrategy" /> is used to resolve. If the attribute defines a TemplateIdentifier, it
		///     is used to validate the ActionItem.
		/// </summary>
		/// <returns>
		///     ActionItem or
		///     <value>null</value>
		/// </returns>
		protected virtual Item GetActionItem()
		{
			var att = GetStrategyAttribute();
			var strategy = att != null ? att.Strategy : GetDefaultStrategy();
			return strategy.Resolve(GetDatasource(), GetContextItem(), att?.TemplateIdentifier);
		}

		/// <summary>
		///     Looks at the current class attributes for a Strategy attribute and uses it to resolve the ActionItem. If no
		///     attribute
		///     is found, <see cref="GetDefaultStrategy" /> is used to resolve. <paramref name="templateIdentifier" /> overrides
		///     any definition on the attribute.
		/// </summary>
		/// <param name="templateIdentifier"></param>
		/// <returns></returns>
		protected virtual Item GetActionItem(string templateIdentifier)
		{
			Assert.IsNotNull(templateIdentifier, nameof(templateIdentifier));

			var att = GetStrategyAttribute();
			var strategy = att != null ? att.Strategy : GetDefaultStrategy();
			return strategy.Resolve(GetDatasource(), GetContextItem(), templateIdentifier);
		}
	}
}
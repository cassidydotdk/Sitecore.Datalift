# Sitecore.Datalift #
Datasource gymnastics by Mark Cassidy ([@cassidydotdk](https://twitter.com/cassidydotdk)) - Cassidy Consult, Switzerland

[![Cassidy Consult Logo](http://www.cassidy-consult.com/img/logo-400x263.png)](http://www.cassidy-consult.com)

*A simple library to help manage datasource resolving in a consistent manner, allowing for flexible Sitecore Information Architecture in line with good Sitecore practices.*


## So what's this? ##

Those who have followed what I write online, specifically [Working with Component Data Templates](http://intothecore.cassidy.dk/2013/09/working-with-component-data-templates.html "Working with Component Data Templates") which was part of my [Creating Good Sitecore Solutions series in 2013](http://intothecore.cassidy.dk/search/label/Creating%20good%20Sitecore%20solutions "Creating Good Sitecore Solutions series in 2013") will know; how you deal with the datasources given to you (your component) matters. 

How you chose to deal with this will affect how well your components adapt to personalisation using Sitecore eXperience Editor, but - probably more importantly - also affects the long term flexibility you will have on your Sitecore solution. Flexibily to change page structures and information architecture; flexibility to piece together components in a new way that was not originally envisioned in V1.0 and so on.

## No seriously. What's this? ##
Right. Sorry. Well it's a package. A NuGet package, to be more exact.

    Install-Package Sitecore.Datalift

And once you add this to your Sitecore MVC website project (the code is very modular, a Webforms version could rather easily be pieced together. I might, if there's enough demand for it), you will get access to `GetActionItem()` in various overloads, on your `Controller` (well `IController` actually), and on your `RenderingContext`
 
> ### I'm sorry?  "ActionItem"? ###
> 
> Ah yea. That is my own term. I use it to avoid confusion - `ActionItem` is the Sitecore Item your controller/rendering/component is meant to "action" on. That will usually be whatever the Datasource points to, but `Sitecore.Datalift` gives you a few more cards on your hand. Look at it this way; `DatasourceItem` - you would expect this to be "whatever the 'datasource' has been set to for your component. But what if it hasn't been set?  Sitecore Best Practice then says you should fall back to the `Context.Item` (no ifs, no buts). So now your `DatasourceItem` is your `Context.Item`. Doesn't really sound right does it?
> 
> So `ActionItem`. Simple as.

You also get a new `Controller`; `Sitecore.Datalift.DataliftController` that you can inherit from, nicely laid out with `protected virtual` methods for all your override needs; and some class `Attributes` you can decorate your controllers with to make things neat and tidy. 

All of this, arranged in a series of small components that can be pieced together, extended or otherwise abused - to keep things flexible, and to allow you to incorporate the functionality (as much or as little of it as you want) in whatever form suits you best.

## So what do I get? ##

- Extension Methods
	- On `IController`
	- On `RenderingContext`

Just add `using Sitecore.Datalift` and you're off with these. `GetActionItem()` becomes available.

- Class Decorator Attributes
	- `[DatasourceOrSelf]`
	- `[DatasourceOrSiteRoot]`
	- `[DatasourceOrAscendant]`
		- Which should really be named `DatasourceOrSelfOrAscendant`, but... well you know.

Use these attributes to decorate your controller. If you're doing Controller Renderings (and why wouldn't you?) - you will have configured your Controller Rendering Rendering (yes) with a Datasource Template. Tell `Sitecore.Datalift` about it using one of these attributes, and save yourself even more typing every time you call `GetActionItem()`.

- Datasource Resolution Strategies
	- `DatasourceOrSelfStrategy` (default)
	- `DatasourceOrSiteRootStrategy` (think footers, headers, etc.)
	- `DatasourceOrAscendantStrategy` (use this theme until overridden further down the hierarchy type thing)

All of them based on `BaseStrategy` (with more `virtuals`, to make your customisation life easy), and implementing `IDataliftStrategy`.

## Ok. So what else do I need to know? ##

Some conventions. 

### Datasource and fallback ###

All of the strategies follow the same convention. Some pseudo. Not the Breaking Bad kind.

    IF datasource-has-been-defined
		Resolve datasource Item
		IF templateIdentifier-has-been-defined
			IF datasource implements our template (inherits)
				USE datasource Item
			ELSE
				USE nothing (null)
		ELSE templateIdentifier-has-been-defined
			USE datasource Item (even if it resolved to null)
		END-IF templateIdentifier-has-been-defined
	ELSE datasource-has-been-defined
		APPLY strategy-fallback
	END-IF

2 things to pay particular attention to here.

1. The use of `templateIdentifier` is optional for most strategies, but I strongly recommend you use it. Always. Your component should ever only action 1 base template anyway, and the cost of adding a check to make sure your `ActionItem` actually inherits from the right base template is negligible.
2. If a datasource has been given, no fallbacks take place. *Fallbacks only ever happen when the datasource is empty*. This is what you want, even if your initial reaction might say otherwise.


### Some examples perhaps? ###

Yep, sure. 

#### Example 1 - RenderingContext extension ####

Minimum tie-in to anything. Just using the extension methods. No inheritance, no attributes.

	using Sitecore.Datalift;

	public ActionResult Index()
	{
		var actionItem = RenderingContext.GetActionItem("User Defined/Carousel");
		if (actionItem == null)
			return EmptyDatasourceView(); // not part of Sitecore.Datalift
	}

What it does?  Gets your ActionItem by first checking `RenderingContext.Rendering.Datasource`, using `RenderingContext.ContextItem` as it's context item. If datasource has been set, it will be used - if not, it applies the default `DatasourceOrSelf` strategy and falls back to context item. In either case, the resulting item is checked to inherit from `User Defined/Carousel` - if it does not, `null` is returned. 

#### Example 2 - RenderingContext extension ####

	using Sitecore.Datalift;

	public ActionResult Index()
	{
		var actionItem = RenderingContext.GetActionItem(
							"User Defined/Carousel",
							new DatasourceOrSiteRootStrategy());
		if (actionItem == null)
			return EmptyDatasourceView(); // not part of Sitecore.Datalift
	}

Same as example #1, except the fallback now goes to Site Root (via `Context.Site`, there's an overload if you want to control this behaviour).

#### Example 3 - RenderingContext extension ####

	using Sitecore.Datalift;

	public ActionResult Index()
	{
		var actionItem = RenderingContext.GetActionItem(
							null,
							new DatasourceOrSiteRootStrategy());
		if (actionItem == null)
			return EmptyDatasourceView(); // not part of Sitecore.Datalift
	}

Here, the template verification system is disabled. If a datasource has been set for your component, it gets returned. If not, you get Site Root. You can still get a `null` result, if there is no `Item Version` in your current context language.

**The `RenderingContext` extension cannot use `[Attribute]` definitions on your `Controller`.**

#### Example 4 - IController extension ####

	using Sitecore.Datalift;

	public ActionResult Index()
	{
		var actionItem = GetActionItem(RenderingContext.Rendering.Datasource);
		if (actionItem == null)
			return EmptyDatasourceView(); // not part of Sitecore.Datalift
	}

Think we're getting the hang of this now. The `IController` extension will go grab `RenderingContext.Current.Rendering.Datasource` if you don't specify one. Send it `String.Empty` if you want to force a blank datasource through.

The `IController` extension otherwise offers the same overloads. I don't think we need more examples here.

**The `IController` extension will use `[Attribute]` definitions on your `Controller` (if they're there)**

### Using [Attributes] ###

The supplied [Attributes] gives you the option to decorate your controller, to control the default Strategy behaviour and to specify the `templateIdentifier`.

So it goes like this.

	using Sitecore.Datalift;

	[DatasourceOrSiteRoot("User Defined/Carousel")]
	public CarouselController : Controller
	{
		public ActionResult Index()
		{
			var actionItem = GetActionItem();
			if (actionItem == null)
				return EmptyDatasourceView();
		}
	}

Default behaviour will now respect this, so your fallback strategy becomes `DatasourceOrSiteRootStrategy` and your `templateIdentifier` becomes `User Defined/Carousel`.

*And yes, if you prefer, you can use a GUID here. It's a normal Sitecore string identifier that gets sent to the `TemplateManager`. Only thing to remember is, if you're sending it a path - it needs to be relative to `/sitecore/system/templates/`.*

One more. Just for good measure.

	using Sitecore.Datalift;

	[DatasourceOrAscendant("User Defined/Section Menu")]
	public SectionMenuController : Controller
	{
		public ActionResult Index()
		{
			var actionItem = GetActionItem();
			if (actionItem == null)
				return EmptyDatasourceView();
		}
	}

**Just remember; these attributes only work if you use the `IController` extension. The `RenderingContext` extension doesn't know about your Controller and therefore cannot see these attributes either.**

*Attributes also work on the preferred approach (below).*

### The preferred approach, inheriting from DataliftController ###

Why is this the preferred approach?  Because it offers you the most flexibility into how `Sitecore.Datalift` behaves. Key decisions sit in `protected virtual` base methods, and you get to sit in the drivers seat when it comes to override those decisions, should you need to.

It doesn't look much different, however.

	using Sitecore.Datalift;

	[DatasourceOrSelf("User Defined/Promotion Spot")]
	public PromotionSpotController : DataliftController
	{
		public ActionResult Index()
		{
			var actionItem = GetActionItem();
			if (actionItem == null)
				return EmptyDatasourceView();
		}
	}

As mentioned; the real difference sits in your ability to `override` base methods. As of now, these are:

#### Protected Virtual methods on DataliftController ####

	protected virtual IDataliftAttribute GetStrategyAttribute()
	protected virtual IDataliftStrategy GetDefaultStrategy()
	protected virtual Item GetContextItem()
	protected virtual Item GetActionItem(string datasourceString = null, string templateIdentifier = null, Item contextItem = null, IDataliftStrategy strategy = null)

That said; `Sitecore.Datalift` really is just a selection of functionality that you can piece together as you see fit. I've tried to cover most of the common scenarios (I see) - but no doubt some people will want to use this differently.

## Extending, changing and so on ##

### Strategy ###

All strategies inherit from `BaseStrategy` but they don't need to. If you want to implement your own strategy, implement `IDataliftStrategy` and you're good to go.

The only advantage you get from inheriting `BaseStrategy` is a few `protected virtual` base methods.

	protected virtual Item GetParent(Item item)
	protected virtual bool InheritsTemplate(Item candidate, string templateIdentifier)

*A note. All the current strategies rely only on the Sitecore basic API (not MVC). You can use these directly from Webforms code*

#### Webforms example ####

	var dsos = new DatasourceOrSelfStrategy();
	var actionItem = dsos.Resolve(
			((Sublayout)Parent).DataSource,
			Sitecore.Context.Item,
			"User Defined/Carousel");

### Attributes ###

Very few requirements here. Actually only two.

1. Inherit from `System.Attribute`. A C# requirement, not mine
2. Implement `IDataliftAttribute`

*Attributes only serve to a developer usability requirement. They are entirely optional. Some (me included) like to use them, others shy away.*

### Extensions ###

To be honest, the extension methods could probably easily do with some more work. Extending in more places, or maybe you want to try and tie this functionality into your ORM mapper of choice. This package puts no requirements on extensions; it only uses them itself to tie together the bits and pieces of the API.

# Current Status #

`Sitecore.Datalift` is being pushed to the public in a state I would consider "late beta". I don't expect any major changes to the API, but there's a few things I might want to extend a little bit further before a version 1.0 is released. Either way, 1.0 release is expected in 1-2 weeks (mid-June). Until then I encourage and highly welcome feedback.

Find me on [Sitecore Slack](https://sitecorechat.slack.com/), or on Twitter ([@cassidydotdk](https://twitter.com/cassidydotdk))

> Mark Cassidy
> Cassidy Consult, Switzerland
> Personal Website: [http://www.cassidy.dk](http://www.cassidy.dk)
> Company Website:  [http://www.cassidy-consult.com](http://www.cassidy-consult.com)
> Blog: [http://intothecore.cassidy.dk](http://intothecore.cassidy.dk)
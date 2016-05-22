# Sitecore.Datalift #
Datasource gymnastics by Mark Cassidy ([@cassidydotdk](https://twitter.com/cassidydotdk)) - Cassidy Consult, Switzerland

A simple library to help manage datasource resolving in a consistent manner, allowing for flexible Sitecore Information Architecture in line with good Sitecore practices.

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
 
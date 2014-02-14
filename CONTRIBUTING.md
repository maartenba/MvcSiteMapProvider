# Contributing to MvcSiteMapProvider

Your contributions to the MvcSiteMapProvider project are welcome. While we have put a lot of time and thought into this project, we recognize that it can always be improved or extended in some way, and we appreciate the feedback. Contributions can take many forms. Here are some common ones.

1. [Opening an issue](https://github.com/maartenba/MvcSiteMapProvider/issues/new) to let us know about something that appears to be defective or to give us an idea for a new feature
2. Contributing bug fixes for features you find that are defective
3. Contributing new features
4. Helping to update the wiki documentation if it is incorrect, incomplete, or out-of-date
5. Contributing unit tests for existing features
6. Contributing "stock" configuration code for additional DI containers

### Code Contributions

First of all, please read [Don't "Push" Your Pull Requests](http://www.igvita.com/2011/12/19/dont-push-your-pull-requests/). If you are thinking of making a change that will probably result in more than 20 changed lines of code, we would appreciate you [opening an issue](https://github.com/maartenba/MvcSiteMapProvider/issues/new) to discuss the change before you start writing. It could save both you and our team quite a bit of work if the code doesn't have to be rewritten to fit in with our overall objectives. Also, please review [Open Source Contribution Etiquette](http://tirania.org/blog/archive/2010/Dec-31.html).

Please use the dev branch when contributing code. This means when you fork the project, you should base all of your modifications on the dev branch. After you have pushed your contribution to your fork on GitHub, you should open a pull request and target the dev branch.

In general, we like features that provide value to a large subset of users that are closely related to navigation and SEO.

Keep in mind when adding new code that we are targeting multiple versions of MVC as well as multiple .NET framework versions. Your code should be made to run under each of the following combinations.

.NET Framework Version | MVC Version
---------------------- | -----------
4.5 | MVC 5
4.5 | MVC 4
4.0 | MVC 4
4.5 | MVC 3
4.0 | MVC 3
4.5 | MVC 2
4.0 | MVC 2
3.5 | MVC 2

If you are referencing something that doesn't function or exist in one of the above versions, make sure you provide an alternate implementation by using conditional compilation symbols for those versions that do not support it. The following conditional compilation symbols can be used in the code using the `#if`, `#else`, and `#endif` statements.

Symbol | Representing
------ | ------------
MVC5 | ASP.NET MVC 5
MVC4 | ASP.NET MVC 4
MVC3 | ASP.NET MVC 3
MVC2 | ASP.NET MVC 2
NET45 | .NET Framework 4.5
NET40 | .NET Framework 4.0
NET35 | .NET Framework 3.5

> **Note:** To test your code in each of the version environments you need to change the MVC version using a compilation symbol, and .NET framework version by selecting it from the dropdown on the Application tab of the MvcSiteMapProvider project properties. Note that when you choose a new .NET framework version, it will automatically update the compilation symbol on the Build tab, but changing the compilation symbol doesn't automatically change the .NET framework version. Therefore, you should do the former, not the latter. However, when changing the MVC version, you need to change the compilation symbol on the Build tab directly.

#### Coding Conventions 

While we don't follow draconian coding practices, we ask that you do your best to follow existing conventions so the code is as consistent as possible. The code should be easy to read and understand, and should be broken into small, maintainable members that have a single purpose. Preferably, a single property or method will have no more than about 10-15 lines of code and a class should generally be no more than 100-200 lines of code.

Also, since we are using DI to make loosely coupled classes, we ask that you do the same. All class dependencies should be injected via constructor injection and checked with a guard clause to ensure the value can never be invalid (usually a null check will do). All dependencies should be based on an abstraction (either an interface or an abstract class). You should almost never need to use the new keyword to create an instance of a class if you follow this approach.

Here is an example:

```csharp
public class SiteMapCreator 
	: ISiteMapCreator
{
	public SiteMapCreator(
		ISiteMapCacheKeyToBuilderSetMapper siteMapCacheKeyToBuilderSetMapper,
		ISiteMapBuilderSetStrategy siteMapBuilderSetStrategy,
		ISiteMapFactory siteMapFactory
		)
	{
		if (siteMapCacheKeyToBuilderSetMapper == null)
			throw new ArgumentNullException("siteMapCacheKeyToBuilderSetMapper");
		if (siteMapBuilderSetStrategy == null)
			throw new ArgumentNullException("siteMapBuilderSetStrategy");
		if (siteMapFactory == null)
			throw new ArgumentNullException("siteMapFactory");
		
		this.siteMapCacheKeyToBuilderSetMapper = siteMapCacheKeyToBuilderSetMapper;
		this.siteMapBuilderSetStrategy = siteMapBuilderSetStrategy;
		this.siteMapFactory = siteMapFactory;
	}
	
	protected readonly ISiteMapCacheKeyToBuilderSetMapper siteMapCacheKeyToBuilderSetMapper;
	protected readonly ISiteMapBuilderSetStrategy siteMapBuilderSetStrategy;
	protected readonly ISiteMapFactory siteMapFactory;

	
	// Implementation here
}
```

If you are creating a new class or interface, please add a sentence or two in a comment header to describe what the purpose of the class or interface is. It is not necessary to comment properties or methods, but please use method and variable names that describe what they do.

### Unit Tests

If you have the time, we would also appreciate contributing unit tests to demonstrate that your new functionality works as expected. Or, you may also help by writing tests for the existing functionality. Please add all unit tests to the MvcSiteMapProvider.Tests project in the Unit folder.

Our unit tests use NUnit and Moq. Yours should, too. If you want to add another tool to the box, please run it by us in advance. Generally, as long as it is well supported, open source, popular, and doesn't conflict with an existing tool, it should be okay.

#### Conventions

- Path is ```[TestType]\[PathToClassUnderTest]\[ClassUnderTest]``` (with suffix of Test). Example: ```Unit\Loader\SiteMapLoaderTest.cs```. Integration tests can be added later using this convention.
- Test method name is ```[MemberName]_[Conditions]_[ExpectedResult]```. Example: ```GetSiteMap_WithSiteMapCacheKey_ShouldCallGetOrAddWithSiteMapCacheKey()```
- ExpectedResult should always begin with the word Should.
- Tests follow the Arrange, Act, Assert pattern.

### Dependencies

We try to keep tight control over the dependencies MvcSiteMapProvider has in order to make it easy to work with. 

#### .NET Framework Libraries

In general, adding dependencies to .NET framework libraries is allowed but common sense should be exercised. For example, there really shouldn't need to be a reason to reference Windows Forms and if you want to do so, you should look for a web-based alternative within the .NET framework or MVC.

#### 3rd Party Libraries

Always contact us first ([open a new issue](https://github.com/maartenba/MvcSiteMapProvider/issues/new)) before adding dependencies on 3rd party libraries. Due to the open-source nature of our project, commercially based libraries are not allowed. Projects that are referenced should have open licenses. 

In general, there should be a reason for referencing a 3rd party library that is compelling enough to outweigh the disadvantage of including the extra dependency. For example, we would have preferred not to add a dependency on WebActivator. But given the fact we could not otherwise startup MvcSiteMapProvider without having our end users manually adding configuration code to Global.asax, it made sense to take on this dependency.
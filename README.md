# C# MVC5 Enumerable to HTML table c#.NET

[![wndrr MyGet Build Status](https://www.myget.org/BuildSource/Badge/wndrr?identifier=22318d40-5a11-4ebf-9fe3-36c2e24e05f5)](https://www.myget.org/)

## Disclaimer - PRERELEASE

This c#.NET MVC5 library is still under heavy development. It's an alpha release, so don't use that in your prod environment (yet).  I haven't taken the time to properly test it and it doesn't yet implement all of the features I intend it to contain.

## Future of the project

At this point in time, it's just a simple fluent-API. For future releases, I intend to:
- [ ] Extend the fluent API with functionalities such as columns sorting, full-row filtering, ...
- [ ] Add attributes to allow parameterization via class decoration
- [ ] Add bootstrap integration
- [ ] Add [JQuery-DataTable](https://datatables.net/) integration (or create a clone of the plugin, depending on legal/technical difficulties)
- [ ] Remove the [RazorEngine](https://preview.nuget.org/packages/RazorEngine/) [dependency](#prerequisites)

### Prerequisites

The always up-to-date list of dependencies can be found on [NuGet](https://preview.nuget.org/packages/MVC5HtmlTable/). For the version 1.0.0.3, the list is as follow:
- [Microsoft.AspNet.Mvc](https://preview.nuget.org/packages/Microsoft.AspNet.Mvc/) >= v5.2.3
- [Microsoft.AspNet.Razor](https://preview.nuget.org/packages/Microsoft.AspNet.Razor/) >= 3.2.3
- [RazorEngine](https://preview.nuget.org/packages/RazorEngine/) >=3.10.0


## Install

The package can be found on;
- [MyGet](https://www.myget.org/feed/wndrr/package/nuget/MVC5HtmlTable) - [https://www.myget.org/feed/wndrr/package/nuget/MVC5HtmlTable](https://www.myget.org/feed/wndrr/package/nuget/MVC5HtmlTable)
- [NuGet](https://preview.nuget.org/packages/MVC5HtmlTable/) - [https://preview.nuget.org/packages/MVC5HtmlTable/](https://preview.nuget.org/packages/MVC5HtmlTable/)
- [Github](https://github.com/Wndrr/MVC5HtmlTable) - [https://github.com/Wndrr/MVC5HtmlTable](https://github.com/Wndrr/MVC5HtmlTable)

To install it, you will need a NuGet client running (a standalone CLI, the Visual Studio integration, or any other installation will do).

If you use the Visual Studio NuGet integration you can simply open the VS NuGet command line (Tools -> NuGet Package Manager -> Package Manager Console) and run the command `Install-Package MVC5HtmlTable` to get the latest available release or `Install-Package MVC5HtmlTable -Version x.x.x.x` to get a specific version. You can check the full versions list at the bottom of [this page](https://preview.nuget.org/packages/MVC5HtmlTable/)

## Getting started

Getting started with this library is fairly easy. All you really need to do is install it and call 

    @Html.DisplayTable(m => m.ListToConvertToTable).Render();

where ListToConvertToTable can be implicitly cast to an IEnumerable and is a property of the ViewModel passed to the Razor view. 
Every parameterization fluent API call has to be made before the call to `.Render()` (unless if you're using the [disposable context](#a-few-examples)).
### Examples

#### Context

    // View model to that will be passed to the Razor view
    public class TestViewModel
    {
        // We will consider that this property was filled with multiple rows in the controller
        public List<RowViewModel> ListTest { get; set; } 
    }
    
    public class RowViewModel
    {
        public string Col1 { get; set; } = "Col1Value";
        public string Col2 { get; set; } = "Col2Value";
        public string Col3 { get; set; } = "Col3Value";
        public long Col4 { get; set; } = 18;
        public bool Col5 { get; set; } = false;
    }

And in the view, the model will be strongly typed like so

    @model TestViewModel

#### Minimal call

The least code needed to make use of the library is
    
    // Fluent API calls go before the .Render() !
    @Html.DisplayTable(m => m.ListTest).Render();

or using the Disposable version as follow

    @using (var table = Html.DisplayTable(m => m.ListTest).Begin)
    {
        // Fluent API calls go here !
    }
**Note** : When using the Disposable version, a call to `Begin` should be made instead of `.Render()`.

#### Configuration

The library uses a fluent API style, which allows you to chain calls.

    @Html.DisplayTable(m => m.ListTest).Rename(m => m.Col1, "newName").Rename(m => m.Col2, "RENAMED").Render();

In the table rendered by the above call, the property the property `Col1` will not be displayed in the header of the table under the name `Col1`, but `newName` instead. The `Col2` property will then become `RENAMED`.

As many chained calls can be made, there are no restrictions except that the call MUST end with `.Render()` in a non-Disposable context. The `.Render()` call will return the generated HTML table as an `IHtmlString`.

Details, technical documentation and full examples *coming soon*
    
## Contributing

Please read [CONTRIBUTING.md](https://github.com/Wndrr/MVC5HtmlTable/blob/master/CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull requests to us.

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the full list of versions available, see the [tags on this repository](https://github.com/Wndrr/MVC5HtmlTable/tags). For the full list of available downloads see [MyGet](https://www.myget.org/feed/wndrr/package/nuget/MVC5HtmlTable) or [NuGet](https://preview.nuget.org/packages/MVC5HtmlTable/)

## Authors

* **Mathieu VIALES** aka. Wndrr

## License

*coming soon*

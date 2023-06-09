# Umbrella.Infrastructure.Templating

To install it, use proper command:

```
dotnet add package Umbrella.Infrastructure.Templating 
```

For more details about download, see [NuGet Web Site](https://www.nuget.org/packages/Umbrella.Infrastructure/)

# How to use the template engine

actually there are 2 types of supported engines: string replacer and razor.

- String  Replacer is the simplest one, since it replace a placeholder in the templates with a string.
- Razor insteas is based on Razor light library, so you can define a model to actualize the tempalte using razor syntax.

for more information bout Razor Light, see the package on [NuGet](https://www.nuget.org/packages/RazorLight)

the usage is stright forward:

- Create the replacer
- Define the tempalte
- create the model to actualize template
- use the engine to generate the actualized output

## String Replacer

Using the string replacer is very simple:

```c#
// create the renderer
this._Renderer = new StringReplaceRenderer();
// define the template
string template = "sup ##Name## here is a number ##Number##";
// define the model
var model = new ViewModel() { Name = "LUKE", Number = 123 };
// actualize the template
var body = this._Renderer.Parse<ViewModel>(template, model);
// expected string: sup LUKE here is a number 123
```

As you see in the snippet, the sintax is using _##[Property Name]##_ to identify the variables in the template.

## Razor Replacer

The most interesting feature is using Razor engine to manage tempaltes. Here you fine the example.

```c#
// create the renderer
this._Renderer = new RazorRenderer();
// define the template
string template = "sup @Model.Name here is a list @foreach(var i in Model.Numbers) { @i }";
// define the model
var model = new ViewModelWithViewBag() { Name = "LUKE", Numbers = new string[] { "1", "2", "3" } };
// actualize the template
var body = this._Renderer.Parse<ViewModelWithViewBag>(template, model)
// expected string: sup LUKE here is a list 123
```

The sintax here is Razor one.

## Using cshtml templates

the first improvement on Razor engine is using an external cshtml file to store the template.

```c#
// create the renderer
this._Renderer = new RazorRenderer(Directory.GetCurrentDirectory());
// define the template
string template = @"
@{
    Layout = ""./Razor/Shared/_Layout.cshtml"";
}
sup @Model.Name here is a list @foreach(var i in Model.Numbers) { @i }";
// define the model
dynamic viewBag = new ExpandoObject();
viewBag.Title = "Hello!";
var model = new ViewModelWithViewBag { Name = "LUKE", Numbers = new[] { "1", "2", "3" }, ViewBag = viewBag };

// actualize the template
var body = this._Renderer.Parse<ViewModelWithViewBag>(template, model)
// expected string:
//<h1>Hello!</h1>
//<div>sup LUKE here is a list 123</div>
```

To use such feature you have to define the layout file as well, stored into folder _Razor/Shared_  compared to folder defined as root into _RazorRenderer_.

```html
<h1>@ViewBag.Title</h1>
<div>@RenderBody()</div>
```

## Using cshtml templates embedded into assembly

the second improvement on Razor engine is using an external cshtml file embedded into assembly.

```c#
// create the renderer
this._Renderer = new RazorRenderer(typeof(RazorTests), isVerbose: true);
// define the template
string template = @"
@{
    Layout = ""./Razor/Shared/_Layout.cshtml"";
}
sup @Model.Name here is a list @foreach(var i in Model.Numbers) { @i }";
// define the model
dynamic viewBag = new ExpandoObject();
viewBag.Title = "Hello!";
var model = new ViewModelWithViewBag { Name = "LUKE", Numbers = new[] { "1", "2", "3" }, ViewBag = viewBag };

// actualize the template
var body = this._Renderer.Parse<ViewModelWithViewBag>(template, model)
// expected string:
//<h1>Hello!</h1>
//<div>sup LUKE here is a list 123</div>
```

as you see here, the only difference is the constructor, where you ahve to specify not the root folder, but a type that is sotred at root of assembly.

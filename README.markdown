# Sparc.Mvc
This library provides a slightly more flexible alternative to Razor's sections.

The primary goals of this project are:

* To allow a child page to completely overwrite a block defined in one of its layouts
* Insert content into a block defined in one of the layouts
* Conditionally render a block only if a child page also defines the block
* Define a block once (and never needed to re-define in a nested layout page)

# Installing
Sparc.Mvc is available on NuGet here: http://nuget.org/packages/Sparc.Mvc

# Usage
To use, your pages can inherit from Sparc.Mvc.BlockablePage, or you can call Sparc.Mvc.BlockablePage.RenderBlock or Sparc.Mvc.BlockablePage.DefineBlock.  To make your pages inherit from BlockablePage, modify the pages element in your web.config (not the primary web.config) to have this entry: <pages pageBaseType="Sparc.Mvc.BlockablePage">

    // Render a block, and require it to be defined (required defaults to false).
    @RenderBlock("MainHeader").Required()
    
    // Define and render a block at the same time, allowing children to insert content into the block.
    @RenderBlock("MainHeader").As(@<h1>Hello @RenderBlock("MainHeader")</h1>)
    
    // Define a block.
    @DefineBlock("MainHeader").As(@<h1>This is some content</h1>)
    
    // Define a block which requires a child view to also define the block.
    @DefineBlock("MainHeader").Required().As(@<div id="main-header" style="font-size: 3em">Header[ @RenderBlock("MainHeader") ]</div>)

    // Define a block which renders a default message if no child view defines the block.
    @DefineBlock("MainHeader").IfDefined(@<h1>@RenderBlock("MainHeader")</h1>).Else(@<p>Main header wasn't defined.</p>)
    
    // Define a block which overwrites any previous/parent definitions of the block.
    @DefineBlock("MainHeader").As(@<h1>Hello World!</h1>).OverwriteParents()

    // Multi-tag definition requires use of Razor's special <text></text> tag.
    @DefineBlock("MainHeader").As(@<text>
        <link href="@Url.Content("~/Content/login.less")" rel="stylesheet" type="text/css" />
        <script src="@Href("~/Scripts/hf.authentication.js")" type="text/javascript"></script>
    </text>)

# License (MIT)
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
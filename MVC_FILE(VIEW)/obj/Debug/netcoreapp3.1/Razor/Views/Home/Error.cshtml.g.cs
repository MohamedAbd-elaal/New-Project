#pragma checksum "F:\.net_core\tes1\Start-Identity-7\MVC_FILE(VIEW)\Views\Home\Error.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "028258f2210b37e21afe8ebfbe558f6e4e806ef7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Error), @"mvc.1.0.view", @"/Views/Home/Error.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "F:\.net_core\tes1\Start-Identity-7\MVC_FILE(VIEW)\Views\_ViewImports.cshtml"
using MVC_FILE_VIEW_.ViewModel;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "F:\.net_core\tes1\Start-Identity-7\MVC_FILE(VIEW)\Views\_ViewImports.cshtml"
using MVC_FILE_VIEW_.Model;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "F:\.net_core\tes1\Start-Identity-7\MVC_FILE(VIEW)\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"028258f2210b37e21afe8ebfbe558f6e4e806ef7", @"/Views/Home/Error.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"27cb38ed5815db4b81e82a89118caa81d5a30eeb", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Error : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "F:\.net_core\tes1\Start-Identity-7\MVC_FILE(VIEW)\Views\Home\Error.cshtml"
 if(ViewBag.ErrorTitle==null)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("<div class=\"alert alert-danger\">\r\n    <h4>Exceptionpath: </h4>\r\n    <hr />\r\n    <p>");
#nullable restore
#line 6 "F:\.net_core\tes1\Start-Identity-7\MVC_FILE(VIEW)\Views\Home\Error.cshtml"
  Write(ViewBag.Exceptionpath);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n</div>\r\n");
            WriteLiteral("<div class=\"alert alert-danger\">\r\n    <h4>ExceptionMessage: </h4>\r\n    <hr />\r\n    <p>");
#nullable restore
#line 12 "F:\.net_core\tes1\Start-Identity-7\MVC_FILE(VIEW)\Views\Home\Error.cshtml"
  Write(ViewBag.ExceptionMessage);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n</div>\r\n");
            WriteLiteral("<div class=\"alert alert-danger\">\r\n    <h4>Exceptionpath: </h4>\r\n    <hr />\r\n    <p>");
#nullable restore
#line 18 "F:\.net_core\tes1\Start-Identity-7\MVC_FILE(VIEW)\Views\Home\Error.cshtml"
  Write(ViewBag.StackTrace);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n</div>\r\n");
#nullable restore
#line 20 "F:\.net_core\tes1\Start-Identity-7\MVC_FILE(VIEW)\Views\Home\Error.cshtml"
}
else{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <h1 class=\"text-danger\">");
#nullable restore
#line 22 "F:\.net_core\tes1\Start-Identity-7\MVC_FILE(VIEW)\Views\Home\Error.cshtml"
                       Write(ViewBag.ErrorTitle);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h1>\r\n    <h5 class=\"text-danger\">");
#nullable restore
#line 23 "F:\.net_core\tes1\Start-Identity-7\MVC_FILE(VIEW)\Views\Home\Error.cshtml"
                       Write(ViewBag.ErrorMessage);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h5>\r\n");
#nullable restore
#line 24 "F:\.net_core\tes1\Start-Identity-7\MVC_FILE(VIEW)\Views\Home\Error.cshtml"
}

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591

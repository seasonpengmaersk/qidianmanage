#pragma checksum "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "28b6847b86c5e2f36631b0822e14cfae6cbced8a"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_ViewTerminals_Search), @"mvc.1.0.view", @"/Views/ViewTerminals/Search.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/ViewTerminals/Search.cshtml", typeof(AspNetCore.Views_ViewTerminals_Search))]
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
#line 1 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\_ViewImports.cshtml"
using ReadExcelPOC;

#line default
#line hidden
#line 2 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\_ViewImports.cshtml"
using ReadExcelPOC.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"28b6847b86c5e2f36631b0822e14cfae6cbced8a", @"/Views/ViewTerminals/Search.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1f84d85a95d8f8876cd659c7b5ad42adb95b62d3", @"/Views/_ViewImports.cshtml")]
    public class Views_ViewTerminals_Search : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<ReadExcelPOC.Models.Terminal>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Edit", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Details", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Delete", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(50, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 3 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
  
    ViewData["Title"] = "Search";

#line default
#line hidden
            BeginContext(94, 103, true);
            WriteLiteral("\r\n<h1>Search</h1>\r\n<table class=\"table\">\r\n    <thead>\r\n        <tr>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(198, 49, false);
#line 12 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
           Write(Html.DisplayNameFor(model => model.TerminalGEOID));

#line default
#line hidden
            EndContext();
            BeginContext(247, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(303, 43, false);
#line 15 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
           Write(Html.DisplayNameFor(model => model.SubArea));

#line default
#line hidden
            EndContext();
            BeginContext(346, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(402, 52, false);
#line 18 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
           Write(Html.DisplayNameFor(model => model.TerminalRKSTCode));

#line default
#line hidden
            EndContext();
            BeginContext(454, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(510, 44, false);
#line 21 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
           Write(Html.DisplayNameFor(model => model.PortRKST));

#line default
#line hidden
            EndContext();
            BeginContext(554, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(610, 45, false);
#line 24 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
           Write(Html.DisplayNameFor(model => model.PortGEOID));

#line default
#line hidden
            EndContext();
            BeginContext(655, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(711, 48, false);
#line 27 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
           Write(Html.DisplayNameFor(model => model.TerminalName));

#line default
#line hidden
            EndContext();
            BeginContext(759, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(815, 44, false);
#line 30 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
           Write(Html.DisplayNameFor(model => model.PortName));

#line default
#line hidden
            EndContext();
            BeginContext(859, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(915, 48, false);
#line 33 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
           Write(Html.DisplayNameFor(model => model.ExtendField1));

#line default
#line hidden
            EndContext();
            BeginContext(963, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(1019, 48, false);
#line 36 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
           Write(Html.DisplayNameFor(model => model.ExtendField2));

#line default
#line hidden
            EndContext();
            BeginContext(1067, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(1123, 48, false);
#line 39 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
           Write(Html.DisplayNameFor(model => model.ExtendField3));

#line default
#line hidden
            EndContext();
            BeginContext(1171, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(1227, 48, false);
#line 42 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
           Write(Html.DisplayNameFor(model => model.ExtendField4));

#line default
#line hidden
            EndContext();
            BeginContext(1275, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(1331, 48, false);
#line 45 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
           Write(Html.DisplayNameFor(model => model.ExtendField5));

#line default
#line hidden
            EndContext();
            BeginContext(1379, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(1435, 48, false);
#line 48 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
           Write(Html.DisplayNameFor(model => model.ExtendField6));

#line default
#line hidden
            EndContext();
            BeginContext(1483, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(1539, 48, false);
#line 51 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
           Write(Html.DisplayNameFor(model => model.ExtendField7));

#line default
#line hidden
            EndContext();
            BeginContext(1587, 55, true);
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
            EndContext();
            BeginContext(1643, 48, false);
#line 54 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
           Write(Html.DisplayNameFor(model => model.ExtendField8));

#line default
#line hidden
            EndContext();
            BeginContext(1691, 86, true);
            WriteLiteral("\r\n            </th>\r\n            <th></th>\r\n        </tr>\r\n    </thead>\r\n    <tbody>\r\n");
            EndContext();
#line 60 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
 foreach (var item in Model) {

#line default
#line hidden
            BeginContext(1809, 48, true);
            WriteLiteral("        <tr>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(1858, 48, false);
#line 63 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
           Write(Html.DisplayFor(modelItem => item.TerminalGEOID));

#line default
#line hidden
            EndContext();
            BeginContext(1906, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(1962, 42, false);
#line 66 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
           Write(Html.DisplayFor(modelItem => item.SubArea));

#line default
#line hidden
            EndContext();
            BeginContext(2004, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(2060, 51, false);
#line 69 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
           Write(Html.DisplayFor(modelItem => item.TerminalRKSTCode));

#line default
#line hidden
            EndContext();
            BeginContext(2111, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(2167, 43, false);
#line 72 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
           Write(Html.DisplayFor(modelItem => item.PortRKST));

#line default
#line hidden
            EndContext();
            BeginContext(2210, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(2266, 44, false);
#line 75 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
           Write(Html.DisplayFor(modelItem => item.PortGEOID));

#line default
#line hidden
            EndContext();
            BeginContext(2310, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(2366, 47, false);
#line 78 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
           Write(Html.DisplayFor(modelItem => item.TerminalName));

#line default
#line hidden
            EndContext();
            BeginContext(2413, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(2469, 43, false);
#line 81 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
           Write(Html.DisplayFor(modelItem => item.PortName));

#line default
#line hidden
            EndContext();
            BeginContext(2512, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(2568, 47, false);
#line 84 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
           Write(Html.DisplayFor(modelItem => item.ExtendField1));

#line default
#line hidden
            EndContext();
            BeginContext(2615, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(2671, 47, false);
#line 87 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
           Write(Html.DisplayFor(modelItem => item.ExtendField2));

#line default
#line hidden
            EndContext();
            BeginContext(2718, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(2774, 47, false);
#line 90 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
           Write(Html.DisplayFor(modelItem => item.ExtendField3));

#line default
#line hidden
            EndContext();
            BeginContext(2821, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(2877, 47, false);
#line 93 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
           Write(Html.DisplayFor(modelItem => item.ExtendField4));

#line default
#line hidden
            EndContext();
            BeginContext(2924, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(2980, 47, false);
#line 96 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
           Write(Html.DisplayFor(modelItem => item.ExtendField5));

#line default
#line hidden
            EndContext();
            BeginContext(3027, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(3083, 47, false);
#line 99 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
           Write(Html.DisplayFor(modelItem => item.ExtendField6));

#line default
#line hidden
            EndContext();
            BeginContext(3130, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(3186, 47, false);
#line 102 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
           Write(Html.DisplayFor(modelItem => item.ExtendField7));

#line default
#line hidden
            EndContext();
            BeginContext(3233, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(3289, 47, false);
#line 105 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
           Write(Html.DisplayFor(modelItem => item.ExtendField8));

#line default
#line hidden
            EndContext();
            BeginContext(3336, 55, true);
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
            EndContext();
            BeginContext(3391, 53, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "28b6847b86c5e2f36631b0822e14cfae6cbced8a18451", async() => {
                BeginContext(3436, 4, true);
                WriteLiteral("Edit");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#line 108 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
                                       WriteLiteral(item.Id);

#line default
#line hidden
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(3444, 20, true);
            WriteLiteral(" |\r\n                ");
            EndContext();
            BeginContext(3464, 59, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "28b6847b86c5e2f36631b0822e14cfae6cbced8a20823", async() => {
                BeginContext(3512, 7, true);
                WriteLiteral("Details");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#line 109 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
                                          WriteLiteral(item.Id);

#line default
#line hidden
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(3523, 20, true);
            WriteLiteral(" |\r\n                ");
            EndContext();
            BeginContext(3543, 57, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "28b6847b86c5e2f36631b0822e14cfae6cbced8a23201", async() => {
                BeginContext(3590, 6, true);
                WriteLiteral("Delete");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#line 110 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
                                         WriteLiteral(item.Id);

#line default
#line hidden
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(3600, 36, true);
            WriteLiteral("\r\n            </td>\r\n        </tr>\r\n");
            EndContext();
#line 113 "D:\Development\Temp\AzureCICDDemo\JRTestRepo\ReadExcelPOC\ReadExcelPOC\Views\ViewTerminals\Search.cshtml"
}

#line default
#line hidden
            BeginContext(3639, 24, true);
            WriteLiteral("    </tbody>\r\n</table>\r\n");
            EndContext();
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<ReadExcelPOC.Models.Terminal>> Html { get; private set; }
    }
}
#pragma warning restore 1591

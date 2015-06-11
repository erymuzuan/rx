﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASP
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 1 "..\..\Views\CustomForm\DiffLine.cshtml"
    using DiffPlex.DiffBuilder.Model;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/CustomForm/DiffLine.cshtml")]
    public partial class _Views_CustomForm_DiffLine_cshtml : System.Web.Mvc.WebViewPage<DiffPiece>
    {
        public _Views_CustomForm_DiffLine_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Views\CustomForm\DiffLine.cshtml"
 if (!string.IsNullOrEmpty(Model.Text))
{
    const string spaceValue = "\u00B7";
    const string tabValue = "\u00B7\u00B7";
    if (Model.Type == ChangeType.Deleted || Model.Type == ChangeType.Inserted)
    {
        
            
            #line default
            #line hidden
            
            #line 9 "..\..\Views\CustomForm\DiffLine.cshtml"
   Write(Html.Encode(Model.Text).Replace(" ", spaceValue).Replace("\t", tabValue));

            
            #line default
            #line hidden
            
            #line 9 "..\..\Views\CustomForm\DiffLine.cshtml"
                                                                                 
    }
    else if (Model.Type == ChangeType.Modified)
    {
        foreach (var character in Model.SubPieces)
        {
            if (character.Type == ChangeType.Imaginary)
            {
                continue;
            }

            
            #line default
            #line hidden
WriteLiteral("            <span");

WriteAttribute("class", Tuple.Create(" class=\"", 610), Tuple.Create("\"", 663)
            
            #line 19 "..\..\Views\CustomForm\DiffLine.cshtml"
, Tuple.Create(Tuple.Create("", 618), Tuple.Create<System.Object, System.Int32>(string.Format("{0}Character",character.Type)
            
            #line default
            #line hidden
, 618), false)
);

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 20 "..\..\Views\CustomForm\DiffLine.cshtml"
           Write(Html.Raw(character.Text.Replace(" ", spaceValue)));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </span>\r\n");

            
            #line 22 "..\..\Views\CustomForm\DiffLine.cshtml"
        }
    }
    else
    {
        
        
            
            #line default
            #line hidden
            
            #line 27 "..\..\Views\CustomForm\DiffLine.cshtml"
   Write(Model.Text.Replace(" ", spaceValue).Replace("\t", tabValue));

            
            #line default
            #line hidden
            
            #line 27 "..\..\Views\CustomForm\DiffLine.cshtml"
                                                                    
    }

}

            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591

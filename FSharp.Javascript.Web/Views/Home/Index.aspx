<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Main.Master" Inherits="System.Web.Mvc.ViewPage<FSharp.Javascript.Web.Model.ModuleCompilerView>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	FSharp.Javascript
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <p>On this page you can copy your F# code that you wish to convert into javascript.  Please ensure that your F# is declared as a module and that all functions that you wish to be translated are marked with a <%= Html.Encode("[<ReflectedDefinition>]") %> attribute.</p>

    <p>Upon submission your javascript will be run in the browser.  You may use the jquery api provided in the FSharp.Javascript project to manipulate the elements on this page.  It is suggested that you use the id of "output" so as not to break the other functionality on this page.</p>

    <p>Please note that free standing code that runs when a module is initialized will not be picked up by the compiler.  Instead please write a main() method and this page will call it automatically for you.</p>

    <%= Html.ValidationSummary() %>
    <form action="<%= this.ResolveUrl("~/home/submit") %>" method="post">
        <label>Input F# here:</label><br />
        <%= Html.TextAreaFor(a => a.FSharp, new { style="width: 600px; height: 200px;" })%>
        <br />
        <input type="submit" />
    </form>

    <div id="results">
        <p>Call jquery("#output").html(your value) to output to this area.</p>
        <fieldset id="output">&nbsp;</fieldset>
    </div>
    <br />
    <script type="text/javascript">
         $(document).ready(function(){
            <%= Model.Javascript %>
        })
        
    </script>
    <%if(Model.Javascript != ""){ %>
        <p>Here is the output javascript:</p>
        <%= Html.TextAreaFor(a => a.Javascript, new { style = "width: 600px; height: 200px;" })%>
    <%} %>
</asp:Content>

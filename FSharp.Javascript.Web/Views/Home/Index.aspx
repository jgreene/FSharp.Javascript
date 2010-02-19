<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Main.Master" Inherits="System.Web.Mvc.ViewPage<FSharp.Javascript.Web.Model.ModuleCompilerView>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <p>On this page you may copy your F# code that you wish to convert into javascript.  Please ensure that your F# is declared as a module and that all functions that you wish to be translated are marked with a <%= Html.Encode("[<ReflectedDefinition>]") %> attribute.</p>

    <p>Upon submission your javascript will be run in the browser.  You may use the jquery api provided in the FSharp.Javascript project to manipulate the elements on this page.  It is suggested that you use the id of "output" so as not to break the other functionality on this page.</p>

    <%= Html.ValidationSummary() %>
    <form action="/home/submit" method="post">
        <label>Input F# here:</label><br />
        <%= Html.TextAreaFor(a => a.FSharp, new { style="width: 600px; height: 200px;" })%>
        <br />
        <input type="submit" />
    </form>

    <div id="results">
        <p>Your results will be displayed below.  Call jquery("#output").html(your value) to output to this area.</p>
        <fieldset id="output">&nbsp;</fieldset>
    </div>
    <br />
    <script type="text/javascript">
        <%= Model.Javascript %>

        <%= Model.ModuleName %>.init()
    </script>

    <%= Html.TextAreaFor(a => a.Javascript, new { style = "width: 600px; height: 200px;" })%>
</asp:Content>

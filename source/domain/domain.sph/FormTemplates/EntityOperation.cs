namespace Bespoke.Sph.Domain
{
    public partial class EntityOperation : DomainObject
    {
        public string GetConfirmationMessage()
        {
            if (string.IsNullOrWhiteSpace(this.SuccessMessage)) return string.Empty;
            var nav = string.Empty;
            if (!string.IsNullOrWhiteSpace(this.NavigateSuccessUrl))
                nav = "window.location='" + this.NavigateSuccessUrl + "'";

            return string.Format(@" 
                                    app.showMessage(""{0}"", ""{1}"", [""OK""])
	                                    .done(function (dialogResult) {{
                                            console.log();
                                            {2}
	                                    }});
                                 ", this.SuccessMessage, ConfigurationManager.ApplicationFullName, nav);
        }
    }
}
namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class LabelItem : ReportItem
    {
        private ReportDefinition m_rdl;
        public override void SetRdl(ReportDefinition rdl)
        {
            m_rdl = rdl;
        }

        public string EvaluateHtml()
        {
            if (string.IsNullOrWhiteSpace(this.Html)) return string.Empty;
            if (this.Html.StartsWith("="))
            {
                var engine = ObjectBuilder.GetObject<IScriptEngine>();
                var script = this.Html.Substring(1, this.Html.Length - 1);
                var output = engine.Evaluate(script, m_rdl);

                return string.Format("{0}", output);
            }

            if (this.Html.StartsWith("@"))
            {
                var output = this.m_rdl.Param(this.Html.Replace("@", string.Empty));
                return string.Format("{0}", output);
            }

            return this.Html;
        }

    }
}

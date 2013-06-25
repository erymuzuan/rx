namespace routes.editor
{
    public class JsRoute
    {
        public string role { set; get; }
        public string url { set; get; }
        public string moduleId { set; get; }
        public string name { set; get; }
        public bool visible { set; get; }
        public string icon { set; get; }
        public string caption { set; get; }
        public JsRouteSetting settings { set; get; }
    }
}
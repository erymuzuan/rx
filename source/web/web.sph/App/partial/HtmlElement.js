/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />



bespoke.sph.domain.HtmlElementPartial = function () {

    var editHtml = function() {
        var self = this;
        var w = window.open("/editor/ace?mode=html", '_blank', 'height=' + screen.height + ',width=' + screen.width + ',toolbar=0,location=0,fullscreen=yes');
        w.code = ko.unwrap(this.Text);
        w.saved = function (code, close) {
            self.Text(code);
            if (close) {
                w.close();
            }
        };
    };
    return {
        editHtml: editHtml
    };
};
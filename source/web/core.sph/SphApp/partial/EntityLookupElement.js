/// <reference path="../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />

bespoke.sph.domain.EntityLookupElementPartial = function () {

    var editDisplayTemplate = function () {
        var self = this,
            w = window.open("/sph/editor/ace?mode=javascript", "_blank", "height=" + screen.height + ",width=" + screen.width + ",toolbar=0,location=0,fullscreen=yes"),
            wdw = w.window || w,
            init = function () {
                wdw.code = ko.unwrap(self.Command);
                if (!w.code) {
                    w.code = "//insert your code here";
                }
                wdw.saved = function (code, close) {
                    self.DisplayTemplate(code);
                    if (close) {
                        w.close();
                    }
                };
            };
        if (wdw.attachEvent) { // for ie
            wdw.attachEvent("onload", init);
        } else {
            init();
        }
    },
    editColumns = function () {
            var self = this;
            require(["viewmodels/members.selector.dialog", "durandal/app"], function (dialog, app2) {
                dialog.entity(self.Entity());
                dialog.selectedMembers(self.LookupColumnCollection());
                app2.showDialog(dialog,self.Entity())
                    .done(function (result) {
                        if (!result) return;
                        if (result === "OK") {
                            self.LookupColumnCollection(dialog.selectedMembers());
                        }
                    });

            });
        },
        editFilter = function() {

            var self = this;
            require(["viewmodels/entity.lookup.filter.dialog", "durandal/app"], function (dialog, app2) {
                dialog.entity(self.Entity());
                dialog.selectedFilters(self.FilterCollection());
                app2.showDialog(dialog, self.Entity())
                    .done(function (result) {
                        if (!result) return;
                        if (result === "OK") {
                            self.FilterCollection(dialog.selectedFilters());
                        }
                    });

            });
        };

    return {
        editDisplayTemplate: editDisplayTemplate,
        editColumns: editColumns,
        editFilter: editFilter
    };
};
/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../SphApp/schemas/form.designer.g.js" />

bespoke.sph.domain.ButtonPartial = function () {

    var editCommand = function () {
        var self = this,
            w = window.open("/sph/editor/ace?mode=javascript", "_blank", "height=" + screen.height + ",width=" + screen.width + ",toolbar=0,location=0,fullscreen=yes"),
            wdw = w.window || w,
            init = function () {
                wdw.code = ko.unwrap(self.Command);
                if (!w.code) {
                    w.code = "//insert your code here";
                }
                wdw.saved = function (code, close) {
                    self.Command(code);
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
        editOperationSuccessCallback = function () {
            var self = this,
                w = window.open("/sph/editor/ace?mode=javascript", "_blank", "height=" + screen.height + ",width=" + screen.width + ",toolbar=0,location=0,fullscreen=yes"),
                wdw = w.window || w,
                init = function () {
                    wdw.code = ko.unwrap(self.OperationSuccessCallback);
                    if (!w.code) {
                        w.code = "//insert your code here";
                    }
                    wdw.saved = function (code, close) {
                        self.OperationSuccessCallback(code);
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
    editOperationFailureCallback = function () {
        var self = this,
            w = window.open("/sph/editor/ace?mode=javascript", "_blank", "height=" + screen.height + ",width=" + screen.width + ",toolbar=0,location=0,fullscreen=yes"),
            wdw = w.window || w,
            init = function () {
                wdw.code = ko.unwrap(self.OperationFailureCallback);
                if (!w.code) {
                    w.code = "//insert your code here";
                }
                wdw.saved = function (code, close) {
                    self.OperationFailureCallback(code);
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
        canSetSuccessCallback = ko.computed(function () {

        });
    return {
        editCommand: editCommand,
        editOperationSuccessCallback: editOperationSuccessCallback,
        editOperationFailureCallback: editOperationFailureCallback,
        canSetSuccessCallback: canSetSuccessCallback
    };
};
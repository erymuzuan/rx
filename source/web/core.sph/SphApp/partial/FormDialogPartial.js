/// <reference path="../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../schemas/form.designer.g.js" />

bespoke.sph.domain.FormDialogPartial = function (model) {

    var system = require("durandal/system"),
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

            }),
        addDialogButton = function () {
                model.DialogButtonCollection.push(new bespoke.sph.domain.DialogButton(system.guid()));
        },
        removeDialogButton = function(btn) {
            model.DialogButtonCollection.remove(btn);
        };
    return {
        removeDialogButton: removeDialogButton,
        addDialogButton: addDialogButton
    };
};
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />


define(["services/datacontext", "services/logger", "plugins/dialog"],
    function (context, logger, dialog) {

        var rule = ko.observable(new bespoke.sph.domain.BusinessRule()),
            activate = function () {
                rule().Name.subscribe(function(n) {
                    if (!rule().Description()) {
                        rule().Description(n);
                    }
                    if (!rule().ErrorMessage()) {
                        rule().ErrorMessage(n);
                    }
                });
            },
            attached = function (view) {
                setTimeout(function () { $(view).find("#Name").focus(); }, 500);
            },
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };

        var vm = {
            activate: activate,
            attached : attached,
            rule: rule,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });

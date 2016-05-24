/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../../../core.sph/SphApp/schemas/__domain.js" />
/// <reference path="../../../core.sph/SphApp/objectbuilders.js" />
/// <reference path="../../../core.sph/Scripts/_task.js" />
/// <reference path="../../../core.sph/Scripts/require.js" />


define(["plugins/dialog", objectbuilders.datacontext, objectbuilders.system],
    function (dialog, context, system) {

        var dtd = ko.observable(),
            id = ko.observable(),
            activate = function () {
                var dlg = new bespoke.sph.domain.DataTransferDefinition({ "WebId": system.guid(), "IgnoreMessaging": true, "BatchSize": 40 });
                dlg.IgnoreMessaging(true);
                dtd(dlg);
                return true;
            },
            okClick = function (data, ev) {
                if (!bespoke.utils.form.checkValidity(ev.target)) {
                    return Task.fromResult(0);
                }

                var json = ko.toJSON(dtd);
                return context.post(json, "/api/data-imports")
                    .then(function (result) {
                        if (result) {
                            id(result.id);
                            dialog.close(data, "OK");
                        }
                    });

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            },
            attached = function(view) {
                setTimeout(function() {
                    $(view).find("#name-input").focus();
                }, 500);
            };

        var vm = {
            dtd: dtd,
            activate: activate,
            attached: attached,
            okClick: okClick,
            id: id,
            cancelClick: cancelClick
        };


        return vm;

    });

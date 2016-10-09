/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/rx.management.g.js" />


define(["plugins/dialog"],
    function (dialog) {

        const webServer = ko.observable(new bespoke.sph.domain.WebServerConfig()),
            computers = ko.observableArray(),
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };

        const vm = {
            webServer: webServer,
            okClick: okClick,
            cancelClick: cancelClick,
            computers: computers
        };


        return vm;

    });

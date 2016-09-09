/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />


define(["plugins/dialog"],
    function (dialog) {

        const field = ko.observable(new bespoke.sph.domain.HeaderFieldMapping()),
            port = ko.observable(),
            headerOptions = ko.observableArray(["Name", "FileName", "ContentType", "Length", "LastWriteTime", "CreationTime", "Rx:ApplicationName", "Rx:LocationName", "Rx:MachineName"]),
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }

            },
                cancelClick = function () {
                    dialog.close(this, "Cancel");
                };

        const vm = {
            headerOptions: headerOptions,
            field: field,
            port: port,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });

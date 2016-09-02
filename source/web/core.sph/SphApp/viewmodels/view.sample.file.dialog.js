/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />


define(["plugins/dialog"],
    function (dialog) {

        const port = ko.observable(new bespoke.sph.domain.ReceivePort()),
            text = ko.observable(),
            activate = function () {
                return $.get(`/receive-ports/${port().Id()}/text`)
                .done(text);
            },
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };

        const vm = {
            activate: activate,
            text: text,
            port: port,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });

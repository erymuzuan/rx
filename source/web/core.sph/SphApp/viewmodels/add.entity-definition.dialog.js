/// <reference path="../../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(['plugins/dialog', objectbuilders.datacontext],
    function (dialog, context) {

        var ed = ko.observable(new bespoke.sph.domain.EntityDefinition()),
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                                    dialog.close(this, "OK");
                }
            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };

        var vm = {
            ed: ed,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });

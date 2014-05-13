/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />

define(['plugins/dialog'],
    function (dialog) {

        var entity = ko.observable(),
            field = ko.observable(new bespoke.sph.domain.DocumentField()),
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }
            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            },
            attached = function () {
                var model = field();
                model.Path.subscribe(function (v) {
                    if (!ko.unwrap(model.Name)) {
                        model.Name(v);
                    }
                });
            };

        var vm = {
            attached: attached,
            entity: entity,
            field: field,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });

/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../schemas/form.designer.g.js" />

define(['plugins/dialog'],
    function (dialog) {

        var field = ko.observable(new bespoke.sph.domain.PropertyChangedField()),
            okClick = function (data, ev) {
            if (bespoke.utils.form.checkValidity(ev.target)) {
                dialog.close(this, "OK");
            }

        },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            },
            attached = function (view) {
                var model = field();
                model.Path.subscribe(function (v) {
                    if (!ko.unwrap(model.Name)) {
                        model.Name(v);
                    }
                });
                setTimeout(function () {
                    $(view).find('#changed-field-path').focus();
                }, 100);
            };

        var vm = {
            field: field,
            okClick: okClick,
            attached: attached,
            cancelClick: cancelClick
        };


        return vm;

    });

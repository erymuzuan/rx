/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />

define(['plugins/dialog', objectbuilders.system],
    function (dialog, system) {

        var okClick = function (data, ev) {
            if (bespoke.utils.form.checkValidity(ev.target)) {
                dialog.close(this, "OK");
            }

        },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            },
            attached = function(view) {
                setTimeout(function() {
                    $(view).find('#constant-field-value').focus();
                }, 100);
            };

        var vm = {
            field: ko.observable(new bespoke.sph.domain.ConstantField(system.guid())),
            okClick: okClick,
            attached: attached,
            cancelClick: cancelClick
        };


        return vm;

    });

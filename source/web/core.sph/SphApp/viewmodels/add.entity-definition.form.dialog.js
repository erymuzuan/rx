/// <reference path="../../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />
/// <reference path="../schemas/form.designer.g.js" />


define(["plugins/dialog"],
    function (dialog) {

        var form = ko.observable(new bespoke.sph.domain.EntityForm()),
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }
            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            },
            formValid = ko.computed(function () {
                return form().Route() && form().Name();
            }),
            activate = function () {
                form().Name.subscribe(function (v) {
                    form().Route(v.toLowerCase().replace(/\W+/g, "-"));
                });
            };

        var vm = {
            form: form,
            okClick: okClick,
            cancelClick: cancelClick,
            activate: activate,
            formValid: formValid
        };


        return vm;

    });

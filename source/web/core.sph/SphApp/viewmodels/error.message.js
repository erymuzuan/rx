/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />

define(['plugins/dialog'],
    function (dialog) {

        var okClick = function (data, ev) {
            if (bespoke.utils.form.checkValidity(ev.target)) {
                dialog.close(this, "OK");
            }

        };
        var vm = {
            validationErrors: ko.observableArray(),
            okClick: okClick
        };


        return vm;

    });

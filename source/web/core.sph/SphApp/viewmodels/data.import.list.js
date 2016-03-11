/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(["plugins/dialog"],
    function (dialog) {

        var model = ko.observable(),
            list = ko.observableArray(),
            activate = function () {
                $.get("/api/data-imports").done(list);
            },
            attached = function (view) {
                var checkboxes = $(view).find("i.fa-check");
                checkboxes.hide();
                $(view).on("click", "tr", function () {
                    checkboxes.hide();
                    var m = ko.dataFor(this);
                    model(m);
                    $(this).find("i.fa-check").show();
                });
                $(view).on("click", "div.modal-footer>a", function () {
                    $("div.modal-backdrop").remove();
                });
            },
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }
            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };

        var vm = {
            model: model,
            list: list,
            activate: activate,
            attached: attached,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });

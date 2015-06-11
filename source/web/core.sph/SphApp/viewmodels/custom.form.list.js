/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../../Scripts/_task.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define([],
    function () {

        var isBusy = ko.observable(false),
            activate = function () {
                return true;
            },
            attached = function (view) {
            },
            importCommand = function () {
                require(["viewmodels/custom.form.import.dialog", "durandal/app"], function (dialog, app2) {
                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {


                            }
                        });
                });

                return Task.fromResult(true);
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            toolbar: {
                importCommand: importCommand
            }
        };

        return vm;

    });

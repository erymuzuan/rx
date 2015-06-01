/// <reference path="../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/__domain.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(["services/datacontext", "services/logger"],
    function (context, logger) {

        var entities = ko.observableArray(),
            isBusy = ko.observable(false),
            activate = function () {

            },
            attached = function (view) {


            },
            uploadPackage = function () {
                return require(["viewmodels/entity.import.dialog", "durandal/app"], function (dialog, app2) {
                    app2.showDialog(dialog)
                        .done(function () {
                            var ent = dialog.entity();
                            if (ent) {
                                entities.push(ent);
                            }
                        });
                });


            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            entities: entities,
            toolbar: {
                importCommand: uploadPackage,
                addNew: {
                    location: "#/entity.details/0",
                    caption: "Add New Custom Entity"
                }
            }
        };

        return vm;

    });

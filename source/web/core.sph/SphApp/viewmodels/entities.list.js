/// <reference path="../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(["services/datacontext", "services/logger"],
    function (context, logger) {

        var
            entities = ko.observableArray(),
            isBusy = ko.observable(false),
            activate = function () {

            },
            attached = function (view) {

            
            },
            uploadPackage = function () {
                var tcs = new $.Deferred();
                require(["viewmodels/entity.import.dialog", "durandal/app"], function (dialog, app2) {
                    //dialog.entity(entity());

                    app2.showDialog(dialog)
                        .done(tcs.resolve);
                });

                return tcs.promise();

            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            entities: entities,
            toolbar: {
                importCommand : uploadPackage,
                addNew: {
                    location: "#/entity.details/0",
                    caption: "Add New Custom Entity"
                }
            }
        };

        return vm;

    });

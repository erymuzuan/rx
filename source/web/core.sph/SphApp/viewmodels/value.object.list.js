/// <reference path="../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/__domain.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(["services/datacontext", "services/logger", "services/new-item"],
    function (context, logger, addItemService) {

        var entities = ko.observableArray(),
            isBusy = ko.observable(false);

        var vm = {
            isBusy: isBusy,
            entities: entities,
            toolbar: {
                addNewCommand: function () {
                    return addItemService.addValueObjectDefinitionAsync();
                }
            }
        };

        return vm;

    });

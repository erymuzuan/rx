/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/__domain.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(["services/datacontext", "services/logger", "services/new-item"],
    function (context, logger, addItemService) {

        const entities = ko.observableArray(),
            isBusy = ko.observable(false);

        const vm = {
            isBusy: isBusy,
            entities: entities,
            toolbar: {
                addNewCommand: function () {
                    return addItemService.addValueObjectDefinitionAsync();
                },
                commands: ko.observableArray()
            }
        };

        return vm;

    });

/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(["services/new-item", "services/datacontext", objectbuilders.logger], function (addItemService, context, logger) {

    const list = ko.observableArray(),
        errors = ko.observableArray(),
        portOptions = ko.observableArray(),
        options = ko.observableArray(),
        isBusy = ko.observable(false),
        activate = function () {
            return context.loadAsync({ entity:"ReceivePort" , size:40})
                .then(function(lo) {
                    portOptions(lo.itemCollection);
                    return context.get("receive-locations/installed");
                }).then(options);
      
        },
            getDesigner = function ($type) {
                const item = _(options()).find(v => ko.unwrap(v.location.$type) === ko.unwrap($type));
                if (item.designer) {
                    return item.designer;
                }
                return {};
            },
        map = function (loc) {
            const designer = getDesigner(loc.$type),
                port = _(portOptions()).find(x => ko.unwrap(x.Id) === ko.unwrap(loc.ReceivePort)),
                started = ko.observable(true);
            loc.portName = port.Name;
            loc.entity = port.Entity;
            loc.designer = designer;
            loc.started = started;

            context.get(`/receive-locations/${ko.unwrap(loc.Id)}/status`).done(started);

            return loc;
        },
        edit = function (location) {
            const designer = getDesigner(location.$type),
                port = _(portOptions()).find(x => ko.unwrap(x.Id) === ko.unwrap(location.ReceivePort));

            require([`viewmodels/${designer.Name}.receive.location.dialog`, "durandal/app"],
                function (dialog, app2) {
                    const clone = ko.mapping.fromJSON(ko.mapping.toJSON(location));
                    dialog.location(clone);
                    dialog.port(port);
                    app2.showDialog(dialog)
                        .done(function(result) {
                            if (!result) return;
                            if (result === "OK") {
                                list.replace(location, map(dialog.location()));
                                isBusy(true);
                                context.post(ko.mapping.toJSON(dialog.location()), "/receive-locations")
                                    .done(() => isBusy(false));
                            }
                        });
                });
            console.log(location);
        },
        start = function (location) {
            const data = ko.mapping.toJSON(location);
            isBusy(true);

            return context.post(data, `/receive-locations/${ko.unwrap(location.Id)}/start`)
                .then(function (result) {

                    isBusy(false);
                    if (result.success) {
                        logger.info(result.message);
                        errors.removeAll();
                    } else {

                        errors(result.Errors);
                        logger.error("There are errors when starting your location, !!!");
                    }
                });
            
        },
        stop = function (location) {
            const data = ko.mapping.toJSON(location);
            isBusy(true);

            return context.post(data, `/receive-locations/${ko.unwrap(location.Id)}/stop`)
                .then(function (result) {

                    isBusy(false);
                    if (result.success) {
                        logger.info(result.message);
                        errors.removeAll();
                    } else {

                        errors(result.Errors);
                        logger.error("There are errors when stopping your location, !!!");
                    }
                });
            
        },
        publish = function(location) {
            const data = ko.mapping.toJSON(location);
            isBusy(true);

            return context.post(data, `/receive-locations/${ko.unwrap(location.Id)}/publish`)
                .then(function (result) {

                    isBusy(false);
                    if (result.success) {
                        logger.info(result.message);
                        errors.removeAll();
                    } else {

                        errors(result.Errors);
                        logger.error("There are errors in your receive location, !!!");
                    }
                });
        };

    const vm = {
        activate: activate,
        errors: errors,
        isBusy: isBusy,
        list: list,
        edit: edit,
        start: start,
        stop: stop,
        publish: publish,
        map: map,
        getDesigner: getDesigner,
        toolbar: {
            addNewCommand: addItemService.addReceiveLocation
        }
    };

    return vm;
});

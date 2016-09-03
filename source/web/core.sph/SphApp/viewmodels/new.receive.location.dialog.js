/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="~/Scripts/_task.js" />
define(["plugins/dialog", objectbuilders.datacontext],
    function (dialog, context) {

        const locationOptions = ko.observableArray(),
            name = ko.observable(),
            port = ko.observable(),
            id = ko.observable(),
            selectedItem = ko.observable(),
            getReceiveLocationType = function (location) {
                return /^.*?,(.*?).receive.location/.exec(ko.unwrap(location.$type))[1].trim();
            },
            isBusy = ko.observable(false),
            locations = ko.observableArray(),
            receivePortOptions = ko.observableArray(),
            activate = function () {
                name("");
                port("");
                selectedItem({
                    location: {
                        Name: ko.observable()
                    },
                    designer: {
                        Name: ko.observable("Select your receive location type"),
                        BootstrapIcon: ko.observable(),
                        PngIcon: ko.observable(),
                        FontAwesomeIcon: ko.observable("plus")
                    }
                });
                var tcs = new $.Deferred();
                context.getTuplesAsync("ReceivePort", "Id ne '0'", "Id", "Name")
                    .done(receivePortOptions);
                $.get("receive-locations/installed", function (d) {
                    locationOptions(d);
                    tcs.resolve(true);
                });
                return tcs.promise();
            },
            getDesigner = function ($type) {
                const item = _(locationOptions()).find(v => ko.unwrap(v.location.$type) === ko.unwrap($type));
                return item.designer || {};

            },
            url = ko.observable(),
            attached = function () {
                setTimeout(function () {
                    $("#name-input").focus();
                }, 500);

            },

            okClick = function (data, ev) {
                if (!bespoke.utils.form.checkValidity(ev.target)) {
                    return Task.fromResult(0);
                }
                if (!selectedItem()) return Task.fromResult(0);

                const location = selectedItem().location;
                location.Name = ko.unwrap(name);
                location.ReceivePort = ko.unwrap(port);
                const json = ko.mapping.toJSON(location);

                return context.post(json, "/receive-locations")
                    .then(function (result) {
                        id(result.id);
                        url(selectedItem().designer.Route.replace("/:id", `/${result.id}`));
                        dialog.close(data, "OK");

                    });
            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            },
            selectLocation = function (item) {
                selectedItem(item);
            };

        const vm = {
            name: name,
            port: port,
            receivePortOptions: receivePortOptions,
            locationOptions: locationOptions,
            getLocationType: getReceiveLocationType,
            isBusy: isBusy,
            locations: locations,
            getDesigner: getDesigner,
            id: id,
            url: url,
            attached: attached,
            activate: activate,
            okClick: okClick,
            cancelClick: cancelClick,
            selectedItem: selectedItem,
            selectLocation : selectLocation
        };


        return vm;

    });

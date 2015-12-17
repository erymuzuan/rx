/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="~/Scripts/_task.js" />
define(["plugins/dialog", objectbuilders.datacontext],
    function (dialog, context) {

        var adapterOptions = ko.observableArray(),
            name = ko.observable(),
            selectedItem = ko.observable(),
            getAdapterType = function (adapter) {

                return /^.*?,(.*?).adapter/.exec(ko.unwrap(adapter.$type))[1].trim();
            },
            isBusy = ko.observable(false),
            adapters = ko.observableArray(),
            activate = function () {
                name("");
                selectedItem({
                    adapter: {
                        Name: ko.observable()
                    },
                    designer: {
                        Name: ko.observable("Select your adapter type"),
                        BootstrapIcon: ko.observable(),
                        PngIcon: ko.observable(),
                        FontAwesomeIcon: ko.observable("plus")
                    }
                });
                var tcs = new $.Deferred();
                $.get("adapter/installed-adapters", function (d) {
                    adapterOptions(d);
                    tcs.resolve(true);
                });
                return tcs.promise();
            },
            getDesigner = function ($type) {
                var item = _(adapterOptions()).find(function (v) {
                    return ko.unwrap(v.adapter.$type) === ko.unwrap($type);
                });

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

                var adapter = selectedItem().adapter;
                adapter.Name = ko.unwrap(name);

                var json = ko.mapping.toJSON(adapter);

                return context.post(json, "/adapter")
                    .then(function (result) {
                        url(selectedItem().designer.Route.replace("/0", "/" + result.id));
                        dialog.close(data, "OK");

                    });
            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            },
            selectAdapter = function (item) {
                selectedItem(item);
            };

        var vm = {
            name: name,
            adapterOptions: adapterOptions,
            getAdapterType: getAdapterType,
            isBusy: isBusy,
            adapters: adapters,
            getDesigner: getDesigner,
            url: url,
            attached: attached,
            activate: activate,
            okClick: okClick,
            cancelClick: cancelClick,
            selectedItem: selectedItem,
            selectAdapter: selectAdapter
        };


        return vm;

    });

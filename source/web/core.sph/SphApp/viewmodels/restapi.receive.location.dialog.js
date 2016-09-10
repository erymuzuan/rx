/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../partial/__partial.js" />


define(["plugins/dialog", "services/datacontext"],
    function (dialog, context) {

        const location = ko.observable(new bespoke.sph.domain.Adapters.RestApiReceiveLocation({ BaseAddress: "" })),
            isBusy = ko.observable(false),
            port = ko.observable(new bespoke.sph.domain.ReceivePort()),
            endpointOptions = ko.observableArray(),
            contentTypeOptions = ko.observableArray(["text/plain", "text/html", "application/json", "text/xml"]),
            activate = function () {
                isBusy(true);
            
                return context.loadAsync("OperationEndpoint", `Entity eq '${ko.unwrap(port().Entity)}'`)
                    .done(lo => endpointOptions(lo.itemCollection));
                
            },
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }
            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };

        const vm = {
            isBusy: isBusy,
            port: port,
            activate: activate,
            location: location,
            contentTypeOptions: contentTypeOptions,
            endpointOptions: endpointOptions,
            okClick: okClick,
            cancelClick: cancelClick
        };

        return vm;

    });

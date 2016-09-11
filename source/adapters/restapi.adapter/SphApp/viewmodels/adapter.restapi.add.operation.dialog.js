/// <reference path="~/Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="~/Scripts/knockout-3.4.0.debug.js" />
/// <reference path="~/Scripts/require.js" />
/// <reference path="~/Scripts/underscore.js" />
/// <reference path="~/Scripts/adapter.api.g.js" />
/// <reference path="~/SphApp/schemas/adapter.restapi.operation.js" />
/// <reference path="~/SphApp/schemas/adapter.restapi.receive.location.js" />


define(["plugins/dialog"],
    function(dialog) {

        const operation = ko.observable(new bespoke.sph.domain.api.RestApiOperationDefinition()),
            adapter = ko.observable(),
            okClick = function(data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }

            },
            cancelClick = function() {
                dialog.close(this, "Cancel");
            };

        const vm = {
            operation: operation,
            adapter: adapter,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });

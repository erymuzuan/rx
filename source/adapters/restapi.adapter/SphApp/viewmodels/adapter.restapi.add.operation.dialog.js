/// <reference path="~/Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="~/Scripts/knockout-3.4.0.debug.js" />
/// <reference path="~/Scripts/require.js" />
/// <reference path="~/Scripts/underscore.js" />
/// <reference path="~/Scripts/objectbuilders.js" />
/// <reference path="~/Scripts/adapter.api.g.js" />
/// <reference path="~/SphApp/schemas/adapter.restapi.operation.js" />
/// <reference path="~/SphApp/schemas/adapter.restapi.receive.location.js" />


define(["plugins/dialog", objectbuilders.datacontext],
    function(dialog, context) {

        const options = ko.observableArray(),
            selectedOptions = ko.observableArray(),
            busy = ko.observable(false),
            storeId = ko.observable(),
            adapter = ko.observable(),
            activate = function() {
                selectedOptions([]);
            },
            okClick = function(data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }

            },
            cancelClick = function() {
                dialog.close(this, "Cancel");
            },
            attached = function (view) {
                $(view).on("click", "input[type=checkbox].selector", function () {
                    const ep = ko.dataFor(this);
                    busy(true);
                    if ($(this).is(":checked")) {
                        context.post(ko.mapping.toJSON(ep), `/restapi-adapters/endpoints/${ko.unwrap(ep.Name)}/build`)
                            .done(function(result) {
                                const built = new bespoke.sph.domain.api.RestApiOperationDefinition(result);
                                selectedOptions.push(built);
                                busy(false);
                            });
                    } else {
                        selectedOptions.remove(ep);
                    }
                    
                });
            };

        storeId.subscribe(function (id) {
            if (!id) {
                busy(false);
                selectedOptions.removeAll();
                return;
            }
            busy(true);
            context.get(`/restapi-adapters/hars/${id}/endpoints`).done(function(list){
                const maps = list.map(v => new bespoke.sph.domain.api.RestApiOperationDefinition(v));
                options(maps);
                selectedOptions.removeAll();
                busy(false);
            });
        });

        const vm = {
            busy: busy,
            options: options,
            selectedOptions: selectedOptions,
            activate: activate,
            attached: attached,
            storeId: storeId,
            adapter: adapter,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });

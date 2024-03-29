﻿/// <reference path="~/Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="~/Scripts/knockout-3.4.0.debug.js" />
/// <reference path="~/Scripts/require.js" />
/// <reference path="~/Scripts/underscore.js" />
/// <reference path="~/Scripts/objectbuilders.js" />
/// <reference path="~/Scripts/adapter.api.g.js" />
/// <reference path="~/SphApp/schemas/adapter.restapi.operation.js" />
/// <reference path="~/SphApp/schemas/adapter.restapi.receive.location.js" />


define(["plugins/dialog", objectbuilders.datacontext, objectbuilders.logger],
    function (dialog, context, logger) {

        const options = ko.observableArray(),
            selectedOptions = ko.observableArray(),
            busy = ko.observable(false),
            storeId = ko.observable(),
            adapter = ko.observable(),
            activate = function () {
                selectedOptions([]);
            },
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }
            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            },
            attached = function (view) {
                $(view).on("click", "input[type=checkbox].selector", function () {
                    const ep = ko.dataFor(this);
                    busy(true);
                    if ($(this).is(":checked")) {
                        selectedOptions.push(ep);
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
            context.get(`/restapi-adapters/hars/${id}/endpoints`).done(function (list) {
                const maps = list.map(v => new bespoke.sph.domain.api.RestApiOperationDefinition(v));
                // change the method name to the Operation name
                maps.forEach(v => {
                    v.Name.subscribe(function (name) {
                        if (name) {
                            v.MethodName(name);
                        }
                    });
                });
                options(maps);
                selectedOptions.removeAll();
                busy(false);
            })
                .fail(function (e, x, st) {

                    logger.error(st);
                    const errors = e.responseJSON;
                    _(errors).each(v => logger.error(v.Message, v.PropertyName));
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

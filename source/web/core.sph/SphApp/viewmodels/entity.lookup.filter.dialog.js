/// <reference path="../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(["services/datacontext", "services/logger", "plugins/dialog"],
    function (context, logger, dialog) {

        var filters = ko.observableArray(),
            selectedFilters = ko.observableArray(),
            entity = ko.observable(),
            activate = function (ent) {
                var tcs = new $.Deferred();

                $.get("/entity-definition/variable-path/" + ko.unwrap(ent))
                    .done(function (list) {
                        filters(list);
                        tcs.resolve(true);
                    });
                return tcs.promise();
            },
            attached = function (view) {
                $(view).on("click", "input[type=checkbox]", function (e) {
                    if ($(this).is(":checked")) {
                        selectedFilters.push($(this).val());
                    } else {
                        selectedFilters.remove($(this).val());
                    }
                });

                _(ko.unwrap(selectedFilters)).each(function (v) {
                    $("#mbs-dialog-form input[type=checkbox]").each(function (i,c) {
                        if (c.value === v) {
                            $(c).prop("checked", true);
                        }
                    });
                });
            },
            okClick = function (data, ev) {
                dialog.close(this, "OK");

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };

        var vm = {
            activate: activate,
            attached: attached,
            entity: entity,
            selectedFilters: selectedFilters,
            members: filters,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });

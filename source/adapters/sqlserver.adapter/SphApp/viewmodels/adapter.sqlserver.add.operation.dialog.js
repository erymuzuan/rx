/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />

/**
 * @param{{ Database:function, Server:function,TrustedConnection:function, UserId:function,Password:function,ColumnDisplayNameStrategy:function}} adapter
 * @param{{toObservable:function}} context
 *
 */

define(["plugins/dialog", "services/datacontext", "knockout", "underscore"],
    function (dialog, context, ko, _) {

        const operations = ko.observableArray(),
            isBusy = ko.observable(false),
            schema = ko.observable(),
            name = ko.observable(),
            adapter = ko.observable(),
            selectedOperations = ko.observableArray(),
            getType = function (op) {

                var typeName = ko.unwrap(op.$type),
                    type = "";
                switch (typeName) {
                    case "Bespoke.Sph.Integrations.Adapters.TableValuedFunction, sqlserver.adapter":
                        type = "table-valued-functions";
                        break;
                    case "Bespoke.Sph.Integrations.Adapters.ScalarValuedFunction, sqlserver.adapter":
                        type = "scalar-valued-functions";
                        break;
                    default:
                        type = "sprocs";
                        break;
                }

                return type;
            },
            activate = function () {

                selectedOperations([]);
                const adp = ko.unwrap(adapter),
                    server = ko.unwrap(adp.Server),
                    database = ko.unwrap(adp.Database),
                    trusted = ko.unwrap(adp.TrustedConnection),
                    userid = ko.unwrap(adp.UserId),
                    password = ko.unwrap(adp.Password),
                    url = trusted ? "" : `&trusted=false&userid=${userid}&password=${password}`;
                return $.getJSON(`/sqlserver-adapter/operation-options?server=${server}&database=${database}${url}`)
                .done(function (result) {
                    const list = _(result).filter(function (v) {
                        const f = _(adapter().OperationDefinitionCollection())
                            .find(function (x) {
                                return ko.unwrap(x.Name) === ko.unwrap(v.Name) && ko.unwrap(v.Schema) === ko.unwrap(x.Schema);

                            });
                        return !f;
                    });
                    operations(list);

                });
            },
            attached = function (view) {

                const $table = $(view).find("table#table-options-panel"),
                    $bodyCells = $table.find("tbody tr:first").children();

                // Get the tbody columns width array
                var colWidth = $bodyCells.map(function () {
                    return $(this).width();
                }).get();

                // Set the width of thead columns
                $table.find("thead tr").children().each(function (i, v) {
                    $(v).width(colWidth[i]);
                });
                setTimeout(() =>
                    $(view).find("input.search-query").focus(),
                    500);
                $(view).on("click", "input[type=checkbox].operation-checkbox", function (e) {
                    const operation = ko.dataFor(this),
                        adp = ko.unwrap(adapter),
                        server = ko.unwrap(adp.Server),
                        database = ko.unwrap(adp.Database),
                        trusted = ko.unwrap(adp.TrustedConnection),
                        userid = ko.unwrap(adp.UserId),
                        password = ko.unwrap(adp.Password),
                        strategy = ko.unwrap(adp.ColumnDisplayNameStrategy),
                        clr = ko.unwrap(adp.ClrNameStrategy),
                        url = trusted ? "" : `&trusted=false&userid=${userid}&password=${password}`,
                        type = ko.unwrap(operation.ObjectType);


                    if ($(this).is(":checked")) {
                        isBusy();
                        $.getJSON(`/sqlserver-adapter/operation-options/${type}/${operation.Schema}/${operation.Name}?server=${server}&database=${database}&strategy=${strategy}&clr=${clr}${url}`)
                                .done(function (result) {
                                    const tr = context.toObservable(result);
                                    selectedOperations.push(tr);
                                    isBusy(false);
                                });

                    } else {
                        const tr = _(selectedOperations()).find(function (v) {
                            return ko.unwrap(v.Name) === operation.Name && ko.unwrap(v.Schema) === operation.Schema;
                        });
                        if (tr)
                            selectedOperations.remove(tr);
                    }
                });
            },
            okClick = function (data, ev) {
                dialog.close(this, "OK");
            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };

        const vm = {
            activate: activate,
            attached: attached,
            isBusy: isBusy,
            getType: getType,
            operations: operations,
            selectedOperations: selectedOperations,
            adapter: adapter,
            name: name,
            schema: schema,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });

/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(["plugins/dialog", "services/datacontext", "knockout", "jquery"],
    function (dialog, context, ko, $) {

        var tables = ko.observableArray(),
            isBusy = ko.observable(false),
            schema = ko.observable(),
            name = ko.observable(),
            adapter = ko.observable(),
            selectedTables = ko.observableArray(),
            activate = function () {

                selectedTables([]);
                var adp = ko.unwrap(adapter),
                    server = ko.unwrap(adp.Server),
                    database = ko.unwrap(adp.Database),
                    trusted = ko.unwrap(adp.TrustedConnection),
                    userid = ko.unwrap(adp.UserId),
                    password = ko.unwrap(adp.Password),
                    url = trusted ? "" : "&trusted=false&userid=" + userid + "&password=" + password;
                return $.getJSON("/sqlserver-adapter/table-options?server=" + server + "&database=" + database + url)
                    .done(function (result) {
                        var list = _(result).filter(function (v) {
                            var f = _(adapter().TableDefinitionCollection())
                                .find(function (x) {
                                    return ko.unwrap(x.Name) === ko.unwrap(v.Name) && ko.unwrap(v.Schema) === ko.unwrap(x.Schema);

                                });
                            return !f;
                        });
                        tables(list);

                    });
            };
        const attached = function (view) {

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
            setTimeout(function() {
                $(view).find("input.search-query").focus().select();
            }, 500);
            $(view).on("click", "input[type=checkbox].table-checkbox", function (e) {
                var table = ko.dataFor(this);

                const adp = ko.toJS(adapter),
                    server = ko.unwrap(adp.Server),
                    database = ko.unwrap(adp.Database),
                    trusted = ko.unwrap(adp.TrustedConnection),
                    userid = ko.unwrap(adp.UserId),
                    password = ko.unwrap(adp.Password),
                    strategy = ko.unwrap(adp.ColumnDisplayNameStrategy),
                    clr = ko.unwrap(adp.ClrNameStrategy),
                    url = trusted ? "" : `&trusted=false&userid=${userid}&password=${password}`;
                if ($(this).is(":checked")) {
                    isBusy(true);
                    $.getJSON(`/sqlserver-adapter/table-options/${table.Schema}/${table.Name}/?server=${server}&database=${database}&strategy=${strategy}&clr=${clr}${url}`)
                        .done(function (result) {
                            const tr = context.toObservable(result);
                            tr.IsSelected(true);
                            selectedTables.push(tr);
                            isBusy(false);
                        });

                } else {
                    const tr = _(selectedTables()).find(function (v) {
                        return ko.unwrap(v.Name) === ko.unwrap(table.Name) && ko.unwrap(v.Schema) === ko.unwrap(table.Schema);
                    });
                    if (tr)
                        selectedTables.remove(tr);
                }
            });
        };
        var okClick = function (data, ev) {
                dialog.close(this, "OK");
            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };
        return {
            activate: activate,
            attached: attached,
            isBusy: isBusy,
            tables: tables,
            selectedTables: selectedTables,
            adapter: adapter,
            name: name,
            schema: schema,
            okClick: okClick,
            cancelClick: cancelClick
        };

    });

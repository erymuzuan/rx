/// <reference path="../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="~/Scripts/_task.js" />
define(["services/datacontext", "services/logger", "plugins/router"],
    function (context, logger, router) {

        var model = ko.observable({
            name: ko.observable(),
            adapter: ko.observable(),
            table: ko.observable(),
            batchSize: ko.observable(40),
            entity: ko.observable(),
            sql: ko.observable(),
            map: ko.observable()
        }),
            adapterOptions = ko.observableArray(),
            tableOptions = ko.observableArray(),
            entityOptions = ko.observableArray(),
            mapOptions = ko.observableArray(),
            previewResult = ko.observableArray(),
            isBusy = ko.observable(false),
            canPreview = ko.computed(function () {
                return model().adapter()
                    && model().table()
                    && model().sql()
                    && model().batchSize();
            }),
            canImport = ko.computed(function () {
                return canPreview() && model().map() && model().entity();
            }),
            activate = function () {
                context.getTuplesAsync("EntityDefinition", "Id ne '0'", "Id", "Name")
                    .done(entityOptions);
                return context.getTuplesAsync("Adapter", "Id ne '0'", "Id", "Name")
                    .done(adapterOptions);
            },
            modelChanged = function () {
                var cloned = ko.toJS(model);

                model().adapter.subscribe(function (a) {
                    if (!a) {
                        return;
                    }
                    isBusy(true);
                    context.loadOneAsync("Adapter", String.format("Id eq '{0}'", a))
                    .done(function (adp) {
                        tableOptions(adp.TableDefinitionCollection().map(ko.mapping.toJS));
                        isBusy(false);

                        model().table(cloned.table);
                    });
                });

                var filterMapOptions = function (list) {
                    var filtered = _(list).filter(function(v) {
                        return v.InputTypeName.indexOf(model().table()) > -1 && v.OutputTypeName.indexOf(model().entity()) > -1;
                    });
                    mapOptions(filtered);
                };
                model().table.subscribe(function (a) {
                    model().sql(String.format("select * from {0} ", a));
                    if (!(a && model().entity())) {
                        return Task.fromResult(0);
                    }
                    return context.getTuplesAsync("TransformDefinition", "Id ne '0'", "Id", "Name", "InputTypeName", "OutputTypeName")
                    .done(filterMapOptions);
                });
                model().entity.subscribe(function (a) {
                    if (!(a && model().entity())) {
                        return Task.fromResult(0);
                    }
                    return context.getTuplesAsync("TransformDefinition", "Id ne '0'", "Id", "Name", "InputTypeName", "OutputTypeName")
                    .done(filterMapOptions)
                    .done(function () {
                        model().map(cloned.map);
                    });
                });

                if (cloned.table) {
                    model().adapter("");
                    model().adapter(cloned.adapter);
                    model().table(cloned.table);
                }
                if (cloned.map) {
                    model().entity("");
                    model().entity(cloned.entity);
                    model().map(cloned.map);
                }
            },
            attached = function (view) {
                modelChanged(view);
            },
            preview = function () {
                return context.post(ko.mapping.toJSON(model), "data-import/preview")
                     .done(function (lo) {
                         var table = _(tableOptions()).find(function (v) { return v.Name === model().table(); });
                         var thead = "<tr>";
                         _(ko.unwrap(table.MemberCollection)).each(function (v) {
                             thead += "<th>" + v.Name + "</th>";
                         });
                         thead += "</tr>";
                         $("#thead").html(thead);

                         var tbody = "";
                         _(lo.ItemCollection).each(function (m) {
                             tbody += "<tr>";
                             _(ko.unwrap(table.MemberCollection)).each(function (v) {
                                 tbody += "  <td>" + m[v.Name] + "</td>\r";
                             });
                             tbody += "</tr>\r";
                         });
                         $("#tbody").html(tbody);

                         console.log(lo);
                         $("#preview-panel").modal("show");
                     });
            },
            importData = function () {
                return context.post(ko.mapping.toJSON(model), "data-import")
                     .done(function (result) {
                         logger.info(result.message);
                     });
            },
            save = function () {
                var data = ko.toJSON(model);
                return context.post(data, "/data-import/save");
            },
            removeAsync = function () {
                var data = ko.toJSON(model);
                return context.send("DELETE", data, "/data-import");
            },
            openSnippet = function () {
                var tcs = new $.Deferred();

                require(["viewmodels/data.import.list", "durandal/app"], function (dialog, app2) {
                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (result === "OK") {
                                model(ko.mapping.fromJS(dialog.model()));
                                modelChanged();
                            }
                            tcs.resolve(true);
                        });

                });


                return tcs.promise();

            };

        var vm = {
            canPreview: canPreview,
            canImport: canImport,
            entityOptions: entityOptions,
            previewResult: previewResult,
            tableOptions: tableOptions,
            adapterOptions: adapterOptions,
            mapOptions: mapOptions,
            preview: preview,
            importData: importData,
            model: model,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            toolbar: {
                saveCommand: save,
                removeCommand: removeAsync,
                openCommand: openSnippet
            }
        };

        return vm;

    });

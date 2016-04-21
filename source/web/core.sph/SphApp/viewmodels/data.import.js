/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />
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
                context.getTuplesAsync("EntityDefinition", null, "Id", "Name")
                    .done(entityOptions);
                return context.getTuplesAsync("Adapter", null, "Id", "Name")
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
                            var filtered = _(list).filter(function (v) {
                                return v.InputTypeName.indexOf(model().table()) > -1 && v.OutputTypeName.indexOf(model().entity()) > -1;
                            });
                            mapOptions(filtered);
                        },
                    loadMappings = function(lo) {
                            var tuples =  _(lo.itemCollection).map(function(v){
                                return {
                                    Id : ko.unwrap(v.Id),
                                    Name : ko.unwrap(v.Name),
                                    InputTypeName : ko.unwrap(v.InputTypeName),
                                    OutputTypeName : ko.unwrap(v.OutputTypeName)
                                }
                            });
                           filterMapOptions(tuples); 
                            
                        };
                model().table.subscribe(function (a) {
                    model().sql(String.format("select * from {0} ", a));
                    if (!(a && model().entity())) {
                        return Task.fromResult(0);
                    }
                    return context.loadAsync("TransformDefinition")
                    .done(loadMappings);
                });
                model().entity.subscribe(function (a) {
                    if (!(a && model().entity())) {
                        return Task.fromResult(0);
                    }
                    return context.loadAsync("TransformDefinition")
                    .done(loadMappings)
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
                $(view).on("click", "div.modal-footer>button", function () {
                    $("div.modal-backdrop").remove();
                });
            },
            preview = function () {
                var tcs = new $.Deferred();
                require(["viewmodels/data.import.result.preview.dialog", "durandal/app"],
                    function (dialog, app2) {
                        dialog.model(ko.unwrap(model));
                        dialog.tableOptions(ko.unwrap(tableOptions));
                        app2.showDialog(dialog).done(tcs.resolve);
                    });

                return tcs.promise();

            },
            importData = function () {
                return context.post(ko.mapping.toJSON(model), "/api/data-imports/" + ko.unwrap(model().Name) + "/execute")
                     .done(function (result) {
                         logger.info(result.message);
                     });
            },
            save = function () {
                var data = ko.toJSON(model);
                return context.post(data, "/api/data-imports");
            },
            removeAsync = function () {
                var data = ko.toJSON(model);
                return context.send("DELETE", data, "/api/data-imports/" + ko.unwrap(model().Name));
            },
            openSnippet = function () {
                var tcs = new $.Deferred();

                require(["viewmodels/data.import.list", "durandal/app"], function (dialog, app2) {
                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (result === "OK") {
                                isBusy(true);
                                
                                var md = model(),
                                    di = dialog.model();
                                md.adapter(di.adapter);
                                md.entity(di.entity);
                                md.sql(di.sql);
                                md.batchSize(di.batchSize);
                                md.name(di.name);


                                modelChanged();

                                setTimeout(function(){
                                    md.table(di.table);

                                    setTimeout(function(){
                                        md.map(di.map);
                                        tcs.resolve(true);
                                        isBusy(false);
                                    },800);

                                },500);
                            }else{
                                tcs.resolve(false);
                                isBusy(false);
                            }
                        });

                });


                return tcs.promise();

            };

        var vm = {
            canPreview: canPreview,
            canImport: canImport,
            entityOptions: entityOptions,
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

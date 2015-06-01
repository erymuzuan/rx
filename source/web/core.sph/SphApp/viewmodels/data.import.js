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

        var model = {
            adapter: ko.observable(),
            table: ko.observable(),
            batchSize: ko.observable(40),
            entity: ko.observable(),
            sql: ko.observable(),
            map: ko.observable()
        },
            adapterOptions = ko.observableArray(),
            tableOptions = ko.observableArray(),
            entityOptions = ko.observableArray(),
            mapOptions = ko.observableArray(),
            previewResult = ko.observableArray(),
            isBusy = ko.observable(false),
            canPreview = ko.computed(function() {
                return model.adapter()
                    && model.table()
                    && model.sql()
                    && model.batchSize();
            }),
            canImport = ko.computed(function() {
                return canPreview() && model.map() && model.entity();
            }),
            activate = function () {
                context.getTuplesAsync("EntityDefinition", "Id ne '0'", "Id", "Name")
                    .done(entityOptions);
                return context.getTuplesAsync("Adapter", "Id ne '0'", "Id", "Name")
                    .done(adapterOptions);
            },
            attached = function (view) {
                model.adapter.subscribe(function (a) {
                    isBusy(true);
                    context.loadOneAsync("Adapter", String.format("Id eq '{0}'", a))
                    .done(function (adp) {
                        tableOptions(ko.mapping.toJS(adp).TableDefinitionCollection);
                        isBusy(false);
                    });
                });
                model.table.subscribe(function (a) {
                    model.sql(String.format("select * from {0} ", a));
                    if (!(a && model.entity())) {
                        return Task.fromResult(0);
                    }
                    return context.getTuplesAsync("TransformDefinition", String.format("substringof('{0}', InputTypeName) and substringof('{1}', OutputTypeName)", a, model.entity()), "Id", "Name")
                    .done(mapOptions);
                });
                model.entity.subscribe(function (a) {
                    if (!(a && model.entity())) {
                        return Task.fromResult(0);
                    }
                    return context.getTuplesAsync("TransformDefinition", String.format("substringof('{0}', InputTypeName) and substringof('{1}', OutputTypeName)", model.table(), a), "Id", "Name")
                    .done(mapOptions);
                });
            },
            preview = function () {
                return context.post(ko.mapping.toJSON(model), "data-import/preview")
                     .done(function (lo) {
                         var table = _(tableOptions()).find(function (v) { return v.Name = model.table(); });
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
            attached: attached
        };

        return vm;

    });

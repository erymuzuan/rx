/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(["services/datacontext", "services/logger", objectbuilders.system],
    function (context, logger, system) {

        var template = ko.observable(new bespoke.sph.domain.DocumentTemplate(system.guid())),
            entityOptions = ko.observableArray(),
            errors = ko.observableArray(),
            isBusy = ko.observable(false),
            activate = function (id) {
                var query = String.format("IsPublished eq true"),
                    query1 = String.format("Id eq '{0}'", id),
                    entityTask = context.loadAsync("EntityDefinition", query),
                    templateTask = context.loadOneAsync("DocumentTemplate", query1),
                    tcs = new $.Deferred();


                $.when(entityTask, templateTask).then(function (lo, b) {
                    var types = _(lo.itemCollection).map(function (v) {
                        return v.Name();
                    });
                    entityOptions(types);
                    if (b) {
                        template(b);
                    } else {
                        template(new bespoke.sph.domain.DocumentTemplate(system.guid()));
                    }
                    tcs.resolve(true);
                });
                return tcs.promise();
            },
            attached = function (view) {

            },
            save = function () {
                var data = ko.mapping.toJSON(template);
                isBusy(true);

                return context.post(data, "/document-template")
                     .then(function (result) {
                         isBusy(false);
                         template().Id(result.id);
                     });
            },
            publishAsync = function () {
                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(template);
                isBusy(true);

                context.post(data, "/document-Template/publish")
                    .then(function (result) {
                        isBusy(false);
                        if (result.success) {
                            logger.info(result.message);
                            template().Id(result.id);
                            errors.removeAll();
                        } else {

                            errors(result.Errors);
                            logger.error("There are errors in your template, !!!");
                        }
                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
            testAsync = function () {
                var data = ko.mapping.toJSON(template);
                isBusy(true);

                return context.post(data, "/document-template/test")
                    .then(function (result) {
                        isBusy(false);
                        if (result.success) {
                            logger.info(result.message);
                            template().Id(result.id);
                            errors.removeAll();
                        } else {

                            errors(result.Errors);
                            logger.error("There are errors in your template, !!!");
                        }
                    });
            },
            removeAsync = function() {
                return context.send("{}", "/document-template/" + template().Id(), "DELETE");
            },
            canRemove = ko.computed(function() {
                return template().Id() && template().Id() !== "0";
            });

        var vm = {
            errors: errors,
            entityOptions: entityOptions,
            template: template,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            toolbar: {

                saveCommand: save,
                canExecuteSaveCommand: ko.computed(function () {
                    return template().Name() && template().Entity();
                }),
                removeCommand : removeAsync,
                canExecuteRemoveCommand : canRemove,
                commands: ko.observableArray([
                    {
                        command: publishAsync,
                        caption: "Publish",
                        icon: "fa fa-sign-out",
                        enable: ko.computed(function () {
                            return template().Id() && template().Id() !== "0";
                        })
                    },
                    {
                        command: testAsync,
                        caption: "Test",
                        icon: "fa fa-cog",
                        enable: ko.computed(function () {
                            return template().Id()
                                && template().Id() !== "0"
                                && template().Entity()
                                && template().WordTemplateStoreId();
                        })
                    }])
            }
        };

        return vm;

    });

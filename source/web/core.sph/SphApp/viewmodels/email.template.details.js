/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/**
 * @param{{Errors:function}} result
 */

define(["services/datacontext", "services/logger", objectbuilders.system, "knockout", "jquery"],
    function (context, logger, system, ko) {

        const template = ko.observable(new bespoke.sph.domain.EmailTemplate(system.guid())),
            entityOptions = ko.observableArray(),
            errors = ko.observableArray(),
            isBusy = ko.observable(false),
            activate = function (id) {
                const query = String.format("IsPublished eq true"),
                    query1 = String.format("Id eq '{0}'", id),
                    entityTask = context.loadAsync("EntityDefinition", query),
                    templateTask = context.loadOneAsync("EmailTemplate", query1),
                    tcs = new $.Deferred();

                $.when(entityTask, templateTask).then(function (lo, b) {
                    const  types = lo.itemCollection.map(v =>  v.Name());
                    entityOptions(types);
                    if (b) {
                        template(b);
                    } else {
                        template(new bespoke.sph.domain.EmailTemplate(system.guid()));
                    }
                    tcs.resolve(true);
                });
                return tcs.promise();
            },
            attached = function () {

            },
            save = function () {
                const  tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(template);
                isBusy(true);

                context.post(data, "email-template")
                    .then(function (id) {
                        isBusy(false);
                        template().Id(id);
                        tcs.resolve(id);
                    });
                return tcs.promise();
            },
            publishAsync = function () {

                const  tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(template);
                isBusy(true);

                context.post(data, `/email-template/publish`)
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

                const  tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(template);
                isBusy(true);

                context.post(data, "/email-template/test")
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
            };

        const vm = {
            errors: errors,
            entityOptions: entityOptions,
            template: template,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            toolbar: {

                saveCommand: save,
                commands: ko.observableArray([
                    {
                        command: publishAsync,
                        caption: "Publish",
                        icon: "fa fa-sign-out",
                        enable: ko.computed(function () {
                            return template().Id() && template().Id() !=="0";
                        })
                    },
                    {
                        command: testAsync,
                        caption: "Test",
                        icon: "fa fa-cog",
                        enable: ko.computed(function () {
                            return template().SubjectTemplate() && template().BodyTemplate() && template().Entity();
                        })
                    }])
            }
        };

        return vm;

    });

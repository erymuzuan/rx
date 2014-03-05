/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', objectbuilders.system],
    function (context, logger, system) {

        var template = ko.observable(new bespoke.sph.domain.EmailTemplate(system.guid())),
            entityOptions = ko.observableArray(),
            errors = ko.observableArray(),
            isBusy = ko.observable(false),
            activate = function (id) {
                var query = String.format("IsPublished eq 1"),
                    query1 = String.format("EmailTemplateId eq {0}", id),
                    entityTask = context.loadAsync("EntityDefinition", query),
                    templateTask = context.loadOneAsync("EmailTemplate", query1),
                    tcs = new $.Deferred();

                $.when(entityTask, templateTask).then(function (lo,b) {
                    var types = _(lo.itemCollection).map(function (v) {
                        return v.Name();
                    });
                    entityOptions(types);
                    template(b);
                    tcs.resolve(true);
                });
                return tcs.promise();
            },
            attached = function (view) {

            },
            save = function () {
                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(template);
                isBusy(true);

                context.post(data, "/Sph/EmailTemplate/Save")
                    .then(function (result) {
                        isBusy(false);
                        template().EmailTemplateId(result.id);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
            publishAsync = function () {

                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(template);
                isBusy(true);

                context.post(data, "/EmailTemplate/Publish")
                    .then(function (result) {
                        isBusy(false);
                        if (result.success) {
                            logger.info(result.message);
                            template().EmailTemplateId(result.id);
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

                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(template);
                isBusy(true);

                context.post(data, "/EmailTemplate/Test")
                    .then(function (result) {
                        isBusy(false);
                        if (result.success) {
                            logger.info(result.message);
                            template().EmailTemplateId(result.id);
                            errors.removeAll();
                        } else {

                            errors(result.Errors);
                            logger.error("There are errors in your template, !!!");
                        }
                        tcs.resolve(result);
                    });
                return tcs.promise();
            };

        var vm = {
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
                        caption: 'Publish',
                        icon: "fa fa-sign-out",
                        enable: ko.computed(function () {
                            return template().EmailTemplateId() > 0;
                        })
                    },
                    {
                        command: testAsync,
                        caption: 'Test',
                        icon: "fa fa-cog",
                        enable: ko.computed(function () {
                            return template().SubjectTemplate()
                            && template().BodyTemplate()
                            && template().Entity();
                        })
                    }])
            }
        };

        return vm;

    });

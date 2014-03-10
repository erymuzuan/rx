/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../../Scripts/_task.js" />
/// <reference path="../objectbuilders.js" />


define(['services/datacontext', 'services/logger', 'plugins/router', objectbuilders.system],
    function (context, logger, router, system) {

        var entity = ko.observable(new bespoke.sph.domain.EntityDefinition()),
            isBusy = ko.observable(false),
            errors = ko.observableArray(),
            forms = ko.observableArray(),
            views = ko.observableArray(),
            member = ko.observable(new bespoke.sph.domain.Member()),
            activate = function (entityid) {
                var id = parseInt(entityid);
                if (id) {
                    var query = String.format("EntityDefinitionId eq {0}", id),
                        tcs = new $.Deferred();
                    context.loadOneAsync("EntityDefinition", query)
                        .done(function (b) {
                            entity(b);
                            tcs.resolve(true);
                        });
                    // load forms and views
                    context.loadAsync("EntityForm", "EntityDefinitionId eq " + id)
                        .done(function (lo) {
                            forms(lo.itemCollection);
                        });
                    context.loadAsync("EntityView", "EntityDefinitionId eq " + id)
                        .done(function (lo) {
                            views(lo.itemCollection);
                        });


                    return tcs.promise();
                }
                var ed = new bespoke.sph.domain.EntityDefinition(system.guid());
                ed.Name.subscribe(function (name) {
                    if (!entity().Plural()) {
                        $.get('/Sph/EntityDefinition/GetPlural/' + name, function (v) {
                            entity().Plural(v);
                        });
                    }
                });

                entity(ed);
                return Task.fromResult(true);

            },
            attached = function () {

            },
            save = function () {
                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(entity);
                isBusy(true);

                context.post(data, "/EntityDefinition/Save")
                    .then(function (result) {
                        tcs.resolve(true);
                        isBusy(false);
                        if (result.success) {
                            logger.info(result.message);
                            entity().EntityDefinitionId(result.id);
                            errors.removeAll();
                        } else {
                            errors(result.Errors);
                            logger.error("There are errors in your entity, !!!");
                        }
                    });
                return tcs.promise();
            },
            publishAsync = function () {

                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(entity);
                isBusy(true);

                context.post(data, "/EntityDefinition/Publish")
                    .then(function (result) {
                        isBusy(false);
                        if (result.success) {
                            logger.info(result.message);
                            entity().EntityDefinitionId(result.id);
                            errors.removeAll();
                        } else {

                            errors(result.Errors);
                            logger.error("There are errors in your schema, !!!");
                        }
                        tcs.resolve(result);
                    });
                return tcs.promise();
            };


        var vm = {
            forms: forms,
            views: views,
            errors: errors,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            entity: entity,
            member: member,
            toolbar: {
                saveCommand: save,
                commands: ko.observableArray([
                    {
                        command: publishAsync,
                        caption: 'Publish',
                        icon: "fa fa-sign-out",
                        enable : ko.computed(function() {
                            return entity().EntityDefinitionId() > 0;
                        })
                    }])
            }
        };

        return vm;

    });

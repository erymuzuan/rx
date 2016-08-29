/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../../core.../schemas/form.designer.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../../Scripts/_task.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="../../../core.sph/SphApp/schemas/form.designer.g.js" />
/// <reference path="../../../core.sph/SphApp/objectbuilders.js" />
/// <reference path="../../../core.sph/Scripts/_task.js" />
/// <reference path="../../../core.sph/Scripts/require.js" />


define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.system, objectbuilders.app, "services/app", "services/new-item"],
    function (context, logger, router, system, app, servicesApp, nis) {

        let originalEntity = "";

        const port = ko.observable(new bespoke.sph.domain.ReceivePort()),
            errors = ko.observableArray(),
            isBusy = ko.observable(false),
            selected = ko.observable({
                Path: ko.observable(),
                TypeName: ko.observable()
            }),
            activate = function (id) {

               const query = `Id eq '${id}'`;
               return context.loadOneAsync("ReceivePort", query)
                   .then(function (b) {
                       if (!b) {
                           originalEntity = null;
                           return app.showMessage(`Cannot find any ReceivePort with id = ${id}`, "Not Found", ["OK"])
                           .done(function () {
                               return router.navigate("#dev.home");
                           });
                       }
                       port(b);
                       window.typeaheadEntity = b.Name();
                       return Task.fromResult(true);
                   });

           },
            attached = function () {

                originalEntity = ko.toJSON(port);
            },
            save = function () {

                const data = ko.mapping.toJSON(port);
                isBusy(true);

                return context.post(data, "/receive-ports")
                    .then(function (result) {
                        isBusy(false);
                        if (result.success) {
                            originalEntity = ko.toJSON(port);
                            logger.info(result.message);
                            errors.removeAll();
                        } else {
                            errors(result.errors);
                            logger.error("There are errors in your entity, !!!");
                        }
                    });
            },
            canDeactivate = function () {
                var tcs = new $.Deferred();
                if (!originalEntity) {
                    return true;
                }



                if (originalEntity !== ko.toJSON(port)) {
                    app.showMessage("Save change to the item", "Rx Developer", ["Yes", "No", "Cancel"])
                        .done(function (dialogResult) {
                            if (dialogResult === "Yes") {
                                save().done(function () {
                                    tcs.resolve(true);
                                });
                            }
                            if (dialogResult === "No") {
                                tcs.resolve(true);
                            }
                            if (dialogResult === "Cancel") {
                                tcs.resolve(false);
                            }

                        });
                } else {
                    return true;
                }
                return tcs.promise();
            },
            publishAsync = function () {

                const data = ko.mapping.toJSON(port);
                isBusy(true);

                return context.post(data, "/receive-port/publish")
                    .then(function (result) {

                        originalEntity = ko.toJSON(port);
                        isBusy(false);
                        if (result.success) {
                            logger.info(result.message);
                            errors.removeAll();
                        } else {

                            errors(result.Errors);
                            logger.error("There are errors in your schema, !!!");
                        }
                    });
            },
            removeAsync = function () {

                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(port);
                isBusy(true);
                app.showMessage("Are you sure you want to permanently remove this Receive Port?", "Reactive Developer", ["Yes", "No"])
                    .done(function (dialogResult) {
                        if (dialogResult === "Yes") {

                            context.send(data, `/receive-ports/${port().Id()}`, "DELETE")
                                .then(function (result) {
                                    isBusy(false);
                                    if (result.success) {
                                        logger.info(result.message);
                                        setTimeout(function () {
                                            window.location = "/sph#dev.home";
                                        }, 2000);
                                    } else {

                                        errors(result.Errors);
                                        logger.error("There are errors in your schema, !!!");
                                    }
                                    tcs.resolve(result);
                                });
                        } else {

                            tcs.resolve(false);
                        }
                    });

                return tcs.promise();
            },
            viewFile = function (e) {
                const file = e.FileName || e;
                const line = e.Line || 1;
                const params = [
                    `height=${screen.height}`,
                    `width=${screen.width}`,
                    "toolbar=0",
                    "location=0",
                    "fullscreen=yes"
                ].join(",");
                const editor = window.open(`/sph/editor/file?id=${file.replace(/\\/g, "/")}&line=${line}`, "_blank", params);
                editor.moveTo(0, 0);
            };


        const vm = {
            viewFile: viewFile,
            selected : selected,
            errors: errors,
            isBusy: isBusy,
            activate: activate,
            canDeactivate: canDeactivate,
            attached: attached,
            port: port,
            toolbar: {
                saveCommand: save,
                removeCommand: removeAsync,
                canExecuteRemoveCommand: function () {
                    return false;
                },
                commands: ko.observableArray([
                    {
                        command: publishAsync,
                        caption: "Publish",
                        icon: "fa fa-sign-in"
                    }])
            }
        };

        return vm;

    });

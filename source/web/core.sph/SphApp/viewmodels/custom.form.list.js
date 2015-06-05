﻿/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(["services/datacontext", "services/logger", "plugins/router"],
    function (context, logger, router) {

        var list = ko.observableArray(),
            isBusy = ko.observable(false),
            activate = function () {
                return $.get("/config/custom-routes").done(list);
            },
            attached = function (view) {

            },
            addNew = function () {
                require(["viewmodels/custom.form.dialog", "durandal/app"], function (dialog, app2) {
                    dialog.route({
                        "role": ko.observable(),
                        "groupName": ko.observable(),
                        "route": ko.observable(),
                        "moduleId": ko.observable(),
                        "title": ko.observable(),
                        "nav": ko.observable(),
                        "icon": ko.observable(),
                        "caption": ko.observable(),
                        "settings": ko.observable(),
                        "showWhenLoggedIn": ko.observable(),
                        "isAdminPage": ko.observable(),
                        "startPageRoute": ko.observable()
                    });
                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (result === "OK") {
                                context.post( ko.toJSON(dialog.route),"/config/custom-route")
                                .done(function () {
                                    list.push(dialog.route());
                                });
                            }
                        });

                });
                return Task.fromResult(0);
            },
            edit = function (route) {
                require(["viewmodels/custom.form.dialog", "durandal/app"], function (dialog, app2) {
                    dialog.route(ko.mapping.fromJS(route));
                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (result === "OK") {
                                context.post(ko.toJSON(dialog.route), "/config/custom-route")
                                .done(function () {
                                    list.push(dialog.route());
                                });
                            }
                        });

                });
                return Task.fromResult(0);
            },
            editViewModel = function (route) {
                window.open("/sph/editor/file?id=/sphapp/" + route.moduleId + ".js", "_blank", "height=600px,width=800px,toolbar=0,location=0");
            },
            editView = function (route) {

                window.open("/sph/editor/file?id=/sphapp/" + route.moduleId.replace("viewmodels/", "views/") + ".html", "_blank", "height=600px,width=800px,toolbar=0,location=0");
            };

        var vm = {
            list: list,
            edit: edit,
            editViewModel: editViewModel,
            editView: editView,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            toolbar: {
                addNewCommand: addNew
            }
        };

        return vm;

    });

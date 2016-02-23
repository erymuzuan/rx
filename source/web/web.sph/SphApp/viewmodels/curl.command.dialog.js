/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../../../core.sph/Scripts/require.js" />
/// <reference path="../../../core.sph/Scripts/__core.js" />
/// <reference path="../../../core.sph/SphApp/schemas/form.designer.g.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(["plugins/dialog", objectbuilders.config],
    function (dialog, config) {

        var endpoint = ko.observable(new bespoke.sph.domain.OperationEndpoint()),
            post = ko.observable(),
            put = ko.observable(),
            patch = ko.observable(),
            delete1 = ko.observable(),
            recurseProperty = function(model) {
                if (typeof model.$type !== "undefined") {
                    delete model.$type;
                }
                if (typeof model.WebId !== "undefined") {
                    delete model.WebId;
                }
                if (typeof model.Id !== "undefined") {
                    delete model.Id;
                }

                for (var n in model) {
                    if (model.hasOwnProperty(n)) {
                        if (typeof model[n] === "function") {
                            if (typeof model[n]() === "undefined") {
                                model[n]("test");
                            } else {
                                recurseProperty(model[n]());
                            }
                        }
                    }
                }
            },
            activate = function () {


                return $.getJSON("/developer-service/environment-variables")
                    .done(function (env) {
                        var model = new bespoke[config.applicationName + "_" + endpoint().Entity().toLowerCase()].domain[endpoint().Entity()]();
                        recurseProperty(model);

                        var json = ko.toJSON(model);

                        var patchText = "curl";
                        patchText += " -X PATCH";
                        patchText += " -H \"Content-Type: application/json\"";
                        patchText += " http://localhost:" + env["RX_" + config.applicationName.toUpperCase() + "_WebsitePort"] + "/api/" + endpoint().Resource() + "/" + endpoint().Route();
                        patchText += " -d '" + json + "'";
                        patch(patchText);

                        var postText = "curl";
                        postText += " -X POST";
                        postText += " -H \"Content-Type: application/json\"";
                        postText += " http://localhost:" + env["RX_" + config.applicationName.toUpperCase() + "_WebsitePort"] + "/api/" + endpoint().Resource() + "/" + endpoint().Route();
                        postText += " -d '" + json + "'";
                        post(postText);


                        var deleteText = "curl";
                        deleteText += " -X DELETE";
                        deleteText += " http://localhost:" + env["RX_" + config.applicationName.toUpperCase() + "_WebsitePort"] + "/api/" + endpoint().Resource() + "/" + endpoint().Route();
                        delete1(deleteText);

                        var putText = "curl";
                        putText += " -X PUT";
                        putText += " -H \"Content-Type: application/json\"";
                        putText += " http://localhost:" + env["RX_" + config.applicationName.toUpperCase() + "_WebsitePort"] + "/api/" + endpoint().Resource() + "/" + endpoint().Route();
                        putText += " -d '" + json + "'";
                        put(putText);
                    });


            },
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };

        var vm = {
            endpoint: endpoint,
            activate: activate,
            okClick: okClick,
            cancelClick: cancelClick,
            post: post,
            put: put,
            patch: patch,
            delete1: delete1
        };


        return vm;

    });

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


define(["plugins/dialog", objectbuilders.config, objectbuilders.logger],
    function (dialog, config, logger) {

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
                        String.prototype.toCamelCase = function () {
                            return this.replace(/^([A-Z])|\s(\w)/g, function (match, p1, p2, offset) {
                                if (p2) return p2.toUpperCase();
                                return p1.toLowerCase();
                            });
                        };
                        var model = new bespoke[config.applicationName + "_" + endpoint().Entity().toCamelCase()].domain[endpoint().Entity()]();
                        recurseProperty(model);

                        var obj = ko.toJS(model),
                            json = JSON.stringify(obj, null, "\t"),
                            port = "RX_" + config.applicationName.toUpperCase() + "_WebsitePort",
                            contentTypeHeader = " -H \"Content-Type: application/json\"",
                            url = " http://localhost:" + env[port] + "/api/" + endpoint().Resource() + "/",
                            body = " -d '" + json + "'";

                        var patchText = "curl";
                        patchText += " -X PATCH";
                        patchText += contentTypeHeader;
                        patchText += url + endpoint().Route();
                        patchText += body;
                        patch(patchText);

                        var postText = "curl";
                        postText += " -X POST";
                        postText += contentTypeHeader;
                        postText += url + endpoint().Route();
                        postText += body;
                        post(postText);


                        var deleteText = "curl";
                        deleteText += " -X DELETE";
                        deleteText += url + endpoint().Route();
                        delete1(deleteText);

                        var putText = "curl";
                        putText += " -X PUT";
                        putText += contentTypeHeader;
                        putText += url + endpoint().Route();
                        putText += body;
                        put(putText);
                    });


            },
            attached = function(view) {
                $(view).find("a.fa-copy").on("click", function() {
                    var textarea = $(view).find("#" + $(this).data("for"));
                    textarea.select();

                    var successful = document.execCommand("copy");
                    logger.info("Curl command copied " + successful);
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
            attached: attached,
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

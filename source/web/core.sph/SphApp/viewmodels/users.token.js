/// <reference path="../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.app],
    function (context, logger, router, app) {

        var isBusy = ko.observable(false),
            tokens = ko.observableArray(),
            activate = function () {


            },
            attached = function () {

            },
            map = function (v) {
                v.token = JSON.parse(ko.unwrap(v.Value));
                v.viewToken = function () {
                    return $.get("custom-token/" + ko.unwrap(v.Id))
                        .done(function (t) {
                            logger.info(t);
                        });
                };
                v.removeToken = function () {
                    return app.showMessage("Are you sure you want to revoke this token, This action cannot be undone", "Reactive Develeoper", ["Yes", "No"])
                        .done(function (dialogResult) {
                            if (dialogResult === "Yes") {
                                $.ajax({ type: "DELETE", url: "custom-token/" + ko.unwrap(v.Id) })
                                        .done(function () {
                                            logger.info("The token has been succesfully revoked");
                                            tokens.remove(v);
                                        });

                            }
                        });
                };
                return v;
            },
            addToken = function () {
                require(["viewmodels/security.token.dialog", "durandal/app"], function (dialog, app2) {
                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return false;
                            if (result === "OK") {
                                var tcs = new $.Deferred(),
                                    data = ko.toJSON(dialog.token);
                                context.post(data, "/custom-token")
                                    .then(function (r) {
                                        if (r.success) {
                                            logger.info("The token has been successfully generated " + r.access_token);
                                            var query = String.format("Id eq '{0}'", r.id);
                                            context.loadOneAsync("Setting", query)
                                                .done(function (b) {
                                                    tokens.push(map(b));
                                                });


                                        } else {
                                            logger.error(r.message);
                                        }
                                    });
                                return tcs.promise();
                            }
                            return false;
                        });

                });
            };

        var vm = {
            isBusy: isBusy,
            tokens: tokens,
            activate: activate,
            map: map,
            attached: attached,
            addToken: addToken
        };

        return vm;

    });

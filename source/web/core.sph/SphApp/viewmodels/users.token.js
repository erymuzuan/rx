/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.app, "services/app"],
    function (context, logger, router, app, serviceApp) {

        var isBusy = ko.observable(false),
            tokens = ko.observableArray(),
            viewToken = function (v) {
                return $.get("custom-token/" + ko.unwrap(v.WebId))
                    .done(function (t) {
                        serviceApp.prompt("Copy the token now", t);
                        setTimeout(function () {
                            var successful = document.execCommand("copy");
                            var msg = successful ? "successful" : "unsuccessful";
                            console.log("Copying token was " + msg);
                        }, 800);
                    });
            },
            removeToken = function (v) {
                return app.showMessage("Are you sure you want to revoke this token, This action cannot be undone", "Reactive Develeoper", ["Yes", "No"])
                    .done(function (dialogResult) {
                        if (dialogResult === "Yes") {
                            $.ajax({ type: "DELETE", url: "custom-token/" + ko.unwrap(v.WebId) })
                                    .done(function () {
                                        logger.info("The token has been succesfully revoked");
                                        tokens.remove(v);
                                    });
                        }
                    });
            },
            map = function (v) {
                v.iat = moment(v.iat).format("YYYY-MM-DD");
                v.exp = moment(v.exp).format("YYYY-MM-DD");
                v.viewToken = viewToken;
                v.removeToken = removeToken;
                return v;
            },
            activate = function () {
                return $.getJSON("/custom-token/")
                    .then(function (list) {
                        var tokenList = _(list).map(map);
                        tokens(tokenList);
                    });

            },
            attached = function () {

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

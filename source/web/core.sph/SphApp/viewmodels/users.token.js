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

        let searchText = "", pager = null;
        const isBusy = ko.observable(false),
            tokens = ko.observableArray(),
            totalRows = ko.observable(),
            viewToken = function (v) {
                return $.get(`/api/auth-tokens/${ko.unwrap(v.sub)}`)
                    .done(function (t) {
                        serviceApp.prompt("Copy the token now", t, "Security token", true);
                    });
            },
            removeToken = function (v) {
                return app.showMessage("Are you sure you want to revoke this token, This action cannot be undone", "Reactive Develeoper", ["Yes", "No"])
                    .done(function (dialogResult) {
                        if (dialogResult === "Yes") {
                            $.ajax({ type: "DELETE", url: `/api/auth-tokens/${ko.unwrap(v.WebId)}` })
                                    .done(function () {
                                        logger.info("The token has been succesfully revoked");
                                        tokens.remove(v);
                                    });
                        }
                    });
            },
            map = function (v) {
                v.iat = moment(v.iat * 1000).format("YYYY-MM-DD");
                v.exp = moment(v.exp * 1000).format("YYYY-MM-DD");
                v.viewToken = viewToken;
                v.removeToken = removeToken;
                return v;
            },
            activate = function () {
                searchText = "";
                return $.getJSON("api/auth-tokens/")
                    .then(function (lo) {
                        const tokenList = lo.ItemCollection.map(map);
                        tokens(tokenList);
                        totalRows(lo.TotalRows);
                        if (pager) {
                            pager.update(lo.TotalRows);
                        }
                    });
            },
            attached = function (view) {

                pager = new bespoke.utils.ServerPager({
                    element: $(view).find("div#tokens-pager"),
                    count: ko.unwrap(totalRows),
                    changed: function (page, size) {
                        var url = `api/auth-tokens/?page=${page}&size=${size}`;
                        if (searchText)
                            url = `/api/auth-tokens/_search?q=${searchText}&page=${page}&size=${size}`;

                        return $.getJSON(url)
                          .then(function (lo) {
                              const tokenList = lo.ItemCollection.map(map);
                              tokens(tokenList);
                          });
                    }
                });
                $(view).on("submit", "form.form-search", function (e) {
                    e.preventDefault();
                    isBusy(true);
                    searchText = $(view).find("input.search-query").val();
                    return context.get(`/api/auth-tokens/_search?q=${searchText}`)
                        .done(function (lo) {
                            isBusy(false);
                            const items = lo.ItemCollection.map(map);
                            logger.info(`search for text "${searchText}", ${lo.TotalRows} items found`);
                            setTimeout(function () {
                                tokens(items);
                            }, 500);
                            if (pager) {
                                pager.update(lo.TotalRows);
                            }

                        });
                });
            },
            addToken = function () {
                require(["viewmodels/security.token.dialog", "durandal/app"], function (dialog, app2) {
                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return false;
                            if (result === "OK") {
                                const tcs = new $.Deferred(),
                                    data = ko.toJSON(dialog.token);
                                context.post(data, "/api/auth-tokens")
                                    .then(function (r) {
                                        tokens.push(map(r));
                                        viewToken(r);
                                    });
                                return tcs.promise();
                            }
                            return false;
                        });

                });
            };

        totalRows.subscribe(function (rows) {
            console.log(`new pager ${rows}`);
            if (pager) {
                pager.update(rows);
                pager.page(1);
            }
        });

        const vm = {
            isBusy: isBusy,
            tokens: tokens,
            activate: activate,
            map: map,
            attached: attached,
            addToken: addToken,
            toolbar: {
                commands: ko.observableArray([{
                    caption: "Reload",
                    icon: "bowtie-icon bowtie-navigate-refresh",
                    command: activate
                }])
            }
        };

        return vm;

    });

/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/loadoperation.js" />
/// <reference path="logger.js" />
/// <reference path="domain.g.js" />

define(['services/logger'],
function (logger) {

    return {
        loadAsync: loadAsync,
        loadOneAsync: loadOneAsync,
        getSumAsync: getSumAsync,
        getCountAsync: getCountAsync,
        getListAsync: getListAsync,
        getTuplesAsync: getTuplesAsync,
        post: post
    };

    function post(json, url) {
        var tcs = new $.Deferred();
        $.ajax({
            type: "POST",
            data: json,
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: tcs.reject,
            success: tcs.resolve
        });

        return tcs.promise();
    }

    function loadOneAsync(entity, query) {
        var tcs = new $.Deferred();
        loadAsync(entity, query)
            .fail(function () {
                tcs.reject();
            })
            .done(function (lo) {
                if (lo.itemCollection.length) {
                    var item = lo.itemCollection[0];
                    tcs.resolve(item);
                } else {
                    tcs.resolve(null);
                }
            });

        return tcs.promise();
    }

    function loadAsync(entity, query, page, includeTotal) {
        var url = "/JsonDataService/" + entity;
        url += "/?filter=";
        if (page) {
            url += "/?page=" + page;
        }
        if (includeTotal) {
            url += "/?includeTotal=" + includeTotal;
        }
        url += query;
        logger.log(url);

        var tcs = new $.Deferred();
        $.ajax({
            type: "GET",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: tcs.reject,
            success: function (msg) {
                var temp = _(msg.results).map(function (v) {
                    var item = ko.mapping.fromJS(v);
                    if (bespoke.sphcommercialspace.domain[entity + "Partial"]) {
                        var extended = _(item).extend(new bespoke.sphcommercialspace.domain[entity + "Partial"]());
                        return extended;
                    }
                    return item;
                });
                var lo = new LoadOperation();
                lo.itemCollection = temp;
                lo.page = msg.page;
                lo.nextPageToken = msg.nextPageToken;
                lo.previousPageToken = msg.previousPageToken;
                lo.size = msg.size;
                lo.rows = msg.rows;

                tcs.resolve(lo);
            }
        });


        return tcs.promise();

    }

    function getSumAsync(entity, query, field) {
        return getAggregateAsync("sum", entity, query, field);
    }
    function getCountAsync(entity, query, field) {
        return getAggregateAsync("count", entity, query, field);
    }

    function getTuplesAsync(entity, query, field, field2) {
        var url = "/List/Tuple";
        url += "?filter=";
        url += query;
        url += "&column=";
        url += field;
        url += "&column2=";
        url += field2;
        url += "&table=" + entity;


        var tcs = new $.Deferred();
        $.ajax({
            type: "GET",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: tcs.reject,
            success: function (msg) {
                tcs.resolve(msg);
            }
        });


        return tcs.promise();
    }
    function getListAsync(entity, query, field) {
        var url = "/List/";
        url += "?filter=";
        url += query;
        url += "&column=";
        url += field;
        url += "&table=" + entity;


        var tcs = new $.Deferred();
        $.ajax({
            type: "GET",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: tcs.reject,
            success: function (msg) {
                tcs.resolve(msg);
            }
        });


        return tcs.promise();
    }

    function getAggregateAsync(aggregate, entity, query, field) {
        var url = "/aggregate/" + aggregate;
        url += "/?filter=";
        url += query;
        url += "&column=";
        url += field;
        url += "&table=" + entity;


        var tcs = new $.Deferred();
        $.ajax({
            type: "GET",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: tcs.reject,
            success: function (msg) {
                tcs.resolve(parseFloat(msg));
            }
        });


        return tcs.promise();
    }

    function LoadOperation() {
        var self = this;
        self.hasNextPage = false;
        self.itemCollection = [];
        self.page = 1;
        self.size = 40;
        self.rows = null;
        self.nextPageToken = "";
        self.previousPageToken = "";
    }

});
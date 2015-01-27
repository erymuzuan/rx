﻿/// <reference path="../../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/loadoperation.js" />
/// <reference path="../../Scripts/modernizr-2.7.2.js" />
/// <reference path="logger.js" />
/// <reference path="../objectbuilders.js" />

define(['services/logger', objectbuilders.system, 'durandal/knockout'],
function (logger, system, ko2) {
    if (!window.ko && typeof ko2 === "object") {
        window.ko = ko2;
    }
    var arrayTypeNamePattern = /\[/,
        toObservable = function (item) {
            if (typeof item === "function") return item;
            if (typeof item === "number") return item;
            if (typeof item === "string") return item;
            if (typeof item.$type === "undefined") return item;
            if (_(item.$type).isNull()) return item;

            var pattern = /Bespoke\..*?\..*?\.(.*?),/,
                $typeFieldValue = item.$type,
                type = "",
                partial = null;

            if (typeof item.$type === "function") {
                $typeFieldValue = item.$type();
            }
            if (typeof $typeFieldValue === "string") {
                var bespokeTypeMatch = pattern.exec($typeFieldValue);
                if (bespokeTypeMatch) {
                    type = bespokeTypeMatch[1];
                } 
            }

            for (var name in item) {
                (function (prop) {

                    var _propertyValue = _(item[prop]);

                    if (_propertyValue.isArray()) {

                        var children = _propertyValue.map(function (x) {
                            return toObservable(x, pattern);
                        });

                        item[prop] = ko.observableArray(children);
                        return;
                    }

                    if (_propertyValue.isNumber()
                        || _propertyValue.isNull()
                        || _propertyValue.isNaN()
                        || _propertyValue.isDate()
                        || _propertyValue.isBoolean()
                        || _propertyValue.isString()) {
                        item[prop] = ko.observable(item[prop]);
                        return;
                    }

                    if (_propertyValue.isObject()) {
                        var $typeFieldValue2 = item[prop].$type;

                        if ($typeFieldValue2 && arrayTypeNamePattern.exec($typeFieldValue2)) {
                            if (_(item[prop].$values).isArray()) {
                                var childItems = _(item[prop].$values).map(function (v) {
                                    return toObservable(v, pattern);
                                });
                                item[prop] = ko.observableArray(childItems);
                            }
                            return;
                        }

                        if (typeof item[prop] === "function") {
                            return;
                        }

                        var child = toObservable(item[prop], pattern);
                        item[prop] = ko.observable(child);
                        return;
                    }

                })(name);

            }

            if (bespoke.sph.domain[type + "Partial"]) {
                partial = new bespoke.sph.domain[type + "Partial"](item);
            }
            if (partial) {
                // NOTE :copy all the partial, DO NO use _extend as it will override the original value 
                // if there is item with the same key
                for (var prop1 in partial) {
                    if (!item[prop1]) {
                        item[prop1] = partial[prop1];
                    }
                }
            }

            // if there are new fields added, chances are it will not be present in the json,
            // even it is, it would be nice to add WebId for those that are still missing one

            if (bespoke.sph.domain[type]) {
                var ent = new bespoke.sph.domain[type](system.guid());
                for (var prop2 in ent) {
                    if (!item[prop2]) {
                        item[prop2] = ent[prop2];
                    }
                }
            }
            // addChildItemFunction
            item.addChildItem = function (list, childType) {
                return function () {
                    if (typeof childType === "function") {
                        list.push(new childType(system.guid()));
                        return;
                    }
                    console.log("Whoaaaaa");
                };
            };

            item.removeChildItem = function (list, obj) {
                return function () {
                    list.remove(obj);
                };
            };


            return item;

        },
        clone = function (item) {
            var item2 = ko.mapping.toJS(item);
            return this.toObservable(item2);
        },
        commit = function (source, destination) {
            for (var gp in destination) {
                if (typeof destination[gp] === "function" && destination[gp].name === "observable") {
                    destination[gp](ko.unwrap(source[gp]));
                } else {
                    destination[gp] = source[gp];
                }
            }
        },
      updateOnlineStatus = function (event) {
          var condition = navigator.onLine ? "online" : "offline";
          logger.info("Event: " + event.type + "; Status: " + condition);
      };

    window.addEventListener('online', updateOnlineStatus);
    window.addEventListener('offline', updateOnlineStatus);


    function send(json, url, verb) {
        var tcs = new $.Deferred();
        $.ajax({
            type: verb,
            data: json,
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: tcs.reject,
            success: tcs.resolve
        });

        return tcs.promise();
    }

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

    function get(url) {
        var tcs = new $.Deferred();
        $.ajax({
            type: "GET",
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

    function loadAsync(entityOrOptions, query) {

        if (!entityOrOptions) throw "This cannot be happending, you have to have entity or option";
        var entity = entityOrOptions,
            includeTotal = false,
            size = 20,
            page = 1,
            orderby = null;
        if (typeof entityOrOptions === "object") {
            entity = entityOrOptions.entity;
            includeTotal = entityOrOptions.includeTotal || false;
            page = entityOrOptions.page || 1;
            size = entityOrOptions.size || 20;
            orderby = entityOrOptions.orderby || entityOrOptions.sort;
        }

        var url = "/api/" + entity;
        url += "/?filter=" + (query || "");
        url += "&page=" + page;
        url += "&includeTotal=" + includeTotal;
        url += "&size=" + size;
        if (orderby) {
            url += "&$orderby=" + orderby;
        }

        var tcs = new $.Deferred();
        $.ajax({
            type: "GET",
            cache: false,
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: tcs.reject,
            success: function (msg) {
                var rows = _(msg.results).map(function (v) {
                    return toObservable(v);
                });
                var lo = new LoadOperation();
                lo.itemCollection = rows;
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

    function searchAsync(entityOrOptions, query) {

        if (!entityOrOptions) throw "This cannot be happening, you have to have entity or option";

        var entity = entityOrOptions,
           size = 20,
           page = 1;

        if (typeof entityOrOptions === "object") {
            entity = entityOrOptions.entity;
            page = entityOrOptions.page || 1;
            size = entityOrOptions.size || 20;
        }

        var url = "/search/" + entity.toLowerCase() + "/";
        //NOTE: for workflows
        if (entity.indexOf("_") > 0) {
            url = entity.toLowerCase() + "/search/";
        }
        query.from = (page - 1) * size;
        query.size = size;

        var tcs = new $.Deferred();
        $.ajax({
            type: "POST",
            url: url,
            data: JSON.stringify(query),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: tcs.reject,
            success: function (msg) {

                var hits = _(msg.hits.hits).chain()
                    .map(function (h) {
                        return h._source;
                    })
                .value();

                var lo = new LoadOperation();
                lo.itemCollection = hits;
                lo.page = page;
                lo.size = size;
                lo.rows = msg.hits.total;
                lo.facets = msg.facets;
                lo.aggregations = msg.aggregations;

                tcs.resolve(lo);
            }
        });


        return tcs.promise();
    }

    function getScalarAsync(entity, query, field) {
        return getAggregateAsync("scalar", entity, query, field);
    }
    function getMaxAsync(entity, query, field) {
        return getAggregateAsync("max", entity, query, field);
    }
    function getMinAsync(entity, query, field) {
        return getAggregateAsync("min", entity, query, field);
    }
    function getSumAsync(entity, query, field) {
        return getAggregateAsync("sum", entity, query, field);
    }
    function getCountAsync(entity, query, field) {
        return getAggregateAsync("count", entity, query, field);
    }

    function getTuplesAsync(entityOrOptions, query, field, field2) {

        var entity = entityOrOptions;
        if (entityOrOptions && typeof entityOrOptions === "object") {
            entity = entityOrOptions.entity;
            query = entityOrOptions.query;
            field = entityOrOptions.field;
            field2 = entityOrOptions.field2;
        }

        var url = "/List/Tuple?";
        if (query) {
            url += "filter=" + query + "&";
        }
        url += "column=";
        url += field;
        url += "&column2=";
        url += field2;
        url += "&table=" + entity;


        var tcs = new $.Deferred();
        $.ajax({
            type: "GET",
            cache: false,
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
        var url = "/List/?";
        if (query) {
            url += "filter=" + query + "&";
        }
        url += "column=";
        url += field;
        url += "&table=" + entity;


        var tcs = new $.Deferred();
        $.ajax({
            type: "GET",
            cache: false,
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
    function getDistinctAsync(entity, query, field) {
        var url = "/List/Distinct";
        url += "?filter=";
        url += query;
        url += "&column=";
        url += field;
        url += "&table=" + entity;


        var tcs = new $.Deferred();
        $.ajax({
            type: "GET",
            cache: false,
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
            cache: false,
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: tcs.reject,
            success: function (msg) {
                if (aggregate === "scalar") {
                    tcs.resolve(msg);
                } else {
                    tcs.resolve(parseFloat(msg));
                }
            }
        });


        return tcs.promise();
    }

    // ReSharper disable InconsistentNaming
    function LoadOperation() {
        // ReSharper restore InconsistentNaming
        var self = this;
        self.hasNextPage = false;
        self.itemCollection = [];
        self.page = 1;
        self.size = 40;
        self.rows = null;
        self.nextPageToken = "";
        self.previousPageToken = "";
    }


    return {
        searchAsync: searchAsync,
        loadAsync: loadAsync,
        loadOneAsync: loadOneAsync,
        getScalarAsync: getScalarAsync,
        getMaxAsync: getMaxAsync,
        getMinAsync: getMinAsync,
        getSumAsync: getSumAsync,
        getCountAsync: getCountAsync,
        getListAsync: getListAsync,
        getDistinctAsync: getDistinctAsync,
        getTuplesAsync: getTuplesAsync,
        post: post,
        send: send,
        get: get,
        clone: clone,
        commit: commit,
        toObservable: toObservable
    };
});
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/loadoperation.js" />
/// <reference path="logger.js" />
/// <reference path="/App/schemas/sph.domain.g.js" />

define(['services/logger', 'durandal/system'],
function (logger, system) {

    return {
        searchAsync: searchAsync,
        loadAsync: loadAsync,
        loadOneAsync: loadOneAsync,
        getMaxAsync: getMaxAsync,
        getMinAsync: getMinAsync,
        getSumAsync: getSumAsync,
        getCountAsync: getCountAsync,
        getListAsync: getListAsync,
        getDistinctAsync: getDistinctAsync,
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

    function loadAsync(entityOrOptions, query) {

        if (!entityOrOptions) throw "This cannot be happending, you have to have entity or option";
        var entity = entityOrOptions,
            includeTotal = false,
            size = 20,
            page = 1;
        if (typeof entityOrOptions === "object") {
            entity = entityOrOptions.entity;
            includeTotal = entityOrOptions.includeTotal || false;
            page = entityOrOptions.page || 1;
            size = entityOrOptions.size || 20;
        }

        var url = "/JsonDataService/" + entity;
        url += "/?filter=" + (query || "");
        url += "&page=" + page;
        url += "&includeTotal=" + includeTotal;
        url += "&size=" + size;
        logger.log(url);

        var tcs = new $.Deferred();
        $.ajax({
            type: "GET",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: tcs.reject,
            success: function (msg) {
                var pattern = /Bespoke\.Sph\.Domain\.(.*?),/;
                var rows = _(msg.results).map(function (v) {
                    var type = pattern.exec(v['$type'])[1];

                    var observable = (function toObservable(item) {
                        if (typeof item === "function") return item;
                        if (typeof item === "number") return item;
                        if (typeof item === "string") return item;
                        if (typeof item['$type'] === "undefined") return item;
                        if (_(item['$type']).isNull()) return item;

                        if (typeof item['$type'] === "function") {
                            type = pattern.exec(item['$type']())[1];
                        }
                        if (typeof item['$type'] === "string") {
                            type = pattern.exec(item['$type'])[1];
                        }
                        

                        if (bespoke.sph.domain[type + "Partial"]) {
                            var partial = new bespoke.sph.domain[type + "Partial"](item);
                        }
                        
                        for (var name in item) {
                            (function (prop) {

                                var propval = _(item[prop]);

                                if (propval.isArray()) {

                                    var children = propval.map(function (x) {
                                        return toObservable(x);
                                    });

                                    item[prop] = ko.observableArray(children);
                                    return;
                                }

                                if (propval.isNumber()
                                    || propval.isNull()
                                    || propval.isNaN()
                                    || propval.isDate()
                                    || propval.isBoolean()
                                    || propval.isString()) {
                                    item[prop] = ko.observable(item[prop]);
                                    return;
                                }

                                if (propval.isObject()) {
                                    var child = toObservable(item[prop]);
                                    item[prop] = ko.observable(child);
                                    return;
                                }

                            })(name);

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
                        // even it is, it would be nice to add Webid for those whos still missing one
                        
                        if (bespoke.sph.domain[type]) {
                            var ent = new bespoke.sph.domain[type](system.guid());
                            for (var prop2 in ent) {
                                if (!item[prop2]) {
                                    item[prop2] = ent[prop2];
                                }
                            }
                        }
                        return item;

                    })(v);
                    return observable;
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
        
        if (!entityOrOptions) throw "This cannot be happending, you have to have entity or option";
        
        var entity = entityOrOptions,
           size = 20,
           page = 1;
        
        if (typeof entityOrOptions === "object") {
            entity = entityOrOptions.entity;
            page = entityOrOptions.page || 1;
            size = entityOrOptions.size || 20;
        }
        
        var url = "/search/" + entity.toLowerCase();
        query.from = (page - 1) * size;
        query.size = size;
        
        var tcs = new $.Deferred();
        $.ajax({
            type: "POST",
            url: url,
            data : JSON.stringify(query),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: tcs.reject,
            success: function (msg) {

                var hits = _(msg.hits.hits).map(function(h) {
                    return h._source;
                });

                var pattern = /Bespoke\.Sph\.Domain\.(.*?),/;
                var rows = _(hits).map(function (v) {
                    var type = pattern.exec(v['$type'])[1];

                    var observable = (function toObservable(item) {
                        if (typeof item === "function") return item;
                        if (typeof item === "number") return item;
                        if (typeof item === "string") return item;
                        if (typeof item['$type'] === "undefined") return item;
                        if (_(item['$type']).isNull()) return item;

                        if (typeof item['$type'] === "function") {
                            type = pattern.exec(item['$type']())[1];
                        }
                        if (typeof item['$type'] === "string") {
                            type = pattern.exec(item['$type'])[1];
                        }


                        if (bespoke.sph.domain[type + "Partial"]) {
                            var partial = new bespoke.sph.domain[type + "Partial"](item);
                        }

                        for (var name in item) {
                            (function (prop) {

                                var propval = _(item[prop]);

                                if (propval.isArray()) {

                                    var children = propval.map(function (x) {
                                        return toObservable(x);
                                    });

                                    item[prop] = ko.observableArray(children);
                                    return;
                                }

                                if (propval.isNumber()
                                    || propval.isNull()
                                    || propval.isNaN()
                                    || propval.isDate()
                                    || propval.isBoolean()
                                    || propval.isString()) {
                                    item[prop] = ko.observable(item[prop]);
                                    return;
                                }

                                if (propval.isObject()) {
                                    var child = toObservable(item[prop]);
                                    item[prop] = ko.observable(child);
                                    return;
                                }

                            })(name);

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
                        // even it is, it would be nice to add Webid for those whos still missing one

                        if (bespoke.sph.domain[type]) {
                            var ent = new bespoke.sph.domain[type](system.guid());
                            for (var prop2 in ent) {
                                if (!item[prop2]) {
                                    item[prop2] = ent[prop2];
                                }
                            }
                        }
                        return item;

                    })(v);
                    return observable;
                });
                var lo = new LoadOperation();
                lo.itemCollection = rows;
                lo.page = page;
                lo.size = size;
                lo.rows = msg.hits.total;
                

                tcs.resolve(lo);
            }
        });


        return tcs.promise();
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

});
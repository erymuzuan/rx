﻿/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="domain.g.js" />
/// <reference path="../../Scripts/loadoperation.js" />
/// <reference path="logger.js" />

define(['services/logger'],
function (logger) {

    var context = {
        loadAsync: loadAsync,
        loadOneAsync: loadOneAsync,
        getSumAsync: getSumAsync,
        getCountAsync: getCountAsync,
        getListAsync: getListAsync,
        getTuplesAsync: getTuplesAsync,
        post: post
    };
    return context;

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
                    tcs.resolve(lo.itemCollection[0]);
                } else {
                    tcs.resolve(null);
                }
            });

        return tcs.promise();
    }

    function loadAsync(entity, query, page, includeTotal) {
        var tcs = new $.Deferred();
        setTimeout(function () {

            var lo = new LoadOperation();

            var cat = new bespoke.sphcommercialspace.domain.ComplaintCategory();
            cat.Name("Pendawaian");
            cat.SubCategoryCollection.push("test 1","test 2");
            
            var cat2 = new bespoke.sphcommercialspace.domain.ComplaintCategory();
            cat2.Name("Litar Pintas");
            cat2.SubCategoryCollection.push("test A","test B");
            
            
            var c = new bespoke.sphcommercialspace.domain.ComplaintTemplate();
            c.ComplaintTemplateId(1);
            c.Name("Kerosakan");
            c.IsActive(true);
            c.ComplaintCategoryCollection.push(cat, cat2);
           

            var c2 = new bespoke.sphcommercialspace.domain.ComplaintTemplate();
            c2.ComplaintTemplateId(2);
            c2.Name("Mekanikal");
            c2.IsActive(true);

            var c3 = new bespoke.sphcommercialspace.domain.ComplaintTemplate();
            c3.ComplaintTemplateId(3);
            c3.Name("Keselamatan");
            c3.IsActive(true);
            
            lo.itemCollection.push(c, c2 , c3);
            tcs.resolve(lo);


        }, 500);

        return tcs.promise();
    };



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
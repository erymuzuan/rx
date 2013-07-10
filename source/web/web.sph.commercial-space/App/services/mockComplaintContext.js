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
            var c = new bespoke.sphcommercialspace.domain.Complaint();
            c.ComplaintId(1);
            c.ReferenceNo('AD0001');
            c.Title('Kerosakan Lampu');
            c.Category('Elektrikal');
            c.SubCategory('Lampu');
            c.Status('New');
            c.TenantId(3);
            c.Type('Kerosakan');
            c.CommercialSpaceId(10);
            c.Remarks('ape ape ape');

            var c2 = new bespoke.sphcommercialspace.domain.Complaint();
            c2.ComplaintId(2);
            c2.ReferenceNo('AD0002');
            c2.Title('Kerosakan Paip');
            c2.Category('Perpaipan');
            c2.SubCategory('Paip Dapur');
            c2.Status('New');
            c2.TenantId(4);
            c2.Type('Kerosakan');
            c2.CommercialSpaceId(16);
            c2.Remarks('ape ape ape');

            lo.itemCollection.push(c, c2);
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
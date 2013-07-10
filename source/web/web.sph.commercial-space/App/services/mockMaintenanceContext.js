/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
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

            var maintenance1 = new bespoke.sphcommercialspace.domain.Maintenance();
            maintenance1.MaintenanceId(1);
            maintenance1.ComplaintId(11);
            maintenance1.Complaint().ReferenceNo('AD201311');
            maintenance1.Complaint().Category('Elektrikal');
            maintenance1.Complaint().SubCategory('Lampu Lobby');
            maintenance1.Complaint().Remarks('Testing in progress.....');
            maintenance1.WorkOrderNo('');
            maintenance1.Department('Unit Senggara');
            maintenance1.Status('New');
            maintenance1.Resolution('Not Started');
            maintenance1.Officer('');
            maintenance1.StartDate('2001-12-17T09:30:47.0Z');
            maintenance1.EndDate('2001-12-17T09:30:47.0Z');
            
            var maintenance2 = new bespoke.sphcommercialspace.domain.Maintenance();
            maintenance2.MaintenanceId(2);
            maintenance2.ComplaintId(10);
            maintenance2.Complaint().ReferenceNo('AD201310');
            maintenance2.Complaint().Category('Elektrikal');
            maintenance2.Complaint().SubCategory('Lampu Hadapan');
            maintenance2.Complaint().Remarks('Testing in progress.....');
            maintenance2.WorkOrderNo('');
            maintenance2.Department('Unit Senggara');
            maintenance2.Status('New');
            maintenance2.Resolution('Not Started');
            maintenance2.Officer('');
            maintenance2.StartDate('2001-12-17T09:30:47.0Z');
            maintenance2.EndDate('2001-12-17T09:30:47.0Z');
            
            var maintenance3 = new bespoke.sphcommercialspace.domain.Maintenance();
            maintenance3.MaintenanceId(3);
            maintenance3.ComplaintId(9);
            maintenance3.Complaint().ReferenceNo('AD201309');
            maintenance3.Complaint().Category('Bangunan');
            maintenance3.Complaint().SubCategory('Dinding Retak');
            maintenance3.Complaint().Remarks('Testing in progress.....');
            maintenance3.WorkOrderNo('');
            maintenance3.Department('Unit Senggara');
            maintenance3.Status('New');
            maintenance3.Resolution('Not Started');
            maintenance3.Officer('');
            maintenance3.StartDate('2001-12-17T09:30:47.0Z');
            maintenance3.EndDate('2001-12-17T09:30:47.0Z');
            

            lo.itemCollection.push(maintenance1, maintenance2, maintenance3);
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
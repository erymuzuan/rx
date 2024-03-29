﻿/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../../core.sph/Scripts/require.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(["services/datacontext", "services/logger", "plugins/router"],
    function (context, logger, router) {

        var logs = ko.observableArray(),
            parentRoot = ko.observable(),
            totalRows = ko.observable(0),
            pager = null,
            currentPage = ko.observable(1),
            currentPageSize = ko.observable(10),
            isBusy = ko.observable(false),
            changed = function(page,size){
                if(!pager){
                    return;
                }
                currentPage(page);
                currentPageSize(size);
                var skip = (page - 1) * size,
                    take = size,
                    model = parentRoot().model().Id();

                $.getJSON("/api/data-imports/" + model + "/histories?$skip=" + skip + "&$take=" + take)
                    .done(function(result){
                        logs(result.items);
                    });
            },
            activate = function (root) {
                parentRoot(ko.unwrap(root));
                parentRoot().progressData().busy.subscribe(function(busy){
                    if(!ko.unwrap(busy) && pager){
                        changed(1, currentPageSize());
                    }
                });
            },
            createPager = function(element){

                var options = {
                        element :element,
                        changed : changed,
                        count : ko.unwrap(totalRows),
                        sizes : [10, 20, 50],
                        defaultSize : 10
                    };
                pager = new bespoke.utils.ServerPager(options);
            },
            attached = function (view) {
                var element = $(view).find("#history-pager"),
                    loadLogs = function (model) {
                        $.getJSON("/api/data-imports/" + ko.unwrap(model.Id) + "/histories?$take=10&$skip=0")
                            .done(function (result) {
                                logs(result.items);
                                totalRows(result.total);
                                createPager(element);
                            });
                    };
                parentRoot().model.subscribe(loadLogs);
                loadLogs(ko.unwrap(parentRoot().model));
            },
            resume = function (item) {
                console.log(item);
                return parentRoot().resume(item)
                    .done(function(){
                        changed(1, currentPageSize());
                    });
            };

        var vm = {
            logs: logs,
            isBusy: isBusy,
            parentRoot :parentRoot,
            activate: activate,
            attached: attached,
            resume : resume
        };

        return vm;

    });

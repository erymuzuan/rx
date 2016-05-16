/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
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
                    model = parentRoot().model().name();

                $.getJSON("/api/data-imports/" + model + "/histories?$skip=" + skip + "&$take=" + take)
                    .done(function(result){
                        logs(result.items);
                    });
            },
            activate = function (root) {
                parentRoot(ko.unwrap(root));
            },
            createPager = function(element){
                if(pager && typeof pager.destroy === "function"){
                    pager.destroy();
                }
                var options = {},
                    count = ko.unwrap(totalRows),
                    sizes =  [10, 20, 50],
                    defaultSize = 10,
                    self2 = this,
                    rows = _.range(count),
                    pagerDataSource = new kendo.data.DataSource({
                        data: rows,
                        pageSize: defaultSize
                    });
                if (options.hidden) {
                    return self2;
                }

                pager = element.kendoPager({
                    dataSource: pagerDataSource,
                    pageSizes: sizes
                }).data("kendoPager");
                pager.page(1);
                pager.bind("change", function () {
                    if (changed) {
                        changed(pager.page(), pager.pageSize());
                    }
                });

                self2.update = function (count2) {
                    rows = [];
                    for (var j = 0; j < count2 ; j++) {
                        rows[j] = j;
                    }
                    setTimeout(function () {
                        pagerDataSource.data(rows);
                    }, 500);
                };
                self2.destroy = function () {
                    pager.destroy();
                    element.empty();
                };

                self2.pageSize = function (size) {
                    if (size) {
                        pager.pageSize(size);
                    }
                    return pager.pageSize();
                };
                self2.page = function (pg) {
                    if (pg) {
                        pager.page(pg);
                    }
                    return pager.page();
                };

                var dropdownlist = $(element).find("select").data("kendoDropDownList");
                dropdownlist.bind("change", function () {
                    try {
                        changed(1, parseInt(this.value()));
                    } catch (e) {

                    }
                });

            },
            attached = function (view) {
                var element = $(view).find("#history-pager");
                parentRoot().model().name.subscribe(function(model){
                    $.getJSON("/api/data-imports/" + model + "/histories?$take=10&$skip=0")
                        .done(function(result){
                            logs(result.items);
                            totalRows(result.total);
                            createPager(element);
                        });
                });
            },
            resume = function (item) {
                console.log(item);
                return Task.fromResult(true);
            };

        var vm = {
            logs: logs,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            resume : resume
        };

        return vm;

    });

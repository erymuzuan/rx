/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../../core.sph/Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../../Scripts/re.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.app],
    function (context, logger, router, app) {

        var isBusy = ko.observable(false),
            parentRoot = ko.observable(),
            totalRows = ko.observable(0),
            pager = null,
            currentPage = ko.observable(1),
            currentPageSize = ko.observable(10),
            changed = function(page,size){
                if(!pager){
                    return;
                }
                currentPage(page);
                currentPageSize(size);
                var skip = (page - 1) * size,
                    take = size,
                    model = parentRoot().model().name();

                $.getJSON("/api/data-imports/" + model + "/errors?$skip=" + skip + "&$take=" + take)
                    .done(function(result){
                        parentRoot().errorRows(result.items);
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

                var element = $(view).find("#pager");
                parentRoot().model.subscribe(function(model){
                    $.getJSON("/api/data-imports/" + ko.unwrap(model.Id) + "/errors?$take=10&$skip=0")
                        .done(function(result){
                            parentRoot().errorRows(result.items);
                            totalRows(result.total);
                            createPager(element);
                        });
                });

            },
            viewData = function (row) {
                var params = [
                           "height=" + screen.height,
                           "width=" + screen.width,
                           "toolbar=0",
                           "location=0",
                           "fullscreen=yes"
                        ].join(","),
                    editor = window.open("/sph/editor/file?id=/App_Data/data-imports/" + row.ErrorId + ".data", "_blank", params);
                    editor.moveTo(0, 0);

                // #3967
                editor.window.onload = function(){
                    $(editor.window.document).find("a[data-bind='click : saveAndClose']")
                        .html("<i style=\"margin-right: 5px\" class=\"fa fa-play\"></i> Save &amp; Execute");

                    $(editor.window.document).find("a[data-bind='click : function(){ window.close();}']")
                        .html("<i style=\"margin-right: 5px\" class=\"fa fa-times\"></i> Close");
                };
                editor.window.saved = function (code, close) {
                    console.log(code, "resend");
                    if (close) {
                        editor.close();
                    }
                    row.Data = JSON.parse(code);
                    parentRoot().importOneRow(row)
                        .done(function(){
                            parentRoot().errorRows.remove(row);
                        });
                };
            },
            ignoreRow = function(row){

                var tcs = new $.Deferred();
                app.showMessage("Are you sure you want to ignore this row", "RX Developer", ["Yes", "No"])
                    .done(function (dialogResult) {
                        if (dialogResult === "Yes") {
                            parentRoot().ignoreRow(row)
                                .done(function(){
                                    parentRoot().errorRows.remove(row);
                                    tcs.resolve(row);

                                    //
                                    pager.destroy();
                                    var skip = (currentPage() - 1) * currentPageSize(),
                                        take = currentPageSize(),
                                        model = parentRoot().model().name(),
                                        element = $("#pager");
                                    $.getJSON("/api/data-imports/" + model + "/errors?$take=" + take + "&$skip=" + skip)
                                        .done(function(result){
                                            parentRoot().errorRows(result.items);
                                            totalRows(result.total);
                                            createPager(element);
                                            changed(currentPage(), currentPageSize());
                                        });
                                });

                        } else {
                            tcs.resolve(false);
                        }
                    });

                return tcs.promise();
            },
            viewException = function (row) {
                console.log(row.Exception);
            };

        var vm = {
            isBusy: isBusy,
            currentPageSize :currentPageSize,
            currentPage:currentPage,
            parentRoot: parentRoot,
            ignoreRow : ignoreRow,
            activate: activate,
            attached: attached,
            viewData: viewData,
            viewException: viewException
        };

        return vm;

    });

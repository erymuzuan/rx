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
            activate = function (root) {
                parentRoot(ko.unwrap(root));
                parentRoot().model().name.subscribe(function(model){
                    $.getJSON("/api/data-imports/" + model + "/errors")
                        .done(parentRoot().errorRows);
                });
            },
            attached = function (view) {
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
            parentRoot: parentRoot,
            ignoreRow : ignoreRow,
            activate: activate,
            attached: attached,
            viewData: viewData,
            viewException: viewException
        };

        return vm;

    });

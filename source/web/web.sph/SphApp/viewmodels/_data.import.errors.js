/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../../core.sph/Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../../Scripts/re.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(["services/datacontext", "services/logger", "plugins/router"],
    function (context, logger, router) {

        var isBusy = ko.observable(false),
            parentRoot = ko.observable(),
            activate = function (root) {
                parentRoot(ko.unwrap(root));
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
            viewException = function (row) {
                console.log(row.Exception);
            };

        var vm = {
            isBusy: isBusy,
            parentRoot: parentRoot,
            activate: activate,
            attached: attached,
            viewData: viewData,
            viewException: viewException
        };

        return vm;

    });

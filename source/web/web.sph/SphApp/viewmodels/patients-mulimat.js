/// <reference path="Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="Scripts/knockout-3.2.0.debug.js" />
/// <reference path="Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="Scripts/require.js" />
/// <reference path="Scripts/underscore.js" />
/// <reference path="Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(["services/datacontext", "services/logger", "plugins/router", "services/chart", objectbuilders.config ,"services/_ko.list"],
    function (context, logger, router, chart,config ) {

        var isBusy = ko.observable(false),
            list = ko.observableArray([]),
            map = function(v) {
                if (typeof partial !== "undefined" && typeof partial.map === "function") {
                    return partial.map(v);
                }
                return v;
            },
            activate = function () {
              return true;
            },
            attached = function (view) {
            };

        var vm = {
            config: config,
            isBusy: isBusy,
            map: map,
            activate: activate,
            attached: attached,
            list: list,
            toolbar: {
                commands: ko.observableArray([])
            }
        };

        return vm;

    });

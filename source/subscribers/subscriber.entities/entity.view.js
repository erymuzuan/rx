/// <reference path="Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="Scripts/knockout-3.4.0.debug.js" />
/// <reference path="Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="Scripts/require.js" />
/// <reference path="Scripts/underscore.js" />
/// <reference path="Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(["services/datacontext", "services/logger", "plugins/router", "services/chart", objectbuilders.config @Raw(Model.PartialPath)],
    function (context, logger, router, chart,config @Model.PartialArg) {

        var isBusy = ko.observable(false),
            chartFiltered = ko.observable(false),
            view = ko.observable(),
            list = ko.observableArray([]),
            map = function(v) {
                if (typeof partial !== "undefined" && typeof partial.map === "function") {
                    return partial.map(v);
                }
                return v;
            },
            entity = ko.observable(new bespoke.sph.domain.EntityDefinition()),
            activate = function (@Model.Routes)
            {
                <text></text>
                     @if (!string.IsNullOrWhiteSpace(Model.PartialArg))
                     {
                         <text>
                            var  tcs = new $.Deferred();
                         if(typeof partial !== "undefined" && typeof partial.activate === "function"){
                             var pt = partial.activate(list);
                             if(typeof pt.done === "function"){
                                 pt.done(tcs.resolve);
                             }else{
                                 tcs.resolve(true);
                             }
                         }
                         return tcs.promise();
                         </text>
                     }
                     



            },
            attached = function (view) {
                chart.init("@Model.Definition.Name", query, chartSeriesClick, "@Model.View.Id");
                @if (!string.IsNullOrWhiteSpace(Model.PartialArg))
                {
                    <text>
                    if(typeof partial !== "undefined" && typeof partial.attached === "function"){
                        partial.attached(view);
                    }
                    </text>
                }
            };

        var vm = {
            config: config,
            view: view,
            chart: chart,
            isBusy: isBusy,
            map: map,
            entity: entity,
            activate: activate,
            attached: attached,
            list: list,
            clearChartFilter:clearChartFilter,
            chartFiltered:chartFiltered,
            query: query,
            toolbar: {
                commands: ko.observableArray([])
            }
        };

        return vm;

    });

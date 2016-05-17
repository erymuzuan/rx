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

        var schedule = ko.observable(new bespoke.sph.domain.IntervalScheduleContainer()),
            parentRoot = ko.observable(),
            isBusy = ko.observable(false),
            activate = function (root) {
                parentRoot(ko.unwrap(root));
                var model = parentRoot().model(),
                    item =  _(model).extend(new bespoke.sph.domain.IntervalScheduleContainer(model));
                if(!ko.isObservable(item.IntervalScheduleCollection)){
                    item.IntervalScheduleCollection = ko.observableArray();
                }
                schedule(item);


                parentRoot().model().id.subscribe(function(id){
                    $.getJSON("/api/data-imports/" + id + "/schedules")
                        .done(function(result){
                            var list = _(result).map(function(v){
                                return context.toObservable(v);
                            });
                            item.IntervalScheduleCollection(list);
                        });
                });
            },
            attached = function (view) {
            },
            removeItem = function(item){

            };

        var vm = {
            isBusy: isBusy,
            parentRoot :parentRoot,
            activate: activate,
            attached: attached,
            removeItem : removeItem,
            schedule: schedule
        };

        return vm;

    });

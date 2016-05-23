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
            mapScheduleMetadata = function(model){
                var list = ko.unwrap(model.IntervalScheduleCollection),
                    properties = ko.unwrap(model.ScheduledDataTransferCollection);
                _(list).each(function(v){
                    var metadata = _(properties).find(function(x){
                        return ko.unwrap(v.WebId) === ko.unwrap(x.ScheduleWebId);
                    }) || new bespoke.sph.domain.ScheduledDataTransfer({ScheduleWebId: ko.unwrap(v.WebId)});

                    v.Metadata = metadata;
                });
            },
            scheduleAdded = function(changes){
                _(changes).each(function(v){
                    if(v.status === "added"){
                        v.value.Metadata = new bespoke.sph.domain.ScheduledDataTransfer({ScheduleWebId: ko.unwrap(v.value.WebId)});
                    }
                });
            },
            activate = function (root) {
                parentRoot(ko.unwrap(root));
                var model = parentRoot().model(),
                    item =  _(model).extend(new bespoke.sph.domain.IntervalScheduleContainer(model));
                if(!ko.isObservable(item.IntervalScheduleCollection)){
                    item.IntervalScheduleCollection = ko.observableArray();
                }
                mapScheduleMetadata(item);
                schedule(item);
                item.IntervalScheduleCollection.subscribe(scheduleAdded, null, "arrayChange");
            },
            attached = function (view) {
                parentRoot().model.subscribe(function(model){
                    model.IntervalScheduleCollection.subscribe(scheduleAdded, null, "arrayChange");
                });

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

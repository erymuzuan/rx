/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/report.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/report.g', 'services/datacontext', 'services/logger', 'durandal/plugins/router', 'durandal/system'],
    function (reportg, context, logger, router, system) {

        var isBusy = ko.observable(false),
            rdl = ko.observable(),
            delivery = ko.observable(),
            editedSchedule = ko.observable(),
            activate = function (routeData) {
                var rdlid = parseInt(routeData.rdlid),
                    query = String.format("ReportDefinitionId eq {0}", rdlid),
                    tcs = new $.Deferred();
                

                context.loadOneAsync("ReportDelivery", query)
                    .done(tcs.resolve)
                    .done(function (d) {
                        if (!d) {
                            d = new bespoke.sphcommercialspace.domain.ReportDelivery(system.guid());
                            d.ReportDefinitionId(rdlid);
                            d.ReportDeliveryId(0);
                        }
                        delivery(d);
                    });

                context.getListAsync("UserProfile", "UserProfileId gt 0", "Username")
                    .done(vm.userOptions);


                var options = [
                    new bespoke.sphcommercialspace.domain.HourlySchedule(system.guid()),
                    new bespoke.sphcommercialspace.domain.DailySchedule(system.guid()),
                    new bespoke.sphcommercialspace.domain.WeeklySchedule(system.guid()),
                    new bespoke.sphcommercialspace.domain.MonthlySchedule(system.guid())
                ];
                vm.scheduleOptions(options);
                return tcs.promise();
            },
            viewAttached = function (view) {


            },
            okDialogHelper = null,
            okDialog = function () {
                okDialogHelper();
            },
            editSchedule = function (schedule, e) {
                e.preventDefault();
                var clone = ko.mapping.fromJS(ko.mapping.toJS(schedule));
                clone.WebId(system.guid());
                clone.isNewItem = true;
                editedSchedule(clone);

                okDialogHelper = function () {
                    delivery().IntervalScheduleCollection.replace(schedule, clone);
                };

                $('#schedule-dialog').modal();

            },
            startAddSchedule = function (data, e) {
                e.preventDefault();
                var clone = ko.mapping.fromJS(ko.mapping.toJS(data));
                clone.WebId(system.guid());
                editedSchedule(clone);

                okDialogHelper = function () {
                    delivery().IntervalScheduleCollection.push(editedSchedule());
                };

                $('#schedule-dialog').modal();
            },
            save = function () {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON(delivery);

                context.post(data, "/ReportDelivery/Save")
                    .then(function (result) {
                        delivery().ReportDeliveryId(result);
                        tcs.resolve(result);
                        logger.info("Schedule is saved");
                    });
                return tcs.promise();
            };


        var vm = {
            isBusy: isBusy,
            delivery: delivery,
            activate: activate,
            viewAttached: viewAttached,
            editedSchedule: editedSchedule,

            okDialog: okDialog,
            scheduleOptions: ko.observableArray(),
            startAddSchedule: startAddSchedule,
            editSchedule: editSchedule,
            userOptions : ko.observableArray(),
            toolbar: {
                saveCommand: save
            }
        };

        return vm;

    });

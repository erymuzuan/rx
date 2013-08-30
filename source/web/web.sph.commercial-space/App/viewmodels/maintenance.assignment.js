/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'durandal/plugins/router'],
    function (context,router) {

        var maintenance = ko.observable(new bespoke.sphcommercialspace.domain.Maintenance()),
            isBusy = ko.observable(false),
            id = ko.observable(),
            activate = function (routeData) {
                id(routeData.id);
                var maintenanceQuery = String.format("MaintenanceId eq {0}", id());
                var tcs = new $.Deferred();

                var maintenanceTask = context.loadOneAsync("Maintenance", maintenanceQuery);
                var templateTask = context.loadAsync("MaintenanceTemplate", "IsActive eq 1");

                $.when(maintenanceTask, templateTask).then(function (m, tlo) {
                    isBusy(false);
                    vm.maintenance(m);
                    vm.templates(tlo.itemCollection);
                    
                    var commands = _(tlo.itemCollection).map(function (t) {
                        return {
                            caption: ko.observable(t.Name()),
                            icon: "icon-gear",
                            command: function () {
                                var url = '/#/maintenance.detail-templateId.' + t.MaintenanceTemplateId() + "/" + t.MaintenanceTemplateId() + "/0";
                                router.navigateTo(url);
                                return {
                                    then: function () { }
                                };
                            }
                        };
                    });

                    vm.toolbar.groupCommands([ko.observable(
                        {
                            caption: ko.observable("Keluarkan Arahan Kerja"),
                            commands: ko.observableArray(commands)
                        })
                    ]);

                    tcs.resolve(true);
                });

                return tcs.promise();
            },

            saveAssignment = function () {
                var tcs = new $.Deferred();
                var data = ko.toJSON({ officer: vm.maintenance().Officer, id: id() });
                isBusy(true);

                context.post(data, "/Maintenance/Assign")
                    .then(function (result) {
                        isBusy(false);
                        tcs.resolve(result);
                        $('#success-panel').modal({})
                         .on('hidden', function () {
                             var url = '/#/maintenance.list';
                             router.navigateTo(url);
                         });
                    });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            maintenance: maintenance,
            templates: ko.observableArray([]),
            saveAssignmentCommand: saveAssignment,
            toolbar: {
                groupCommands: ko.observableArray(),
                reloadCommand: function () {
                    return activate();
                },
                printCommand: ko.observable({
                    entity: ko.observable("Maintenance"),
                    id: ko.observable(0),
                    item: maintenance,
                })
            }
        };

        return vm;

    });

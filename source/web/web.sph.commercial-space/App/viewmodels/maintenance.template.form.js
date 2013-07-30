/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext'],
    function (context) {

        var isBusy = ko.observable(false),
            templateId = ko.observable(),
            activate = function (routeData) {
                var id = parseInt(routeData.id);
                templateId(id);
                if (id) {
                    var query = String.format("MaintenanceTemplateId eq {0}", templateId());
                    var tcs = new $.Deferred();
                    context.loadOneAsync("MaintenanceTemplate", query)
                        .done(function (b) {
                            vm.maintenanceTemplate(b);
                            tcs.resolve(true);
                        });

                    return tcs.promise();
                } else {
                    vm.maintenanceTemplate(new bespoke.sphcommercialspace.domain.MaintenanceTemplate());
                    return true;
                }
            },
            addCustomField = function () {
                var customfield = new bespoke.sphcommercialspace.domain.CustomField();
                vm.maintenanceTemplate().CustomFieldCollection.push(customfield);
            },
            removeCustomField = function (customfield) {
                vm.maintenanceTemplate().CustomFieldCollection.remove(customfield);
            },
            save = function () {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON({ template: vm.maintenanceTemplate });
                isBusy(true);

                context.post(data, "/Template/SaveMaintenanceTemplate")
                    .then(function (result) {
                        isBusy(false);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            maintenanceTemplate: ko.observable(new bespoke.sphcommercialspace.domain.MaintenanceTemplate()),
            addCustomField: addCustomField,
            removeCustomField: removeCustomField,
            toolbar: {
                saveCommand: save
            }
        };

        return vm;

    });

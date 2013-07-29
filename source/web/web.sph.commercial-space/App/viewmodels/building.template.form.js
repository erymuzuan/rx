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
                    var query = String.format("BuildingTemplateId eq {0}", templateId());
                    var tcs = new $.Deferred();
                    context.loadOneAsync("BuildingTemplate", query)
                        .done(function (b) {
                            vm.buildingTemplate(b);
                            tcs.resolve(true);
                        });

                    return tcs.promise();
                } else {
                    vm.buildingTemplate(new bespoke.sphcommercialspace.domain.BuildingTemplate());
                    return true;
                }
            },
            addCustomField = function () {
                var customfield = new bespoke.sphcommercialspace.domain.CustomField();
                vm.buildingTemplate().CustomFieldCollection.push(customfield);
            },
            removeCustomField = function (customfield) {
                vm.buildingTemplate().CustomFieldCollection.remove(customfield);
            },
            save = function () {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON({ buildingTemplate: vm.buildingTemplate });
                isBusy(true);

                context.post(data, "/Template/SaveBuildingTemplate")
                    .then(function (result) {
                        isBusy(false);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            buildingTemplate: ko.observable(new bespoke.sphcommercialspace.domain.BuildingTemplate()),
            addCustomField: addCustomField,
            removeCustomField: removeCustomField,
            saveCommand: save
        };

        return vm;

    });

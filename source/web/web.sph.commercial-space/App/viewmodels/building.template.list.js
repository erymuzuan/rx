    define(['services/datacontext'], function (context) {
        var isBusy = ko.observable(false),
            activate = function () {
                var query = String.format("BuildingTemplateId gt 0");
                var tcs = new $.Deferred();
                context.loadAsync("BuildingTemplate", query)
                    .then(function(lo) {
                        isBusy(false);
                        vm.buildingTemplates(lo.itemCollection);
                        
                        tcs.resolve(true);
                    });
                return tcs.promise();
            };
            

        var vm = {
            activate: activate,
            buildingTemplates: ko.observableArray(),
            toolbar : {
                addNew : {
                    location: '/#/building.template.form/0',
                    caption : 'Tambah Template bangunan'
                }
            }
        };

        return vm;
    });

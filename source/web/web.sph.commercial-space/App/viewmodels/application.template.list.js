define(['services/datacontext'], function (context) {
    var isBusy = ko.observable(false),
        activate = function () {
            var query = String.format("ApplicationTemplateId gt 0");
            var tcs = new $.Deferred();
            context.loadAsync("ApplicationTemplate", query)
                .then(function (lo) {
                    isBusy(false);
                    vm.applicationTemplates(lo.itemCollection);

                    tcs.resolve(true);
                });
            return tcs.promise();
        };


    var vm = {
        activate: activate,
        applicationTemplates: ko.observableArray(),
        toolbar : {
            addNew : {
                location: '/#/template.application-id.0/0',
                caption :'tambah jenis permohonan'
            }
        }
    };

    return vm;
});

define(['services/datacontext', 'durandal/plugins/router'], function (context,router) {

    var title = ko.observable('Senarai Ruang Komersil'),
        isBusy = ko.observable(false),
        activate = function() {
            var tcs = new $.Deferred();
            var templateTask = context.loadAsync("CommercialSpaceTemplate", "IsActive eq 1");
            var csTask = context.loadAsync("CommercialSpace", "CommercialSpaceId gt 0");
            
            $.when(templateTask, csTask).done(function (tlo, lo) {
                
                var commands = _(tlo.itemCollection).map(function(t) {
                    return {
                        caption: ko.observable("" + t.Name()),
                        icon: "icon-plus",
                        command : function() {
                            var url = '/#/commercialspace.detail/' + t.CommercialSpaceTemplateId() + "/0/-/0";
                            router.navigateTo(url);
                            return {
                                then: function () { }
                            };
                        }
                    };
                });
                vm.toolbar.commands(commands);
                vm.commercialspaces(lo.itemCollection);
                tcs.resolve(true);
            });
           return tcs.promise();
        };

    var vm = {
        title: title,
        activate: activate,
        isBusy : isBusy,
        commercialspaces: ko.observableArray([]),
        toolbar : {
            commands: ko.observableArray()
        }
    };

    return vm;
});
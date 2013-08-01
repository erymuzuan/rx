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
                            var url = '/#/commercialspace.detail-templateid.' + t.CommercialSpaceTemplateId() + "/" + t.CommercialSpaceTemplateId() + "/0/-/0";
                            router.navigateTo(url);
                            return {
                                then: function () { }
                            };
                        }
                    };
                });
                vm.toolbar.groupCommands([ko.observable(
                {
                    caption: ko.observable("Ruang Komersil Baru"),
                    commands: ko.observableArray(commands)
                })
                ]);
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
        toolbar: {
            groupCommands: ko.observableArray()
        }
    };

    return vm;
});
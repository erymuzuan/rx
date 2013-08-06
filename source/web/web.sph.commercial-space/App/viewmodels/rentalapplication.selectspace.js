/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'durandal/plugins/router'], function (context, router) {

    var activate = function () {
            var tcs = new $.Deferred();
            var templateTasks = context.loadAsync("ApplicationTemplate", "IsActive eq 1");
            var csTasks = context.loadAsync("CommercialSpace", "IsAvailable eq 1 and IsOnline eq 1");
            $.when(templateTasks, csTasks)
                .done(function (templates, spaces) {
                    var items = _(templates.itemCollection).map(function (t) {
                        var filtered = _(spaces.itemCollection).filter(function(c) {
                            return c.ApplicationTemplateOptions().indexOf(t.ApplicationTemplateId()) > -1;
                        });
                        return {
                            name: t.Name(),
                            id :t.ApplicationTemplateId(),
                            spaces: filtered
                        };
                    });
                    vm.items(items);
                    tcs.resolve(true);

                });


            return tcs.promise();
        };

    var vm = {
        activate: activate,
        items: ko.observableArray([])
    };

    return vm;
});
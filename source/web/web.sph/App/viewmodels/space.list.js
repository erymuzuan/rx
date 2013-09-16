/// <reference path="../services/cultures.my.js" />
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/loadoperation.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="/App/schemas/sph.domain.g.js" />

define([objectbuilders.datacontext, objectbuilders.router, objectbuilders.cultures],
    function (context, router, cultures) {

    var title = ko.observable(cultures.space.title),
        isBusy = ko.observable(false),
        activate = function() {
            var tcs = new $.Deferred();
            var templateTask = context.loadAsync("SpaceTemplate", "IsActive eq 1");
            var csTask = context.loadAsync("Space", "SpaceId gt 0");
            
            $.when(templateTask, csTask).done(function (tlo, lo) {
                
                var commands = _(tlo.itemCollection).map(function(t) {
                    return {
                        caption: ko.observable("" + t.Name()),
                        icon: "icon-plus",
                        command : function() {
                            var url = '/#/space.detail-templateid.' + t.SpaceTemplateId() + "/" + t.SpaceTemplateId() + "/0/-/0";
                            router.navigateTo(url);
                            return {
                                then: function () { }
                            };
                        }
                    };
                });
                vm.toolbar.groupCommands([ko.observable(
                {
                    caption: ko.observable(cultures.space.toolbar.ADD_NEW_SPACE),
                    commands: ko.observableArray(commands)
                })
                ]);
                vm.spaces(lo.itemCollection);
                tcs.resolve(true);
            });
           return tcs.promise();
        };

    var vm = {
        title: title,
        activate: activate,
        isBusy : isBusy,
        spaces: ko.observableArray([]),
        toolbar: {
            groupCommands: ko.observableArray()
        },
        cultures : cultures
    };

    return vm;
});
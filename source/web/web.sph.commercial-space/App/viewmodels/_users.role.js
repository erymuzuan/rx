/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger', 'durandal/plugins/router'],
    function(context, logger, router) {

        var isBusy = ko.observable(false),
            activate = function (roles) {
                vm.roles.removeAll();
                _.each(roles, function (v, i) {
                    var r = { RoleName: ko.observable(''), Index: ko.observable() };
                    r.Index(i + 1);
                    r.RoleName(v);
                    vm.roles.push(r);
                });
            },
            viewAttached = function(view) {

            },
            saveRole = function () {
                var tcs = new $.Deferred();
                var data = JSON.stringify({ role: vm.role() });
                isBusy(true);

                context.post(data, "/Admin/AddRole")
                    .then(function (result) {
                        isBusy(false);
                        var r = { RoleName: ko.observable(''), Index: ko.observable() };
                        var index = _.max(vm.roles(), function (list) {
                            return parseFloat(list.Index()) + 1;
                        });
                        r.RoleName(vm.role());
                        r.Index(index.Index() + 1);
                        vm.roles.push(r);
                        vm.role('');

                        tcs.resolve(result);
                    });
                return tcs.promise();
            };;

        var vm = {
            isBusy: isBusy,
            activate: activate,
            viewAttached: viewAttached,
            role: ko.observable(''),
            roles: ko.observableArray(),
            saveroleCommand: saveRole
        };

        return vm;

    });

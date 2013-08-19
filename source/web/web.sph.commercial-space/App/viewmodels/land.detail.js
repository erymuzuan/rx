/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'config','viewmodels/map'],
    function (context, config,map) {

        var isBusy = ko.observable(false),
            activate = function (routeData) {
                var id = parseInt(routeData.id);
                if (id === 0) {
                    vm.land(new bespoke.sphcommercialspace.domain.Land());
                    return true;
                }
                var query = "LandId eq " + id;
                var tcs = new $.Deferred();

                context.loadOneAsync("Land", query)
                    .then(function (land) {
                        vm.land(land);
                        tcs.resolve(true);
                    });
                return tcs.promise();


            },
            viewAttached = function (view) {

            },
            save = function () {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON(vm.land);

                context.post(data, "/Land/Save")
                    .then(function (result) {
                        vm.land().LandId(result);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
            saveMap = function() {
                
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            viewAttached: viewAttached,
            land: ko.observable(),
            saveMapCommand : saveMap,
            toolbar: {
                saveCommand: save
            }
        };

        return vm;

    });

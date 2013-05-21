/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/__common.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />

define(['services/datacontext', 'services/logger', 'durandal/plugins/router', 'durandal/app', 'viewmodels/map'],
    function (context, logger, router, app, map) {

        var title = ko.observable(''),
            buildingId = ko.observable(),
            mapLoaded = false,
            isBusy = ko.observable(false),
            floorname = ko.observable(),
            activate = function (routeData) {

                buildingId(routeData.buildingId);
                floorname(routeData.floorname);

                title('Pelan lantai : ' + floorname());
                var tcs = new $.Deferred();
                var buildingTask = context.loadOneAsync('Building', 'BuildingId eq ' + buildingId());
                var centerTask = $.get('/Building/GetCenter/'+ routeData.buildingId);
                $.when(buildingTask, centerTask)
                    .then(function (b, center) {
                        if (!b) {
                            var lot = {
                                Name: ko.observable(''),
                                Size: ko.observable(''),
                                IsCommercialSpace: ko.observable(true)
                            };
                            vm.floor.LotCollection.push(lot);
                        }
                        var flo = $.grep(b.FloorCollection(), function (x) { return x.Name() === floorname(); })[0];
                        vm.floor.LotCollection(flo.LotCollection());
                        vm.floor.Name(flo.Name());
                        vm.floor.Size(flo.Size());
                        tcs.resolve(true);

                        setTimeout(function () {
                            var panel = document.getElementById('floorplanmap');
                            if (panel && !mapLoaded) {
                                map.init({ panel: 'floorplanmap', draw: true , center : center, zoom: 18});
                                mapLoaded = true;
                            }
                        }, 3000);
                    });

                return tcs.promise();
        },
        save = function () {
            var tcs = new $.Deferred();
            var data = JSON.stringify({
                floor: ko.mapping.toJS(vm.floor),
                buildingId: buildingId(),
                floorname: floorname()
            });
            isBusy(true);
            context.post(data, "/Building/AddLot").done(function (e) {
                logger.log("Data has been successfully saved ", e, "buildingdetail", true);

                isBusy(false);
                tcs.resolve(true);
            });

            return tcs.promise();
        },

        goBack = function () {
            var url = "/#/buildingdetail/" + buildingId();
            router.navigateTo(url);
        };

        var vm = {
            activate: activate,
            title: title,
            floor: {
                Name: ko.observable(''),
                Size: ko.observable(''),
                LotCollection: ko.observableArray([])
            },
            goBackCommand: goBack,
            saveCommand: save,
            isBusy: isBusy
        };

        return vm;
    });
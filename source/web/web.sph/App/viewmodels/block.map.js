/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../../Scripts/google-maps-3-vs-1-0-vsdoc.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', 'durandal/plugins/router', 'viewmodels/map'],
    function (context, logger, router, map) {

        var isBusy = ko.observable(false),
            activate = function (storeId) {
                vm.spatialStoreId(storeId);

            },
            init = function (buildingId, storeId) {
                window.setTimeout(function () {

                    $.get("/Building/GetCenter/" + buildingId)
                      .done(function (e) {
                          var point = new google.maps.LatLng(e.Lat, e.Lng);
                          map.init({
                              panel: 'block-map-panel',
                              draw: true,
                              zoom: 18,
                              center: point
                          });
                      });

                }, 1000);

            },
            viewAttached = function (view) {

            },
            okClick = function () {
                this.modal.close("OK");
            },
            cancelClick = function () {
                this.modal.close("Cancel");
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            viewAttached: viewAttached,
            spatialStoreId: ko.observable(),
            okClick: okClick,
            cancelClick: cancelClick,
            init: init,
            buildingId: ko.observable()
        };

        return vm;

    });

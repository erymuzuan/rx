/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../../Scripts/google-maps-3-vs-1-0-vsdoc.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../viewmodels/map.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', 'durandal/plugins/router', 'viewmodels/map'],
    function (context, logger, router, map) {

        var isBusy = ko.observable(false),
            id = ko.observable(),
            activate = function (storeId) {
                vm.spatialStoreId(storeId);

            },
            polygon = null,
            polygoncomplate = function (shape) {
                polygon = shape;
            },
            init = function (buildingId, spatialId) {
                vm.spatialStoreId(spatialId);
                id(buildingId);

                // time out to allow for the UI to render
                window.setTimeout(function () {

                    $.get("/Building/GetCenter/" + buildingId)
                      .done(function (e) {
                          map.init({
                              panel: 'block-map-panel',
                              draw: true,
                              zoom: 18,
                              polygoncomplete: polygoncomplate
                          }).done(function () {
                              map.setCenter(e.Lat, e.Lng);

                              context.getScalarAsync("SpatialStore", String.format("StoreId eq '{0}'", spatialId), "EncodedWkt")
                                  .done(function (path) {
                                      if (!path) return;
                                      var shape = map.add({
                                          encoded: path,
                                          draggable: true,
                                          editable: true,
                                          zoom: 18
                                      });
                                      if (shape.type === 'marker') {
                                          // pointMarker = shape;
                                      }

                                      if (shape.type === 'polygon') {
                                          polygon = shape;
                                      }

                                  });

                          });
                      });

                }, 1000);

            },
            viewAttached = function (view) {

            },
            okClick = function () {
                // save the spatial
                var tcs = new $.Deferred(),
                    data = JSON.stringify(
                        {
                            "EncodedPath": map.getEncodedPath(polygon),
                            "Tag": id().toString(),
                            "Type": "Block"
                        }),
                    modal = vm.modal;

                context.post(data, "/Map/Create")
                    .then(function (result) {
                        tcs.resolve(result);
                        vm.spatialStoreId(result);
                        modal.close("OK");
                    });
                return tcs.promise();
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

/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', 'durandal/plugins/router','viewmodels/map'],
    function(context, logger, router, map) {

        var isBusy = ko.observable(false),
            activate = function(storeId) {
                vm.spatialStoreId(storeId);
                
            },
            viewAttached = function(view) {
                map.init({
                    panel: 'block-map-panel',
                    draw: true,
                    zoom: 18
                });
            },
            okClick = function() {
                this.modal.close("OK");
            },
            cancelClick = function() {
                this.modal.close("Cancel");
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            viewAttached: viewAttached,
            spatialStoreId: ko.observable(),
            okClick: okClick,
            cancelClick: cancelClick
        };

        return vm;

    });

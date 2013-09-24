/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/cultures.my.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../objectbuilders.js" />

define(['services/datacontext', 'services/logger', './_space.contract', 'durandal/system', 'config', objectbuilders.cultures, objectbuilders.map],
    function (context, logger, contractlistvm, system, config, cultures,map) {

        var title = ko.observable(),
            activate = function (routeData) {
                var id = parseInt(routeData.id),
                    tcs = new $.Deferred();
                context.loadOneAsync("Space", "SpaceId eq " + id).done(
                    function(space) {
                        vm.space(space);
                        tcs.resolve(true);
                        
                        // load map
                        map.init({
                            panel: 'map-space'
                            
                        });
                    });

              

                return tcs.promise();
            },
            viewAttached = function (view) {
                $(view).tooltip({ 'placement': 'right' });
            };

        var vm = {
            activate: activate,
            title: title,
            viewAttached: viewAttached,
            space: ko.observable(new bespoke.sph.domain.Space()),
            building : ko.observable(new bespoke.sph.domain.Building())
        };

    

        return vm;
    });
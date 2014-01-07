/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger', 'plugins/router'],
    function(context, logger, router) {

        var
            entities = ko.observableArray(),
            isBusy = ko.observable(false),
            activate = function() {

            },
            attached = function(view) {

            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            entities: entities,
            toolbar : {
                addNew: {
                    location: '#/entity.details/0',
                    caption: 'Add New Custom Entity'
                }
            }
        };

        return vm;

    });

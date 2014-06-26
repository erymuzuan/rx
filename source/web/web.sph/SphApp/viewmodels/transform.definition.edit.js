/// <reference path="../../Scripts/jquery-2.1.0.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />


define(['services/datacontext', 'services/logger', objectbuilders.system],
    function (context, logger, system) {

        var td = ko.observable(new bespoke.sph.domain.TransformDefinition(system.guid())),
            isBusy = ko.observable(false),
            activate = function () {

            },
            attached = function (view) {

            },
            save = function() {

                return Task.fromResult(0);
            },
            editProp = function() {
                return Task.fromResult(0);
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            td: td,
            toolbar : {
                saveCommand: save,
                commands :ko.observableArray([
                {
                    command: editProp,
                    caption: 'Edit Properties',
                    icon : 'fa fa-table'
                }])
            }
        };

        return vm;

    });

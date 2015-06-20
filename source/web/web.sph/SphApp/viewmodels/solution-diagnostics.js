/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(["services/datacontext", "services/logger", "plugins/router"],
    function(context, logger, router) {

        var
            isBusy = ko.observable(false),
            activate = function() {

            },
            attached = function(view) {

            },
            startDiagnostics = function() {
                return context.post(ko.toJSON({}), "/solution/diagnostics");
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            toolbar : {
                commands: ko.observableArray( [
                    {
                        command: startDiagnostics,
                        caption: "Start Diagnostics",
                        icon : "fa fa-file-o"
                    }
                ])
            }
        };

        return vm;

    });

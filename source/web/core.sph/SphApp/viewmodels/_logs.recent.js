/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(["services/datacontext", "services/logger", "plugins/router"],
    function(context, logger, router) {

        const list = ko.observableArray(),
            severityOptions = ko.observableArray(),
            isBusy = ko.observable(false),
            activate = function() {
                return true;
            },
            attached = function() {
                return true;
            },
            openDetails = function (log) {
                require(["viewmodels/log.details.dialog", "durandal/app"], function (dialog, app2) {
                    dialog.log(log);
                  
                    app2.showDialog(dialog)
                        .done(function () {});

                });
            },
            query = {
                "sort": [
                    {
                        "time": {
                            "order": "desc"
                        }
                    }
                ]
            };

        const vm = {
            openDetails: openDetails,
            severityOptions: severityOptions,
            query: query,
            list: list,
            isBusy: isBusy,
            activate: activate,
            attached: attached
        };

        return vm;

    });

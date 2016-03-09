/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../../core.sph/Scripts/require.js" />
/// <reference path="../../../core.sph/SphApp/schemas/form.designer.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(["services/datacontext", "services/logger", "plugins/router"],
    function(context, logger, router) {

        var route = ko.observable(),
            security = ko.observable({
                IsSecurityEnabled: ko.observable(),
                ClaimCollection: ko.observableArray(),
                addClaim : function() {
                    security().ClaimCollection.push({
                        Claim: ko.observable(),
                        Value : ko.observable()
                    });
                },
                removeClaim : function(c) {
                    security().ClaimCollection.remove(c);
                }
            }),
            isBusy = ko.observable(false),
            activate = function(r) {
                route(r);
            },
            attached = function(view) {

            };

        var vm = {
            security: security,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            route: route
        };

        return vm;

    });

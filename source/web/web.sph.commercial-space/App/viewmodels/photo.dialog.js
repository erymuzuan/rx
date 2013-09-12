/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', 'durandal/plugins/router'],
    function(context, logger, router) {

        var isBusy = ko.observable(false),
            activate = function() {

            },
            viewAttached = function(view) {

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
            photo: ko.observable(new bespoke.sphcommercialspace.domain.Photo()),
            okClick: okClick,
            cancelClick : cancelClick
        };
        

        return vm;

    });

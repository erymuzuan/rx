/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger', 'durandal/plugins/router'],
    function(context, logger, router) {

        var isBusy = ko.observable(false),
            activate = function() {

            },
            viewAttached = function(view) {
                $("#contractTypeTemplate").kendoUpload({
                    async: {
                        saveUrl: "/BinaryStore/Upload",
                        removeUrl: "/BinaryStore/Remove",
                        autoUpload: true
                    },
                    multiple: false,
                    error: function(e) {
                        logger.logError(e, e, this, true);
                    },
                    success: function(e) {
                        logger.log('Your file has been ' + e.operation, e, this, true);
                        var storeId = e.response.storeId;
                        var uploaded = e.operation === "upload";
                        var removed = e.operation != "upload";
                        if (uploaded) {
                            vm[e.sender.element.attr("Id")](storeId);
                        }

                        if (removed) {
                            vm[e.sender.element.attr("Id")]("");
                        }
                        

                    }
                });
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            contractTypeTemplate : ko.observable(),
            viewAttached: viewAttached
        };

        return vm;

    });

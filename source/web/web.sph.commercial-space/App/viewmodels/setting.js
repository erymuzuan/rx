/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger'],
    function (context, logger) {

        var isBusy = ko.observable(false),
            activate = function () {
            },
            viewAttached = function () {
                $("#returnLetterTemplate,#offerLetterTemplate").kendoUpload({
                    async: {
                        saveUrl: "/BinaryStore/Upload",
                        removeUrl: "/BinaryStore/Remove",
                        autoUpload: true
                    },
                    multiple: false,
                    error: function (e) {
                        logger.logError(e, e, this, true);
                    },
                    success: function (e) {
                        logger.log('Your file has been ' + e.operation, e, this, true);
                        var storeId = e.response.storeId;
                        var uploaded = e.operation === "upload";
                        var removed = e.operation != "upload";

                        if (uploaded) {
                            vm[e.sender.element.attr("id")](storeId);
                        }

                        if (removed) {
                            vm[e.sender.element.attr("id")]("");
                        }

                    }
                });
            },

                save = function () {

                    var tcs = new $.Deferred();
                    var data = JSON.stringify({
                        settings: [{
                            Key: "Template.Returned.Letter",
                            Value: vm.returnLetterTemplate()
                        },
                            {
                                Key: "Template.Offer.Letter",
                                Value: vm.offerLetterTemplate()
                            }]
                    });
                    isBusy(true);

                    context.post(data, "/Setting/Save")
                        .then(function (result) {
                            isBusy(false);
                            logger.log('All the setting has been saved', result, this, true);
                            tcs.resolve(result);
                        });
                    return tcs.promise();
                };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            viewAttached: viewAttached,
            returnLetterTemplate: ko.observable(''),
            offerLetterTemplate: ko.observable(''),
            saveCommand: save
        };

        return vm;

    });

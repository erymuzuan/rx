define(['services/datacontext', 'services/logger', 'durandal/plugins/router', 'durandal/app'], function(context, logger, router, app) {

    var activate = function() {
        logger.log('Rental Application View Activated', null, 'commercialspace', true);
        return true;
    },
        save = function () {
            var tcs = new $.Deferred();
            var data = ko.mapping.toJSON(vm.rentalapplication);
            isBusy(true);
            context.post(data, "/RentalApplication/SaveRentalApplication").done(function (e) {
                logger.log("Data has been successfully saved ", e, "rentalapplication", true);
                isBusy(false);
                tcs.resolve(true);
            });
            return tcs.promise();
        };
    var vm = {
        activate: activate,
        saveCommand:save,
        rentalapplication : {
            CompanyName: ko.observable(''),
            CompanyRegistrationNo:ko.observable(''),
            DateStart:ko.observable(),
            DateEnd:ko.observable(),
            Purpose:ko.observable(''),
            CompanyType: ko.observable(''),
            Address: {
                Street: ko.observable(),
                City: ko.observable(),
                State: ko.observable(),
                Postcode: ko.observable(),
            },
            
        },
        
    };

    return vm;
});
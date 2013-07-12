define(['services/datacontext', ], function (context) {
    var isBusy = ko.observable(false),
        activate = function () {

        },

        register = function () {
            var tcs = new $.Deferred();
            var data = ko.toJSON(vm.profile);
            isBusy(true);

            context.post(data, "/Admin/Register")
                .then(function(result) {
                    isBusy(false);
                    
                    tcs.resolve(result);
                });
            return tcs.promise();
        };


    var vm = {
        activate: activate,
        isBusyValidatingUserName: ko.observable(false),
        profile: {
            FullName: ko.observable(),
            UserName: ko.observable(),
            Email: ko.observable(),
            Password: ko.observable(),
            ConfirmPassword: ko.observable(''),
            Status: ko.observable(),
            Designation: ko.observable(),
            Department: ko.observable(),
            Telephone: ko.observable(),
            Mobile: ko.observable(),
            IsNew: ko.observable()
        },
        userNameValidationStatus: ko.observable(),
        submitCommand: register
    };
    vm.profile.UserName.subscribe(function (username) {
        vm.isBusyValidatingUserName(true);
        var tcs = new $.Deferred();
        var data = JSON.stringify({ userName: username });
        isBusy(true);
        context.post(data, "/Admin/ValidateUserName")
            .then(function (result) {
                isBusy(false);
                vm.isBusyValidatingUserName(false);
                if (result.status !== "OK") {
                    vm.userNameValidationStatus(result.message);
                }

                tcs.resolve(result);
            });
        return tcs.promise();
    });
    return vm;
});
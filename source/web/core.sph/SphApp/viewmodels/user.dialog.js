/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(["plugins/dialog", "services/datacontext"],
    function (dialog, context) {

        var profile = ko.observable(new bespoke.sph.domain.UserProfile()),
            departmentOptions = ko.observableArray(),
            designationOptions = ko.observableArray(),
            userNameValidationStatus = ko.observable(""),
            isBusy = ko.observable(false),
            passwordValidationStatus = ko.observable(true),
            emailValidationStatus = ko.observable(""),
            isBusyValidatingUserName = ko.observable(false),
            isBusyValidatingEmail = ko.observable(false),
            activate = function () {
                var designationTask = context.getListAsync("Designation", "Id ne '0'", "Name");
                var departmentTask = context.loadOneAsync("Setting", "Key eq 'Departments'");
                return $.when(designationTask, departmentTask)
                  .then(function (s, d) {
                      if (s) {
                          designationOptions(s);
                      }
                      if (d) {
                          var departments = JSON.parse(ko.mapping.toJS(d.Value));;
                          departmentOptions(departments);
                      }

                  });
            },
            userNameChaged = function (userName) {
                isBusyValidatingUserName(true);
                var tcs = new $.Deferred();
                var data = JSON.stringify({ userName: userName });
                isBusy(true);
                context.post(data, "/sph/Admin/ValidateUserName")
                    .then(function (result) {
                        isBusy(false);
                        isBusyValidatingUserName(false);
                        if (result.status !== "OK") {
                            userNameValidationStatus(result.message);
                        }

                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
            passwordChanged = function (pwd) {
                var data = JSON.stringify({ password: pwd });
                return context.post(data, "/sph/Admin/CheckPasswordComplexity")
                    .then(function (result) {
                        passwordValidationStatus(result);
                    });
            },
            emailChanged = function (email) {
                isBusyValidatingEmail(true);
                var tcs = new $.Deferred();
                var data = JSON.stringify({ email: email });
                isBusy(true);
                context.post(data, "/Sph/Admin/ValidateEmail")
                    .then(function (result) {
                        isBusy(false);
                        isBusyValidatingEmail(false);
                        if (result.status !== "OK") {
                            emailValidationStatus(result.message);
                        }

                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
            attached = function (view) {
                if (ko.unwrap(profile().IsNew)) {

                    profile().UserName("");
                    profile().Email.subscribe(emailChanged);
                    profile().UserName.subscribe(userNameChaged);
                    profile().Password.subscribe(passwordChanged);

                    $(view).find("input[type=password]").attr("required","required");
                } else {
                    $(view).find("input[type=password]").removeAttr("required");
                }
            },
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };

        var vm = {
            activate: activate,
            attached: attached,
            isBusy: isBusy,
            profile: profile,
            isBusyValidatingUserName: isBusyValidatingUserName,
            isBusyValidatingEmail: isBusyValidatingEmail,
            designationOptions: designationOptions,
            departmentOptions: departmentOptions,
            userNameValidationStatus: userNameValidationStatus,
            emailValidationStatus: emailValidationStatus,
            passwordValidationStatus: passwordValidationStatus,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });

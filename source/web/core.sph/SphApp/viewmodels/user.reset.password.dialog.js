/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(["plugins/dialog", "services/logger", "services/datacontext"],
    function (dialog, logger, context) {

        var password1 = ko.observable(),
            password2 = ko.observable(),
            passwordValidationStatus = ko.observable(),
            passwordChanged = function (pwd) {
                var data = JSON.stringify({ password: pwd });
                return context.post(data, "/sph/Admin/CheckPasswordComplexity")
                    .then(function (result) {
                        passwordValidationStatus(result);
                    });
            },
            activate = function () {
                password1.subscribe(passwordChanged);
            },
            attached = function (view) {
                password1("");
                password2("");
                setTimeout(function () {
                    $(view).find("#password1").focus();
                }, 500);
            },
            okClick = function (data, ev) {
                if (!password1() && !password2()) {
                    return;
                };
                if (password1() !== password2()) {
                    logger.logError("Password mismatch", this, this, true);
                    return;
                }

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
            password1: password1,
            password2: password2,
            passwordValidationStatus: passwordValidationStatus,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });

/// <reference path="../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(["plugins/dialog", objectbuilders.datacontext],
    function (dialog, context) {

        var username = ko.observable(),
            expiry = ko.observable(),
            useroptions = ko.observableArray(),
            token = ko.observable({
                "grant_type": "admin",
                "username": username,
                "expiry": expiry
            }),
            activate = function () {
                var query = String.format("Id ne '{0}'", "0"),
                    tcs = new $.Deferred();

                context.getListAsync("UserProfile", query, "UserName")
                    .then(function (lo) {
                        useroptions(lo);
                        tcs.resolve(true);
                    });
                return tcs.promise();

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
            username: username,
            expiry: expiry,
            token: token,
            activate: activate,
            useroptions: useroptions,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });

/// <reference path="../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(["plugins/dialog"],
    function (dialog) {

        var route = ko.observable({
            "role": ko.observable(),
            "groupName": ko.observable(),
            "route": ko.observable(),
            "moduleId": ko.observable(),
            "title": ko.observable(),
            "nav": ko.observable(),
            "icon": ko.observable(),
            "caption": ko.observable(),
            "settings": ko.observable(),
            "showWhenLoggedIn": ko.observable(),
            "isAdminPage": ko.observable(),
            "startPageRoute": ko.observable()
        }),
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };

        var vm = {
            route: route,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });

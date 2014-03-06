/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(['plugins/dialog', objectbuilders.config, objectbuilders.system],
    function (dialog, config, system) {

        var member = ko.observable(new bespoke.sph.domain.Member()),
            activate = function () {
                var permissions = _(config.roles).map(function (v) {
                    return new bespoke.sph.domain.FieldPermission({ Role: v, WebId: system.guid() });
                });
                if (typeof member().FieldPermissionCollection === "undefined") {
                    member().FieldPermissionCollection = ko.observableArray(permissions);
                } else {
                    member().FieldPermissionCollection(permissions);
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
            member: member,
            activate: activate,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });

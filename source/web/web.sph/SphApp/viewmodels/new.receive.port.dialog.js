/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../../../core.sph/SphApp/schemas/__domain.js" />
/// <reference path="../../../core.sph/SphApp/objectbuilders.js" />
/// <reference path="../../../core.sph/Scripts/_task.js" />
/// <reference path="../../../core.sph/Scripts/require.js" />


define(["plugins/dialog", objectbuilders.datacontext, objectbuilders.system],
    function (dialog, context, system) {

        const port = ko.observable(new bespoke.sph.domain.ReceivePort(system.guid())),
            page = ko.observable(1),
            isDelimiter = ko.observable(false),
            isPositional = ko.observable(false),
            activate = function () {
                const formatter = port().TextFormatter();
                formatter.IsFixedLength.ForEditing = ko.computed({
                    read: function () {
                        return formatter.IsFixedLength().toString();
                    },
                    write: function (newValue) {
                        const ok = newValue === "true" || newValue === "on";
                        formatter.IsDelimited(!ok);
                        formatter.IsFixedLength(ok);
                    },
                    owner: this
                });
                formatter.IsDelimited.ForEditing = ko.computed({
                    read: function () {
                        return formatter.IsDelimited().toString();
                    },
                    write: function (newValue) {
                        const ok = newValue === "true" || newValue === "on";
                        formatter.IsDelimited(ok);
                        formatter.IsFixedLength(!ok);
                    },
                    owner: this
                });
            },
            backEnable = ko.computed(function () {
                return ko.unwrap(page) > 1;
            }),
            nextEnable = ko.computed(function () {
                return ko.unwrap(page) < 5;
            }),
            okEnable = ko.observable(false),
            backClick = function () {
                page(ko.unwrap(page) - 1);
            },
            nextClick = function () {
                page(ko.unwrap(page) + 1);
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
            port: port,
            backClick: backClick,
            nextClick: nextClick,
            okClick: okClick,
            backEnable: backEnable,
            nextEnable: nextEnable,
            okEnable: okEnable,
            isDelimiter: isDelimiter,
            isPositional: isPositional,
            page: page,
            cancelClick: cancelClick
        };


        return vm;

    });

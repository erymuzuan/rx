/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(['plugins/dialog'],
    function(dialog) {

        var table = ko.observable(new bespoke.sph.domain.api.TableDefinition()),
            activate = function () {
                var timestampColumn = _(table().ColumnCollection()).filter(function(v) {
                    return ko.unwrap(v.SqlType) === "Timestamp";
                }),
                modifiedDateColumnOptions = _(table().ColumnCollection()).filter(function(v) {
                    return ko.unwrap(v.ClrType).startsWith("System.DateTime");
                });
                table().versionColumnOptions = ko.observableArray(timestampColumn);
                table().modifiedDateColumnOptions = ko.observableArray(modifiedDateColumnOptions);
            },
            okClick = function(data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }
            },
            cancelClick = function() {
                dialog.close(this, "Cancel");
            };

        var vm = {
            table: table,
            okClick: okClick,
            activate: activate,
            cancelClick: cancelClick
        };


        return vm;

    });

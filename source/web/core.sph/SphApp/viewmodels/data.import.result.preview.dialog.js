/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/jquery.signalR-2.2.0.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(["plugins/dialog", objectbuilders.datacontext],
    function (dialog, context) {

        var model = ko.observable(),
            connection = $.connection.hub,
            hub = $.connection.dataImportHub,
            tableOptions = ko.observableArray(),
            previewResult = ko.observable(),
            error = ko.observable(),
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            },
            activate = function () {
                previewResult(null);
                error(null);

                return connection.start()
                    .then(function () {
                        return hub.server.preview(ko.mapping.toJS(model));
                    })
                     .then(function(result) {
                         if (!result.ItemCollection) {
                             error(result);
                             return;
                         } else {
                             previewResult(result);
                         }
                    });
            },
            attached = function (view) {

                if (ko.unwrap(error)) {
                    return;
                }
               
                var table = _(tableOptions()).find(function (v) { return v.Name === model().table(); }),
                    thead = "<tr>";
                _(ko.unwrap(table.MemberCollection)).each(function (v) {
                    thead += "<th>" + v.Name + "</th>";
                });
                thead += "</tr>";
                $("#thead").html(thead);

                var tbody = "";
                _(ko.unwrap(previewResult).ItemCollection).each(function (m) {
                    tbody += "<tr>";
                    _(ko.unwrap(table.MemberCollection)).each(function (v) {
                        tbody += "  <td>" + m[v.Name] + "</td>\r";
                    });
                    tbody += "</tr>\r";
                });
                $(view).find("#tbody").html(tbody);

            };

        var vm = {
            activate: activate,
            error: error,
            attached: attached,
            model: model,
            tableOptions: tableOptions,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });

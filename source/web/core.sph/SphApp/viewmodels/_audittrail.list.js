﻿/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../../Scripts/_uiready.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../services/domain.g.js.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="~/Scripts/_task.js" />
/// <reference path="~/Scripts/jquery.signalR-2.1.2.js" />

define(["services/datacontext"],
    function (context) {

        var isBusy = ko.observable(false),
            log = ko.observable(new bespoke.sph.domain.AuditTrail()),
            auditTrailCollection = ko.observableArray(),
            connection = null,
            ///
            activate = function (typeOrOption, id) {
                var type = typeOrOption;
                if (typeOrOption && typeof typeOrOption === "object") {
                    type = typeOrOption.entity;
                    id = typeOrOption.id;
                }
                if (!id) {
                    return Task.fromResult(false);
                }

                var query = String.format("Type eq '{0}' and EntityId eq '{1}'", type, id);

                connection = $.connection("/signalr_audittrail", "type=" + type + "&id=" + id);

                connection.received(function (data) {
                    console.log(data);
                    data.ChangeCollection = ko.observableArray(data.ChangeCollection);
                    auditTrailCollection.splice(0, 0, data);
                });

                connection.start().done(function (e) {
                    console.log("started...connection to message connection");
                    console.log(e);
                });

                return context.loadAsync({ entity: "AuditTrail", orderby: "CreatedDate desc" }, query)
                    .then(function (lo) {
                        isBusy(false);
                        auditTrailCollection(lo.itemCollection);
                    });

            },
            detached = function () {
                console.log("Disconnect from the server");
                return connection.stop();
            },
            attached = function () {

            },
            showChange = function (audit) {
                log(audit);
                $("#changes-dialog").modal();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            detached: detached,
            attached: attached,
            auditTrailCollection: auditTrailCollection,
            log: log,
            showChangesCommand: showChange
        };

        return vm;

    });

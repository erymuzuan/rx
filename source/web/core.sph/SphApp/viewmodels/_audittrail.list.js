/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
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

define(['services/datacontext'],
    function (context) {

        var isBusy = ko.observable(false),
            ///
            activate = function (typeOrOption, id) {
                var type = typeOrOption;
                if (typeOrOption && typeof typeOrOption === "object") {
                    type = typeOrOption.entity;
                    id = typeOrOption.id;
                }
                if (!parseInt(id)) {
                    return Task.fromResult(false);
                }

                var query = String.format("Type eq '{0}' and EntityId eq '{1}'", type, id),
                 tcs = new $.Deferred();

                context.loadAsync({ entity: "AuditTrail", orderby: "CreatedDate desc" }, query)
                    .then(function (lo) {
                        isBusy(false);
                        vm.auditTrailCollection(lo.itemCollection);
                        tcs.resolve(true);
                    });
                return tcs.promise();

            },
            attached = function () {

            },
            showChange = function (audit) {
                vm.log(audit);
                $("#changes-dialog").modal();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            auditTrailCollection: ko.observable(),
            log: ko.observable(new bespoke.sph.domain.AuditTrail()),
            showChangesCommand: showChange
        };

        return vm;

    });

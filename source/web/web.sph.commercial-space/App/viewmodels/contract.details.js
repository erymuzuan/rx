/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/_uiready.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="./_contract.documents.js" />
/// <reference path="./_contract.clauses.js" />


define(['services/datacontext', './_contract.clauses', './_contract.documents', './_audittrail.list'],
    function (context, clausesvm, documentsvm, audittrailvm) {

        var isBusy = ko.observable(false),

            activate = function (routeData) {
                isBusy(true);
                var tcs = new $.Deferred();

                var contractLoaded = function (ctr) {
                    vm.contract(ctr);
                    vm.title("Butiran kontrak " + ctr.ReferenceNo());
                    clausesvm.init(ctr);
                    documentsvm.init(ctr);

                    //load audit trails
                    var query = _("EntityId eq <%= contractId %> AND Type eq 'Contract'").template({ contractId: ctr.ContractId() });
                    var query2 = _("EntityId eq <%= rentalApplicationId %> AND Type eq 'RentalApplication'").template({ rentalApplicationId: ctr.RentalApplicationId() });

                    var t1 = context.loadAsync("AuditTrail", query);
                    var t2 = context.loadAsync("AuditTrail", query2);
                    $.when(t1, t2)
                        .then(function (lo, lo2) {

                            var logs = _(lo.itemCollection).chain()
                                .union(lo2.itemCollection)
                                .sortBy(function (o) { return o.DateTime(); })
                                .reverse()
                                .value();

                            audittrailvm.auditTrailCollection(logs);
                            isBusy(false);
                            tcs.resolve(true);
                        });
                };

                context.loadOneAsync("Contract", "ContractId eq " + routeData.id)
                    .then(contractLoaded);
                $.get("/Watch/GetWatch?entity=Contract&id=" + routeData.id)
                    .done(function(w) {
                        vm.toolbar.watching(w);
                    });

                return tcs.promise();

            },

            viewAttached = function (view) {
                _uiready.init(view);
            },

            save = function () {
                vm.contract().TopicCollection(clausesvm.topicCollection());
                vm.contract().DocumentCollection(documentsvm.documentCollection());
                var json = ko.mapping.toJSON({ contract: vm.contract });
                var tcs = new $.Deferred();
                context.post(json, "Contract/Save")
                    .then(function (c) {
                        vm.contract(c);
                        tcs.resolve(c);
                    });
                return tcs.promise();
            },
            watch = function () {
                var tcs = new $.Deferred();
                var data = JSON.stringify({ id: vm.contract().ContractId(), entity: "Contract" });
                isBusy(true);

                context.post(data, "/Watch/Register")
                    .then(function (result) {
                        isBusy(false);


                        tcs.resolve(result);
                    });
                return tcs.promise();
            };

        var vm = {
            title: ko.observable(),
            isBusy: isBusy,
            activate: activate,
            viewAttached: viewAttached,
            contract: ko.observable(new bespoke.sphcommercialspace.domain.Contract()),
            toolbar: {
                saveCommand: save,
                watchCommand: watch,
                unwatchCommand: watch,
                watching: ko.observable(false)
            }
        };

        return vm;

    });

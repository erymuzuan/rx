/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />


define(['services/datacontext', 'services/logger', 'plugins/dialog'],
    function (context, logger, dialog) {

        var entity = ko.observable(),
            forms = ko.observableArray(),
            activate = function () {
                var query = String.format("Name eq '{0}'", entity()),
                    tcs = new $.Deferred();
                context.loadOneAsync("EntityDefinition", query)
                    .done(function (b) {

                        var query2 = String.format("EntityDefinitionId eq {0}", b.EntityDefinitionId());
                        context.loadAsync("EntityForm", query2)
                                            .then(function (lo) {
                                                forms(lo.itemCollection);
                                                tcs.resolve(true);
                                            });

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
            activate: activate,
            entity: entity,
            forms: forms,
            column: ko.observable(new bespoke.sph.domain.ViewColumn()),
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });

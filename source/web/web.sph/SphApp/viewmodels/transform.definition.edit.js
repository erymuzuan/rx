/// <reference path="../../Scripts/jquery-2.1.0.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../../Scripts/.js" />
/// <reference path="../services/datacontext.js" />


define(['services/datacontext', 'services/logger', objectbuilders.system],
    function (context, logger, system) {

        var td = ko.observable(new bespoke.sph.domain.TransformDefinition({ Name: 'New Mapping Definition', WebId: system.guid() })),
            isBusy = ko.observable(false),
            activate = function () {

            },
            attached = function (view) {

            },
            save = function () {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON(td);
                isBusy(true);

                context.post(data, "/sph/transformdefinition")
                    .then(function (result) {
                        isBusy(false);


                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
            editProp = function () {

                var tcs = new $.Deferred(),
                    clone = context.toObservable(ko.mapping.toJS(td));
                require(['viewmodels/transform.definition.prop.dialog', 'durandal/app'], function (dialog, app2) {
                    dialog.td(clone);

                    app2.showDialog(dialog)
                        .done(function (result) {
                            tcs.resolve(true);
                            $('div.modalBlockout,div.modalHost').remove();
                            if (!result) return;
                            if (result === "OK") {
                                for (var g in td()) {
                                    if (typeof td()[g] === "function" && (td()[g].name === "c" || td()[g].name === "observable")) {
                                        td()[g](ko.unwrap(clone[g]));
                                    } else {
                                        td()[g] = clone[g];
                                    }
                                }
                            }
                        });

                });

                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            td: td,
            toolbar: {
                saveCommand: save,
                commands: ko.observableArray([
                {
                    command: editProp,
                    caption: 'Edit Properties',
                    icon: 'fa fa-table'
                }])
            }
        };

        return vm;

    });

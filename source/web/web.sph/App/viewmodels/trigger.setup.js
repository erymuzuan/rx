/// <reference path="../../Scripts/jquery-2.0.2.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="../../Scripts/bootstrap.js" />

define(['services/datacontext', 'services/jsonimportexport', objectbuilders.app],
    function (context, eximp, app) {

        var isBusy = ko.observable(false),
            id = ko.observable(),
            activate = function (id2) {
                id(parseInt(id2));

                var query = String.format("TriggerId eq {0} ", id());
                var tcs = new $.Deferred();
                context.loadOneAsync("Trigger", query)
                    .done(function (t) {
                        if (t) {
                            vm.trigger(t);
                        } else {
                            vm.trigger(new bespoke.sph.domain.Trigger());
                        }
                        tcs.resolve(true);
                    });

                return tcs.promise();
            },
            attached = function () {


            },
            save = function () {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON(vm.trigger);
                isBusy(true);

                context.post(data, "/Trigger/Save")
                    .then(function (result) {
                        isBusy(false);
                        vm.trigger().TriggerId(result);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            },

            exportJson = function () {
                return eximp.exportJson("trigger." + vm.trigger().TriggerId() + ".json", ko.mapping.toJSON(vm.trigger));

            },

         importJson = function () {
             return eximp.importJson()
                 .done(function (json) {
                     var clone = context.toObservable(JSON.parse(json));
                     vm.trigger(clone);
                     vm.trigger().TriggerId(0);

                 });
         },
            reload = function () {
                return app.showMessage("This discard all your changed, do you wish to continue", "Reload", ["Yes", "No"])
                     .done(function (dialogResult) {
                         if (dialogResult === "Yes") {
                             activate({ id: id() });
                         }
                     });
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            trigger: ko.observable(new bespoke.sph.domain.Trigger()),



            toolbar: {
                saveCommand: save,
                reloadCommand: reload,
                exportCommand: exportJson,
                commands: ko.observableArray([
                    {
                        icon: 'fa fa-upload',
                        caption: 'import',
                        command: importJson
                    }
                ])
            }

        };

        return vm;

    });

/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../objectbuilders.js" />


define([objectbuilders.datacontext, objectbuilders.cultures],
    function (context, cultures) {

        var isBusy = ko.observable(false),
            templateId = ko.observable(),
            activate = function (routeData) {
                var id = parseInt(routeData.id);
                templateId(id);
                if (id) {
                    var query = String.format("SpaceTemplateId eq {0}", templateId());
                    var tcs = new $.Deferred();
                    context.loadOneAsync("SpaceTemplate", query)
                        .done(function (b) {
                            vm.csTemplate(b);
                            tcs.resolve(true);
                        });

                    return tcs.promise();
                } else {
                    vm.csTemplate(new bespoke.sph.domain.SpaceTemplate());
                    return true;
                }
            },
            addCustomField = function () {
                var customfield = new bespoke.sph.domain.CustomField();
                vm.csTemplate().CustomFieldCollection.push(customfield);
            },
            removeCustomField = function (customfield) {
                vm.csTemplate().CustomFieldCollection.remove(customfield);
            },
            save = function () {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON({ csTemplate: vm.csTemplate });
                isBusy(true);

                context.post(data, "/Template/SaveSpaceTemplate")
                    .then(function (result) {
                        isBusy(false);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            csTemplate: ko.observable(new bespoke.sph.domain.SpaceTemplate()),
            addCustomField: addCustomField,
            removeCustomField: removeCustomField,
            toolbar: { saveCommand: save },
            cultures: cultures
        };

        return vm;

    });

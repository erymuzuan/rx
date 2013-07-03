/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext'],
    function (context) {

        var isBusy = ko.observable(false),
            templateId = ko.observable(),
            activate = function (routeData) {
                var id = parseInt(routeData.templateId);
                templateId(id);
                if (id) {
                    var query = String.format("ComplaintTemplateId eq {0}", templateId());
                    var tcs = new $.Deferred();
                    context.loadOneAsync("ComplaintTemplate", query)
                        .done(function (b) {
                            vm.complaintTemplate(b);
                            tcs.resolve(true);
                        });

                    return tcs.promise();
                } else {
                    vm.complaintTemplate(new bespoke.sphcommercialspace.domain.ComplaintTemplate());
                    return true;
                }
            },
            addComplaintCategory = function () {
                var category = new bespoke.sphcommercialspace.domain.ComplaintCategory();
                vm.complaintTemplate().ComplaintCategoryCollection.push(category);
            },
            addSubCategory = function() {
                var subCategory = ko.observable();
                vm.complaintTemplate.subCategoryCollection.push(subCategory);
            },
            addCustomField = function () {
                var customfield = new bespoke.sphcommercialspace.domain.ComplaintCustomField();
                vm.complaintTemplate().ComplaintCustomFieldCollection.push(customfield);
            },
            save = function () {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON({ complaintTemplate: vm.complaintTemplate });
                isBusy(true);

                context.post(data, "/ComplaintTemplate/Save")
                    .then(function (result) {
                        isBusy(false);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            complaintTemplate: ko.observable(new bespoke.sphcommercialspace.domain.ComplaintTemplate()),
            addComplaintCategory: addComplaintCategory,
            addCustomField: addCustomField,
            addSubCategory:addSubCategory,
            saveCommand: save
        };

        return vm;

    });

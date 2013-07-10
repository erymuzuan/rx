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
            addSubCategory = function () {
                vm.subCategoryOptions.push({ text: ko.observable() });
            },

            removeSubCategory = function (sub) {
                vm.subCategoryOptions.remove(sub);
            },

            updateCategory = function () {
                vm.complaintTemplate().ComplaintCategoryCollection.push(vm.selectedComplaintCategory());
            },
            removeCategory = function (category) {
                vm.complaintTemplate().ComplaintCategoryCollection.remove(category);
            },
            addCustomField = function () {
                var customfield = new bespoke.sphcommercialspace.domain.ComplaintCustomField();
                vm.complaintTemplate().ComplaintCustomFieldCollection.push(customfield);
            },
            removeCustomField = function (customfield) {
                vm.complaintTemplate().ComplaintCustomFieldCollection.remove(customfield);
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
            },

        editCategory = function (category) {
            vm.selectedComplaintCategory(category);
            var subs = _(category.SubCategoryCollection()).map(function (s) {
                return { text: ko.observable(s) };
            });

            vm.subCategoryOptions(subs);
        },

         saveSubCategory = function () {
             var subs = (vm.subCategoryOptions()).map(function(s) {
                 return s.text();
             });
             vm.selectedComplaintCategory().SubCategoryCollection(subs);
         };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            
            subCategoryOptions: ko.observableArray(),
            
            complaintTemplate: ko.observable(new bespoke.sphcommercialspace.domain.ComplaintTemplate()),
            selectedComplaintCategory: ko.observable(new bespoke.sphcommercialspace.domain.ComplaintCategory()),
            
            addComplaintCategory: addComplaintCategory,
            removeCategory: removeCategory,
            addCustomField: addCustomField,
            removeCustomField: removeCustomField,
            addSubCategory: addSubCategory,
            removeSubCategory: removeSubCategory,
            
            updateCategoryCommand: updateCategory,
            saveCommand: save,

            editCategory: editCategory,
            saveSubCategoryCommand: saveSubCategory
        };

        return vm;

    });

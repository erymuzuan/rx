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
    function(context) {

        var isBusy = ko.observable(false),
            templateId = ko.observable(),
            activate = function(routeData) {
                templateId(routeData.templateId);
                
                if (routeData.templateId) {
                    var query = String.format("ComplaintTemplateId eq {0}", templateId());
                    var tcs = new $.Deferred();
                    context.loadOneAsync("ComplaintTemplate", query)
                        .done(function(b) {
                            vm.complainttemplate(b);
                            tcs.resolve(true);
                        });

                    return tcs.promise();
                } else {
                    vm.complainttemplate().ComplaintTemplateId(0);
                    vm.complainttemplate().Name('');
                    vm.complainttemplate().Description('');
                    vm.complainttemplate().IsActive(false);
                    return true;
                }
            },
            addComplaintCategory = function(category) {
                vm.ComplaintCategoryCollection.push(category);
            },
            addNewComplaintCategory = function () {
                $('#add-complaint-category').modal();
            },
            save = function() {
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON({ complainttemplate: vm.complainttemplate });
                isBusy(true);

                context.post(data, "/ComplaintTemplate/Save")
                    .then(function(result) {
                        isBusy(false);

                        
                        tcs.resolve(result);
                    });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            complainttemplate: ko.observable(new bespoke.sphcommercialspace.domain.ComplaintTemplate()),
            complaintCategory: ko.observable(new bespoke.sphcommercialspace.domain.ComplaintCategory()),
            addNewComplaintCategory: addNewComplaintCategory,
            addComplaintCategory: addComplaintCategory,
            saveCommand : save
        };

        return vm;

    });

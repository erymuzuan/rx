/// <reference path="../../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', 'plugins/dialog'],
    function (context, logger, dialog) {

        var entities = ko.observableArray(),
            departments = ko.observableArray(),
            designations = ko.observableArray(),
            activate = function(){
                var entitiesQuery = String.format("IsPublished eq {0}", 1),
                    tcs = new $.Deferred(),
                    departmentQuery = "Key eq 'Departments' and Owner eq '{0}';


                var departmentTask = context.loadOneAsync("Department", departmentQuery),
                    designationTask = context.getListAsync("Designation", null, "Name"),
                    entitiesTask = context.getListAsync("EntityDefinition", entitiesQuery, "Name");

                $.when(departmentTask, designationTask, entitiesTask)
                    .done(function(ds,dslo, elo){
                        entities(elo);
                        designations(dslo);
                        departments(_(ds).map(function(v){return v.Name;}));
                        tcs.resolve(true);
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
            activate : activate(),
            departments : departments,
            designations : designations,
            entities : entities,
            sd: ko.observable(new bespoke.sph.domain.SearchDefinition()),
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });

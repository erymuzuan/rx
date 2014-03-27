/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger', 'plugins/router', 'services/chart'],
    function (context, logger, router, chart) {

        var isBusy = ko.observable(false),
            view = ko.observable(),
            list = ko.observableArray([]),
            entity = ko.observable(new bespoke.sph.domain.EntityDefinition()),
            activate = function () {
                var edQuery = String.format("Name eq '{0}'", 'Patient'),
                    tcs = new $.Deferred(),
                    viewQuery = String.format("EntityDefinitionId eq 2002"),
                    edTask = context.loadOneAsync("EntityDefinition", edQuery),
                    viewTask = context.loadOneAsync("EntityView", viewQuery);


                $.when(edTask, viewTask)
                    .done(function (b, vw) {
                        entity(b);
                        view(vw);

                        tcs.resolve(true);
                    });


                return tcs.promise();
            },
            attached = function (view) {
                chart.draw('Patient');
                $(view).on('submit', 'form.paging-search', function (e) {
                    e.preventDefault();
                    var text = $(this).find('input[type=search]').val();
                    query2.query.query_string.query = text;

                    isBusy(true);
                    context.searchAsync("Patient", query2)
                        .done(function (lo) {
                            console.log(lo);
                            isBusy(false);
                        });

                });
            },
            query2 = {
                "from": 0,
                "size": 20,
                "query": {
                    "query_string": {
                        "default_field": "_all",
                        "query": ""
                    },
                    "filtered": {
                        "filter": {
                            "and": {
                                "filters": [

                                ]
                            }
                        }
                    }
                },
                "sort": [
                    {"Mrn": {"order": "asc"}}
                ]
            },
            query = {
                "from": 0,
                "size": 20,
                "query": {
                    "filtered": {
                        "filter": {
                            "and": {
                                "filters": [

                                ]
                            }
                        }
                    }
                },
                "sort": [
                    {"Mrn": {"order": "asc"}}
                ]
            };

        var vm = {
            exec : {},
            view: view,
            chart: chart,
            isBusy: isBusy,
            entity: entity,
            activate: activate,
            attached: attached,
            list: list,
            query: query,
            toolbar: {
                commands: ko.observableArray([]),
                addNew: {
                    location: '#/patient-registration/0',
                    caption: 'Patient registration'
                }
            }
        };

        return vm;

    });



define(['services/datacontext', 'services/logger', 'plugins/router'],

function(context, logger, router) {

    var isBusy = ko.observable(false),
        id = ko.observable([]),
        tools = ko.observableArray([]),
        reports = ko.observableArray([]),
        recentItems = ko.observableArray([]),
        charts = ko.observableArray([]),
        views = ko.observableArray([]),
        entity = ko.observable(new bespoke.sph.domain.EntityDefinition()),
        logQuery = {"query":{"bool":{"must":[{"terms":{"source":["postsalesorders"]}}]}},"sort":[{"time":{"order":"desc"}}],"from":0,"size":20},       
        activate = function() {
            var query = String.format("Name eq '{0}'", 'Product'),
                tcs = new $.Deferred(),
                chartsQuery = String.format("Entity eq 'Product' and IsDashboardItem eq 1"),
                formsQuery = String.format("EntityDefinitionId eq 'product' and IsPublished eq 1 and IsAllowedNewItem eq 1"),
                surchageFormsQuery = String.format("EntityDefinitionId eq 'surcharge-add-on' and IsPublished eq 1 and IsAllowedNewItem eq 1"),
                edTask = context.loadOneAsync("EntityDefinition", query),
                chartsTask = context.loadAsync("EntityChart", chartsQuery),
                formsTask = context.loadAsync("EntityForm", formsQuery),
                surchageFormsTask = context.loadAsync("EntityForm", surchageFormsQuery),
                reportTask = context.loadAsync("ReportDefinition", "[DataSource.EntityName] eq 'Product'"),
                viewsTask = $.get("/api/entity-views/product/dashboard"),//
                salesOrderViewsTask =$.get("/api/entity-views/sales-order/dashboard"),
                surchargeViewsTask =$.get("/api/entity-views/surcharge-add-on/dashboard") ;


            $.when(edTask, formsTask, viewsTask, reportTask, chartsTask, salesOrderViewsTask, surchargeViewsTask, surchageFormsTask)
                .done(function(b, formsLo, viewsLo, reportsLo, chartsLo, salesOrderViewsLo, surchargeViewsLo, surchageFormsLo) {
                entity(b);
                var getFormCommand = function(v) {
                    return {
                        caption: v.Name(),
                        command: function() {
                            window.location = `#${ko.unwrap(v.Route)}/0`;
                            return Task.fromResult(0);
                        },
                        icon: v.IconClass()
                    };
                },
                 formsCommands = formsLo.itemCollection.map(getFormCommand),
                 surchargeFormsCommands = surchageFormsLo.itemCollection.map(getFormCommand);

                 surchargeFormsCommands.forEach(v => formsCommands.push(v));


                charts(chartsLo.itemCollection);
                reports(reportsLo.itemCollection);

                 if(_.isArray(viewsLo) && _.isArray(viewsLo[0])){
                    views(viewsLo[0]);
                 }
                 if(_.isArray(salesOrderViewsLo) && _.isArray(salesOrderViewsLo[0])){
                     salesOrderViewsLo[0].forEach(v => views.push(v));
                 }
                 if(_.isArray(surchargeViewsLo) && _.isArray(surchargeViewsLo[0])){
                     surchargeViewsLo[0].forEach(v => views.push(v));
                 }
                 //surchargeViewsLo

                // get counts
                _(views()).each(function(v) {
                    v.CountMessage = ko.observable("....");
                    let tm = setInterval(function() {
                        v.CountMessage(v.CountMessage() == "...." ? "..." : "....");
                    }, 250);

                    let route = "products";
                    if(v.Endpoint === "all-sales-orders"){
                        route = "sales-orders";
                    }
                    if(v.Endpoint === "default-surcharges-and-addons"){
                        route = "surcharges-addons";
                    }
                    
                    $.get(`/api/${route}/_metadata/_count/`)
                        .done(function(c) {
                        clearInterval(tm);
                        v.CountMessage(c._count);
                    });
                });

                vm.toolbar.commands(formsCommands);
                tcs.resolve(true);
            });

            // TODO : get views

            // TODO: get recent items

            //TODO : reports

            // TODO : tools
            //http://i.imgur.com/OZ6mSFq.png

            return tcs.promise();
        },
        attached = function(view) {
            $(view).on('click', 'a.unpin-chart', function(e) {
                e.preventDefault();
                var chart = ko.dataFor(this),
                    link = $(this);
                if (!chart) {
                    return;
                }
                if (typeof chart.unpin === "function") {
                    link.prop('disabled', true);
                    chart.unpin().done(function() {
                        charts.remove(chart);
                    });
                }
            });
        },
        addForm = function() {

        },
        addView = function() {

        },
        recentItemsQuery = {
            "sort": [{
                "ChangedDate": {
                    "order": "desc"
                }
            }]
        },
        openDetails = function (log) {
            require(["viewmodels/log.details.dialog", "durandal/app"], function (dialog, app2) {
                dialog.log(log);

                app2.showDialog(dialog)
                    .done(function () { });

            });
        },
        getMetronicColor = function(color) {
            switch (ko.unwrap(color)) {
                case "borange":
                    return "yellow-gold";
                case "bviolet":
                    return "purple";
                case "blightblue":
                    return "blue";
                case "bblue":
                    return "blue-madison";
                case "bred":
                    return "red";
                case "bgreen":
                    return "green-meadow";

            }
            return "blue-madison";
        };

    var vm = {
        isBusy: isBusy,
        logQuery : logQuery,
        openDetails: openDetails,
        logs : ko.observableArray(),
        getMetronicColor: getMetronicColor,
        views: views,
        charts: charts,
        entity: entity,
        activate: activate,
        attached: attached,
        reports: reports,
        tools: tools,
        recentItems: recentItems,
        addForm: addForm,
        addView: addView,
        recentItemsQuery: recentItemsQuery,
        toolbar: {
            commands: ko.observableArray([])
        }
    };

    return vm;

});
define(['services/datacontext'], function (context) {

    var field = ko.observable(),
        format = ko.observable(),
        aggregate = ko.observable(),
        dateInterval = ko.observable(),
        histogramInterval = ko.observable(),
        entityName = ko.observable(),
        _query = null,
        init = function (entity, query) {
            entityName(entity);
            _query = query;
        },
        draw = function (fd) {
            if (!field()) {
                field(fd);
            }
            if (!fd) {
                return;
            }
            var tcs = new $.Deferred();
            if (aggregate() === 'term') {
                _query.aggs = {
                    "category": {
                        "terms": {
                            "field": fd,
                            "size": 10
                        }
                    }
                };
            }

            if (aggregate() === 'histogram') {
                _query.aggs = {
                    "category": {
                        "histogram": {
                            "field": fd,
                            "interval": parseInt(histogramInterval()),
                            "min_doc_count": 0
                        }
                    }
                };
            }


            if (aggregate() === 'date_histogram') {
                _query.aggs = {
                    "category": {
                        "date_histogram": {
                            "field": fd,
                            "interval": dateInterval(),
                            "format": "yyyy-mm-dd"
                        }
                    }
                };
            }

            context.searchAsync(entityName(), _query)
                .done(function (result) {

                    var buckets = result.aggregations.category.buckets || result.aggregations.category,
                        data = _(buckets).map(function (v) {
                            return {
                                category: v.key_as_string || v.key.toString(),
                                value: v.doc_count
                            };
                        }),
                        chart = $("div#chart-" + entityName()).empty().kendoChart({
                            theme: "metro",
                            title: {
                                text: entityName() + " count by " + field()
                            },
                            legend: {
                                position: "bottom"
                            },
                            seriesDefaults: {
                                labels: {
                                    visible: true,
                                    format: "{0}"
                                }
                            },
                            series: [
                                {
                                    type: "pie",
                                    data: data
                                }
                            ], tooltip: {
                                visible: true,
                                format: "{0}",
                                template: "#= category #: #= value #"
                            }
                        }).data("kendoChart");
                    console.log(chart);
                    tcs.resolve(true);

                });


            return tcs.promise();
        },
        execute = function () {
           return draw(field());
        };


    field.subscribe(function (f) {
        if (aggregate() === 'term') {
            draw(f);
        }
        if (aggregate() === 'date_histogram' && dateInterval()) {
            draw(f);
        }
        if (aggregate() === 'histogram' && histogramInterval()) {
            draw(f);
        }
    });
    histogramInterval.subscribe(function () {
        if (aggregate() === 'histogram' && histogramInterval()) {
            draw(field());
        }
    });
    dateInterval.subscribe(function () {
        if (aggregate() === 'histogram' && histogramInterval()) {
            draw(field());
        }
    });

    return {
        execute: execute,
        format: format,
        aggregate: aggregate,
        dateInterval: dateInterval,
        histogramInterval: histogramInterval,
        draw: draw,
        init: init,
        field: field
    };
});
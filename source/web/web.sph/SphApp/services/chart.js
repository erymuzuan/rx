define(['services/datacontext'], function (context) {

    var field = ko.observable(),
        draw = function (entity, fd) {
            if(!field()){
                field(fd);
            }
            field.subscribe(function(f){
                draw(entity,f);
            });
            var tcs = new $.Deferred(),
                query = {
                    "aggs": {
                        "category": {
                            "terms": {
                                "field": fd,
                                "size": 10
                            }
                        }
                    }
                };

            context.searchAsync(entity, query)
                .done(function (result) {
                    tcs.resolve(true);
                    var data = _(result.aggregations.category.buckets).map(function (v) {
                            return {
                                category: v.key,
                                value: v.doc_count
                            };
                        }),
                        chart = $("div#chart-" + entity).empty().kendoChart({
                            title: {
                                text: "whatever"
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
                            ]
                        }).data("kendoChart");
                    console.log(chart);

                });


            return tcs.promise();
        };
    return {
        draw: draw,
        field: field
    };
});
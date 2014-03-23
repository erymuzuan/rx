define(['services/datacontext'], function (context) {

    var field = ko.observable(),
        entityName = ko.observable(),
        draw = function (entity, fd) {
            entityName(entity);
            if(!field()){
                field(fd);
            }
            if(!fd){
                return;
            }
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
                                text: entityName() +  " count by " + field()
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



    field.subscribe(function(f){
        draw(entityName(), f);
    });

    return {
        draw: draw,
        field: field
    };
});
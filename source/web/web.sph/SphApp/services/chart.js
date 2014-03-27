define(['services/datacontext'], function (context) {

    var field = ko.observable(),
        entityName = ko.observable(),
        _query = null,
        init = function(entity, query){
            entityName(entity);
            _query = query;
        },
        draw = function (fd) {
            if(!field()){
                field(fd);
            }
            if(!fd){
                return;
            }
            var tcs = new $.Deferred();
            _query.aggs = {
                "category": {
                    "terms": {
                        "field": fd,
                            "size": 10
                    }
                }
            };

            context.searchAsync(entityName(), _query)
                .done(function (result) {
                    tcs.resolve(true);
                    var data = _(result.aggregations.category.buckets).map(function (v) {
                            return {
                                category: v.key,
                                value: v.doc_count
                            };
                        }),
                        chart = $("div#chart-" + entityName()).empty().kendoChart({
                            theme :"metro",
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
        draw(f);
    });

    return {
        draw: draw,
        init : init,
        field: field
    };
});
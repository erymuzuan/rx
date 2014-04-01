define(['services/datacontext', objectbuilders.system], function (context, system) {

    var _field = ko.observable(),
        _format = ko.observable(),
        _aggregate = ko.observable(),
        _dateInterval = ko.observable(),
        _histogramInterval = ko.observable(),
        _entity = ko.observable(),
        _entityDefinitionId = ko.observable(),
        _query = null,
        _type = ko.observable('pie'),
        _click = null,
        _charts = ko.observableArray(),
        _viewId = ko.observable(),
        _selectedChartId = ko.observable(),
        _selectedChart = ko.observable(),
        init = function (entity, query, click, viewId) {
            var tcs = new $.Deferred(),
                query1 = String.format("Entity eq '{0}'", entity),
                query2 = String.format("Name eq '{0}'", entity);

            if (viewId) {
                _viewId(parseInt(viewId));
            }

            _entity(entity);
            _query = query;
            _click = click;
            context.loadAsync("EntityChart", query1)
                .done(function (lo) {
                    _charts(lo.itemCollection);
                });
            context.getScalarAsync("EntityDefinition", query2, "EntityDefinitionId")
                .done(function (id) {
                    _entityDefinitionId(parseInt(id));
                });
            return tcs.promise();
        },
        draw = function (fd) {
            if (!_field()) {
                _field(fd);
            }
            if (!fd) {
                return;
            }
            var tcs = new $.Deferred();
            if (_aggregate() === 'term') {
                _query.aggs = {
                    "category": {
                        "terms": {
                            "field": fd,
                            "size": 10
                        }
                    }
                };
            }

            if (_aggregate() === 'histogram') {
                _query.aggs = {
                    "category": {
                        "histogram": {
                            "field": fd,
                            "interval": parseInt(_histogramInterval()),
                            "min_doc_count": 0
                        }
                    }
                };
            }


            if (_aggregate() === 'date_histogram') {
                _query.aggs = {
                    "category": {
                        "date_histogram": {
                            "field": fd,
                            "interval": _dateInterval(),
                            "format": "yyyy-mm-dd"
                        }
                    }
                };
            }

            context.searchAsync(_entity(), _query)
                .done(function (result) {

                    var buckets = result.aggregations.category.buckets || result.aggregations.category,
                        data = _(buckets).map(function (v) {
                            return {
                                category: v.key_as_string || v.key.toString(),
                                value: v.doc_count
                            };
                        }),
                        categories = _(buckets).map(function (v) {
                            return  v.key_as_string || v.key.toString();
                        }),
                        chart = $("div#chart-" + _entity()).empty().kendoChart({
                            theme: "metro",
                            title: {
                                text: _entity() + " count by " + _field()
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
                                    type: _type(),
                                    data: data
                                }
                            ],
                            categoryAxis: {
                                categories: categories,
                                majorGridLines: {
                                    visible: false
                                }
                            },
                            seriesClick: function (e) {
                                if (typeof  _click === "function") {
                                    _click({
                                        query: _query,
                                        category: e.category,
                                        value: e.value,
                                        field: _field(),
                                        aggregate: _aggregate(),
                                        histogramInterval: _histogramInterval(),
                                        dateInterval: _dateInterval()

                                    });

                                }
                            }, tooltip: {
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
            return draw(_field());
        },
        saveAs = function () {
            var tcs = new $.Deferred(),
                editedChart = new bespoke.sph.domain.EntityChart({
                    WebId: system.guid(),
                    Name: '',
                    Query: ko.mapping.toJSON(_query),
                    Entity: _entity(),
                    EntityDefinitionId: _entityDefinitionId(),
                    Type: _type(),
                    EntityViewId: _viewId(),
                    Aggregate: _aggregate(),
                    Field: _field(),
                    DateInterval: _dateInterval(),
                    HistogramInterval: _histogramInterval()

                });

            require(['viewmodels/entity.chart.dialog', 'durandal/app'], function (dialog, app2) {
                dialog.chart(editedChart);

                app2.showDialog(dialog)
                    .done(function (result) {
                        if (!result) return;
                        if (result === "OK") {
                            context.post(ko.mapping.toJSON(editedChart), '/sph/entitychart/save')
                                .done(tcs.resolve);
                        } else {
                            tcs.resolve(false);
                        }
                    });

            });

            return tcs.promise();
        },
        save = function () {
            var tcs = new $.Deferred();
            _selectedChart().Type(_type());
            _selectedChart().Aggregate(_aggregate());
            _selectedChart().Field(_field());
            _selectedChart().DateInterval(_dateInterval());
            _selectedChart().HistogramInterval(_histogramInterval());

            require(['viewmodels/entity.chart.dialog', 'durandal/app'], function (dialog, app2) {
                dialog.chart(_selectedChart());

                app2.showDialog(dialog)
                    .done(function (result) {
                        if (!result) return;
                        if (result === "OK") {
                            context.post(ko.mapping.toJSON(_selectedChart), '/sph/entitychart/save')
                                .done(tcs.resolve);
                        } else {
                            tcs.resolve(false);
                        }
                    });

            });

            return tcs.promise();
        },
        remove = function () {
            var tcs = new $.Deferred();

            context.post(ko.mapping.toJSON(_selectedChart), '/sph/entitychart/remove')
                .done(tcs.resolve);

            return tcs.promise();
        };


    _field.subscribe(function (f) {
        if (_aggregate() === 'term') {
            draw(f);
        }
        if (_aggregate() === 'date_histogram' && _dateInterval()) {
            draw(f);
        }
        if (_aggregate() === 'histogram' && _histogramInterval()) {
            draw(f);
        }
    });
    _histogramInterval.subscribe(function (hi) {
        if (_aggregate() === 'histogram' && hi) {
            draw(_field());
        }
    });
    _dateInterval.subscribe(function (di) {
        if (_aggregate() === 'date_histogram' && di) {
            draw(_field());
        }
    });
    _selectedChartId.subscribe(function (di) {
        if (!di) {
            return;
        }
        var tcs = new $.Deferred();
        context.loadOneAsync("EntityChart", "EntityChartId eq " + di)
            .done(function (ec) {
                if (!ec) {
                    return;
                }
                _selectedChart(ec);
                _field('');// stop draw
                _type(ec.Type());
                _aggregate(ec.Aggregate());
                _dateInterval(ec.DateInterval());
                _histogramInterval(ec.HistogramInterval());
                _field(ec.Field());

            });
        return tcs.promise();
    });
    _type.subscribe(function (type) {
        if (!_field()) {
            return;
        }
        if (!type) {
            return;
        }
        if (_aggregate() === 'term') {
            draw(_field());
        }
        if (_aggregate() === 'date_histogram' && _dateInterval()) {
            draw(_field());
        }
        if (_aggregate() === 'histogram' && _histogramInterval()) {
            draw(_field());
        }
    });

    return {
        selectedChartId: _selectedChartId,
        save: save,
        saveAs: saveAs,
        remove: remove,
        type: _type,
        charts: _charts,
        execute: execute,
        format: _format,
        aggregate: _aggregate,
        dateInterval: _dateInterval,
        histogramInterval: _histogramInterval,
        draw: draw,
        init: init,
        field: _field
    };
});
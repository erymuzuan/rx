define(['durandal/system', 'services/logger', objectbuilders.config, 'services/datacontext'],
    function (system, logger, config, context) {

        var isBusy = ko.observable(),
            text = ko.observable(),
            entities = ko.observable(),
            activate = function () {
                var query = String.format("IsPublished eq {0}", 1);
                var tcs = new $.Deferred();

                context.getTuplesAsync({
                    entity: "EntityDefinition",
                    query: query,
                    field: "Name",
                    field2: "RecordName"
                })
                    .then(function (lo) {
                        isBusy(false);
                        entities(lo);
                        tcs.resolve(true);
                    });
                return tcs.promise();

            },
            attached = function (view) {
                $(view).on('click', '#close-search-result', function (e) {
                    e.preventDefault();
                    vm.searchResults.removeAll();
                });

            },
            search = function () {
                var tcs = new $.Deferred(),
                    data = JSON.stringify({ text: vm.searchText() });
                isBusy(true);

                context.post(data, "/Search")
                    .then(function (result) {
                        isBusy(false);
                        var hits = _(result.hits.hits).map(function (v) {
                            var record = _(entities()).find(function (x) {
                                return x.Item1.toLowerCase() == v._type.toLowerCase();
                            });
                            if (!record) {
                                console.log("whoaa canot find", v);
                                return {
                                    type: v._type,
                                    id: v._id,
                                    title: "",
                                    iconClass: 'fa fa-user'
                                }
                            }
                            var title = v._source[record.Item2];
                            if (v.highlight) {
                                title = v.highlight[record.Item2][0];
                            }
                            return {
                                type: v._type,
                                id: v._id,
                                title: title,
                                iconClass: 'fa fa-user'
                            };
                        });
                        vm.searchResults(hits);
                        tcs.resolve(hits);
                    });
                return tcs.promise();
            }
            ,
            navigateSearch = function (sr) {
                var query = String.format("Name eq '{0}'", sr.type),
                    tcs = new $.Deferred();
                context.loadOneAsync("EntityDefinition", query)
                    .done(function (b) {
                        // get the default form
                        var query2 = String.format("EntityDefinitionId eq {0}", b.EntityDefinitionId());
                        context.loadOneAsync("EntityForm", query2)
                            .done(function (f) {
                                window.location = String.format("/sph#{0}/{1}", f.Route(), sr.id);
                                tcs.resolve(true);
                            });
                    });

                return tcs.promise();
            };

        text.subscribe(function (t) {
            console.log(t);
        });

        var vm = {
            entities: entities,
            activate: activate,
            searchCommand: search,
            searchResults: ko.observableArray(),
            navigateSearch: navigateSearch,
            isBusy: isBusy,
            searchText: text,
            attached: attached
        };

        return vm;


    });
define(['durandal/system', 'services/logger', objectbuilders.config, 'services/datacontext'],
    function (system, logger, config, context) {

        var isBusy = ko.observable(),
            text = ko.observable(),
            entities = ko.observableArray(),
            activate = function () {
                var query = String.format("IsPublished eq {0}", 1),
                    tcs = new $.Deferred();

                context.loadAsync({
                    entity: "EntityDefinition",
                    query: query
                })
                    .then(function (lo) {
                        isBusy(false);
                        entities(lo.itemCollection);
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
                var tcs = new $.Deferred();
                isBusy(true);

                context.get("search/" + ko.unwrap(vm.searchText))
                    .then(function (result) {
                        isBusy(false);
                        var hits = _(result.hits.hits).map(function (v) {
                            var record = _(entities()).find(function (x) {
                                return ko.unwrap(x.Name).toLowerCase() === v._type.toLowerCase();
                            });
                            if (!record) {
                                console.log("whoaa cannot find", v);
                                return {
                                    type: v._type,
                                    id: v._id,
                                    title: "",
                                    iconClass: ko.unwrap(v.IconClass)
                                };
                            }
                            var rn = ko.unwrap(record.RecordName),
                                title = v._source[rn];
                            if (v.highlight) {
                                if (v.highlight[rn]) {
                                    title = v.highlight[rn][0];
                                } else {
                                    title = v.highlight[Object.keys(v.highlight)[0]][0];
                                }
                            }

                            return {
                                type: v._type,
                                id: v._id,
                                title: title,
                                iconClass: ko.unwrap(record.IconClass)
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
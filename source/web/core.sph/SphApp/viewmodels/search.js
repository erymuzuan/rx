define(['durandal/system', 'services/logger', objectbuilders.config, 'services/datacontext'],
    function (system, logger, config, context) {

        var isBusy = ko.observable(),

            attached = function (view) {
                $(view).on('click', '#close-search-result', function (e) {
                    e.preventDefault();
                    vm.searchResults.removeAll();
                });

            },
            search = function () {
                var tcs = new $.Deferred();
                var data = JSON.stringify({ text: vm.searchText() });
                isBusy(true);

                context.post(data, "/Search")
                    .then(function (result) {
                        isBusy(false);
                        vm.searchResults(result);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            }
            ,
            navigateSearch = function (sr) {
                
            };

        var vm = {
            
            searchCommand: search,
            searchResults: ko.observableArray(),
            navigateSearch: navigateSearch,
            isBusy: isBusy,
            searchText: ko.observable(''),
            attached: attached
        };

        return vm;


    });
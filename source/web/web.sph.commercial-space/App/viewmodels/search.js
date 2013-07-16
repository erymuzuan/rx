define(['durandal/system', 'durandal/plugins/router', 'services/logger', 'config','services/datacontext'],
    function (system, router, logger, config, context) {

        var isBusy = ko.observable(),

            viewAttached = function (view) {
                $(view).on('click', '#close-search-result', function (e) {
                    e.preventDefault();
                    shell.searchResults.removeAll();
                });

            },
            search = function () {
                var tcs = new $.Deferred();
                var data = JSON.stringify({ text: shell.searchText() });
                isBusy(true);

                context.post(data, "/Search")
                    .then(function (result) {
                        isBusy(false);
                        shell.searchResults(result);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            }
            ,            
            navigateSearch = function(sr) {
                console.log(sr);
            };

        var shell = {
            activate: activate,
            searchCommand: search,
            searchResults: ko.observableArray(),
            navigateSearch : navigateSearch, 
            isBusy: isBusy,
            searchText: ko.observable(),
            viewAttached: viewAttached,
            router: router
        };

        return shell;

        //#region Internal Methods
        function activate() {
            return boot();
        }


        function boot() {
            router.map(config.routes);
            log('System Loaded!', null, true);
            return router.activate(config.startModule);
        }

        function log(msg, data, showToast) {
            logger.log(msg, data, system.getModuleId(shell), showToast);
        }
        //#endregion
    });
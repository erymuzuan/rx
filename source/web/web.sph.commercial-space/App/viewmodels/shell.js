define(['durandal/system', 'durandal/plugins/router', 'services/logger', 'config'],
    function (system, router, logger, config) {

        var viewAttached = function (view) {
                // NOTE: there's a bug someweher that makes bootstrap data-toggle didn't work
                $(view).on('click', 'li.dropdown>a.dropdown-toggle', function () {
                    $(this).parent().toggleClass("open");
                });

            };

        var shell = {
            activate: activate,
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
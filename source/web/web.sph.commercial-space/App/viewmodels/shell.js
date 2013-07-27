define(['durandal/system', 'durandal/plugins/router', 'services/logger', 'config'],
    function (system, router, logger, config) {

        var viewAttached = function (view) {
            $(view).on('click', 'li.dropdown>a.dropdown-toggle', function () {
                $(this).parent().toggleClass("open");
            });
            var $menu = $('.jPanelMenu-panel');
            $('#drawer-menu').on('click', function(e) {
                e.preventDefault();
                $menu.toggle();
            });
            $menu.hide().on('click', 'a', function() {
                $menu.hide();
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
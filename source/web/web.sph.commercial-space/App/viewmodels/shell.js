define(['durandal/system', 'durandal/plugins/router', 'services/logger', 'services/datacontext', 'config'],
    function (system, router, logger, context, config) {

        var viewAttached = function (view) {


            var dropDown = function (e) {
                e.preventDefault();
                e.stopPropagation();

                var button = $(this);
                button.parent().addClass("open");

                $(document).one('click', function () {
                    button.parent().removeClass("open");
                });
            };

            $(document).on('click', 'button.btn-context-action,a.btn-context-action', function (e) {
                e.preventDefault();
                var $a = $(this);
                var $panel = $(this).parent().find('.context-action');

                var closeButton = function (e) {
                    e.preventDefault();
                    $panel.hide();

                };

                $panel.css({
                    "right": e.offsetX || "0px"
                })
                    .show();

                $("button.close").one("click", closeButton);

            });
            $(document).on('click', 'a.dropdown-toggle', dropDown);

            var $menu = $('.jPanelMenu-panel');
            $('#drawer-menu').on('click', function (e) {
                e.preventDefault();
                $menu.toggle();
            });
            $menu.hide().on('click', 'a', function () {
                $menu.hide();
            });

            var $links = $('li.nred');
            var filterInput = $('#filter-text');

            var dofilter = function () {
                var filter = filterInput.val().toLowerCase();
                console.log(filter);
                $links.each(function () {
                    var $anchor = $(this);
                    if (typeof $anchor.data("string") !== "string") {
                        return;
                    }
                    if ($anchor.data("string").toLowerCase().indexOf(filter) > -1) {
                        $anchor.show();
                    } else {
                        $anchor.hide();
                    }
                });

            };
            var throttled = _.throttle(dofilter, 800);
            filterInput.on('keyup', throttled).siblings('.icon-remove')
                .click(function () {
                    filterInput.val('');
                    dofilter();
                });

            if (filterInput.val()) {
                dofilter();
            }

        },
            viewAuditTrail = function (log) {
                console.log(log);
                var query = String.format("Type eq '{0}' and EntityId eq {1}", log.entity, log.id());
                var tcs = new $.Deferred();

                context.loadAsync("AuditTrail", query)
                    .then(function (lo) {
                        shell.auditTrailCollection(lo.itemCollection);
                        shell.selectedAuditTrail(new bespoke.sphcommercialspace.domain.AuditTrail());

                        tcs.resolve(true);
                        $('#shell-logviewer-modal').modal({});
                    });
                return tcs.promise();
            },
            selectAuditTrail = function (log) {
                shell.selectedAuditTrail(log);
            },

            print = function (commandParameter) {
                var parameter = typeof commandParameter === "function" ? commandParameter() : commandParameter;

                var tcs = new $.Deferred();

                var url = "/print/" + parameter.entity + "/" + parameter.id();
                window.open(url);
                setTimeout(function () {
                    tcs.resolve(true);
                }, 500);
                return tcs.promise();
            };

        var shell = {
            activate: activate,
            viewAttached: viewAttached,
            router: router,
            viewAuditTrailCommand: viewAuditTrail,
            printCommand: print,
            auditTrailCollection: ko.observableArray(),
            selectedAuditTrail: ko.observable(new bespoke.sphcommercialspace.domain.AuditTrail()),
            selectAuditTrail: selectAuditTrail
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
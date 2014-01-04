/// <reference path="../objectbuilders.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />


define(['durandal/system', 'plugins/router', 'services/logger', 'services/datacontext', objectbuilders.config, objectbuilders.cultures],
    function (system, router, logger, context, config) {

        var activate = function () {
            return router.map(config.routes)
                .buildNavigationModel()
                .mapUnknownRoutes('viewmodels/page.list', 'page.list')
                .activate();
        },
            attached = function (view) {
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
                    var $panel = $a.parent().find('.context-action');

                    var closeButton = function (evt1) {
                        evt1.preventDefault();
                        $panel.hide();

                    };

                    $panel.css({
                        "right": e.offsetX || "0px"
                    })
                        .show();

                    $("button.close").one("click", closeButton);

                });
                $(document).on('click', 'a.dropdown-toggle', dropDown);

                $(document).on('click', 'table.table-striped th', function (e) {
                    e.preventDefault();
                    var table = $(this).parents("table.table-striped");
                    if (table && !table.prop("sorted")) {
                        console.dir(e);
                        table.tablesorter();
                        table.prop("sorted", "1");
                        $(this).trigger('click');
                    }
                });
                var $menu = $('#slider-menu'),
                    hideSlider = function () {
                        $('section#content').animate({ "margin-left": 0 });
                        $menu.hide().css({ "width": 0 });
                    },
                    showSlider = function () {
                        var tcs = new $.Deferred();

                        $('section#content').animate({ "margin-left": 280 }, function () {
                            tcs.resolve(true);
                            $menu.show();
                        });
                        $menu.css("height", $(document).height()).animate({ "width": 280 }, tcs.resolve)

                        return tcs.promise();

                    },
                    sliderVisible = $menu.is(':visible');


                $('#drawer-menu').on('click', function (e) {
                    e.preventDefault();
                    if (sliderVisible) {
                        hideSlider();
                    } else {
                        showSlider();
                    }
                    sliderVisible = !sliderVisible;

                });
                $('#slider-menu').on('click', 'li>a', function () {
                    hideSlider();
                    sliderVisible = false;
                });

                $(document).on('keyup', function (e) {
                    if (e.ctrlKey && e.keyCode === 81) {
                        if (sliderVisible) {
                            hideSlider();
                            sliderVisible = false;
                        } else {
                            showSlider()
                                .done(function () {
                                    filterInput.focus();
                                    sliderVisible = true;
                                });
                        }
                    }
                });

                var $links = $('div#slider-menu li'),
                    filterInput = $('#filter-text'),
                    selectedRouteItemIndex = 0,
                    dofilter = function (e) {
                        if (e && e.keyCode) {
                            if (e.keyCode === 40 || e.keyCode === 38 || e.keyCode === 13) {
                                return;
                            }
                        }
                        selectedRouteItemIndex = 0;
                        var filter = filterInput.val().toLowerCase();
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

                    },
                    navigateSelectedItem = function (e) {
                        var selectRouteItem = function (step) {
                            selectedRouteItemIndex += step;
                            var $list = $('div#slider-menu li:visible');

                            if (selectedRouteItemIndex <= 0) {
                                selectedRouteItemIndex = 1;
                                return;
                            }

                            if (selectedRouteItemIndex > $list.length - 1) {
                                selectedRouteItemIndex = $list.length - 1;
                                return;
                            }

                            $list.removeClass('active');
                            $($list[selectedRouteItemIndex]).addClass('active');

                        };
                        // select items
                        if (e && e.keyCode) {
                            if (e.keyCode === 40) {
                                selectRouteItem(1);
                                return;
                            }
                            if (e.keyCode === 38) {
                                selectRouteItem(-1);
                                return;
                            }
                            if (e.keyCode === 13) {
                                $('div#slider-menu li:visible.active').find('a').trigger('click');
                                return;
                            }
                        }
                    },

                    throttled = _.throttle(dofilter, 800);


                filterInput
                    .on('keyup', throttled)
                    .on('keyup', navigateSelectedItem)
                    .siblings('span')
                    .click(function () {
                        filterInput.val('');
                        dofilter();
                    });

                if (filterInput.val()) {
                    dofilter();
                }

            },
            viewAuditTrail = function (log2) {
                console.log(log2);
                var query = String.format("Type eq '{0}' and EntityId eq {1}", log2.entity, log2.id());
                var tcs = new $.Deferred();

                context.loadAsync("AuditTrail", query)
                    .then(function (lo) {
                        shell.auditTrailCollection(lo.itemCollection);
                        shell.selectedAuditTrail(new bespoke.sph.domain.AuditTrail());

                        tcs.resolve(true);
                        $('#shell-logviewer-modal').modal({});
                    });
                return tcs.promise();
            },
            selectAuditTrail = function (log2) {
                shell.selectedAuditTrail(log2);
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
            attached: attached,
            router: router,
            viewAuditTrailCommand: viewAuditTrail,
            printCommand: print,
            auditTrailCollection: ko.observableArray(),
            selectedAuditTrail: ko.observable(new bespoke.sph.domain.AuditTrail()),
            selectAuditTrail: selectAuditTrail
        };

        return shell;


    });
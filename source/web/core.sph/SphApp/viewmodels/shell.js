/// <reference path="../objectbuilders.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/require.js" />


define(['durandal/system', 'plugins/router', 'services/logger', 'services/datacontext', objectbuilders.config, objectbuilders.cultures],
    function (system, router, logger, context, config) {

        var activate = function () {
            return router.map(config.routes)
                .buildNavigationModel()
                .mapUnknownRoutes('viewmodels/not.found', 'not.found')
                .activate();
        },
            attached = function (view) {
                $(view).on('click', 'a#help', function (e) {
                    e.preventDefault();
                    var topic = window.location.hash;
                    window.open("/docs/" + topic);
                });
                $(document).on('click', 'a#help-dialog', function (e) {
                    e.preventDefault();
                    var topic = $(this).data("dialog");
                    window.open("/docs/#" + topic);
                });
                // BUG:#1499
                if (window.location.href.indexOf("/sph#") === -1) {
                    window.location = "/sph#" + config.startModule;
                    return;
                }
                var dropDown = function (e) {
                    e.preventDefault();
                    e.stopPropagation();

                    var button = $(this);
                    button.parent().addClass("open");

                    $(document).one('click', function () {
                        button.parent().removeClass("open");
                    });
                };

                $(document).on('mouseenter', '.view-model-modal .modal-header', function (e) {
                    e.preventDefault();
                    var elem = $(this).parents('.view-model-modal'),
                        draggable = elem.data('draggable') || elem.data('ui-draggable') || elem.data('uiDraggable');

                    if (!draggable) {
                        elem.draggable({
                            handle: '.modal-header'
                        });
                        $('.modal-header').css("cursor", "move");
                        console.log('draggagle modal');
                        elem.find('div.modal-header>button').
                            after('<a class="pull-right" id="help-dialog" data-dialog="' + elem.attr('id') + '" href="#" title="see help on this topic" style="margin-right:10px; color:gray"><i class="fa fa-question-circle"></i></a>');
                    }
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


                $(view).on('click', '#drawer-menu', function (e) {
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
                    //console.log(e.keyCode);
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
                    if (e.ctrlKey && (e.keyCode === 188 || e.keyCode === 192)) {
                        if (_(config.roles).indexOf("developers") < 0) {
                            return;
                        }
                        require(['viewmodels/dev.quick.nav', 'durandal/app'], function (dialog, app2) {

                            app2.showDialog(dialog)
                                .done(function (result) {
                                    console.log(result);
                                });

                        });
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

            print = function (commandParameter) {
                var parameter = typeof commandParameter === "function" ? commandParameter() : commandParameter,
                    url = String.format("/sph/print/{0}/{1}", parameter.entity, +parameter.id());
                window.open(url);
                return Task.fromResult(true);
            },
            email = function (commandParameter) {
                var parameter = typeof commandParameter === "function" ? commandParameter() : commandParameter,
                    url = String.format("/sph/print/{0}/{1}", parameter.entity, +parameter.id()),
                    tcs = new $.Deferred();



                require(['viewmodels/email.entity.dialog', 'durandal/app'], function (dialog, app2) {
                    dialog.entity(parameter.entity);
                    dialog.id(parameter.id());
                    if (typeof dialog.wd === "function") {
                        dialog.wd(self);
                    }
                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {

                            }
                            tcs.resolve(true);
                        });

                });
                return tcs.promise();
            };

        var shell = {
            config: config,
            activate: activate,
            attached: attached,
            router: router,
            printCommand: print,
            emailCommand: email
        };

        return shell;


    });
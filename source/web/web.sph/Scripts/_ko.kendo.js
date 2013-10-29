﻿/// <reference path="knockout-2.3.0.debug.js" />
/// <reference path="underscore.js" />
/// <reference path="moment.js" />
/// <reference path="~/Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="~/App/durandal/amd/require.js" />
/// <reference path="~/kendo/js/kendo.all.js" />
/// <reference path="_pager.js" />
/// <reference path="/App/services/datacontext.js" />
/// <reference path="/App/objectbuilders.js" />



ko.bindingHandlers.kendoEditor = {
    init: function (element, valueAccessor) {
        var $editor = $(element),
            value = valueAccessor(),
            updating = false;

        setTimeout(function () {
            var editor = $editor.kendoEditor({
                change: function () {
                    if (updating) return;
                    updating = true;
                    value(this.value());
                    setTimeout(function () { updating = false; }, 500);
                }
            }).data("kendoEditor");

            editor.value(value());

            value.subscribe(function (html) {
                if (updating) return;
                updating = true;
                editor.value(html);
                setTimeout(function () { updating = false; }, 500);
            });

        }, 500);


    },
    update2: function (element, valueAccessor) {
        var $editor = $(element),
            value = valueAccessor(),
            ke = $editor.data("kendoEditor");
        if (!$editor.data("updating")) {
            ke.value(value());
        }
    }
};

ko.bindingHandlers.kendoDropDownListValue = {
    init: function (element, valueAccessor) {
        var value = valueAccessor();
        var currentModelValue = ko.utils.unwrapObservable(value);
        var dd = $(element).data('kendoDropDownList');
        dd.value(currentModelValue);

        dd.bind("change", function () {
            var nv = dd.value();
            value(nv);
        });
    },

    update: function (element, valueAccessor) {
        //update value based on a model change
        var value = valueAccessor();
        var modelValue = ko.utils.unwrapObservable(value);

        if (modelValue) {
            $(element).data('kendoDropDownList').value(modelValue);
        }
    }
};

ko.bindingHandlers.source = {
};
ko.bindingHandlers.kendoComboBox = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var value = valueAccessor(),
            allBindings = allBindingsAccessor(),
            currentModelValue = ko.utils.unwrapObservable(value),
            dd = $(element).data('kendoComboBox') ||
                $(element).kendoComboBox({
                    dataSource: allBindings.source()
                }).data('kendoComboBox');

        dd.value(currentModelValue);
        allBindings.source.subscribe(function (options) {
            // console.log(options, dd);
            dd.dataSource.data(options);
        });
        dd.bind("change", function () {
            var nv = dd.value();
            value(nv);
        });
    },

    update: function (element, valueAccessor) {
        //update value based on a model change
        var value = valueAccessor();
        var modelValue = ko.utils.unwrapObservable(value);

        if (modelValue) {
            $(element).data('kendoComboBox').value(modelValue);
        }
    }
};

ko.bindingHandlers.money = {
    init: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        var money = parseFloat(value).toFixed(2);

        $(element).text(money);
        $(element).val(money);

        $(element).on("change", function () {
            var nv = $(this).val();
            value.text(nv);
        });
    },
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        var money = parseFloat(value).toFixed(2);

        $(element).text(money);
        $(element).val(money);
    }
};

///user moment format
ko.bindingHandlers.date = {
    init: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor()),
            date = moment(value.value());

        $(element).on("change", function () {
            var nv = $(this).val();
            value.value(nv);
        });
        if (!date) {
            $(element).text("");
            $(element).val("");
            return;
        }
        if (date.year() == 1) { // DateTime.Min
            $(element).text("");
            $(element).val("");
            return;
        }


        var dateString = date.format(value.format).toString();
        if (dateString.indexOf("NaN") < 0) {
            $(element).text(dateString);
            $(element).val(dateString);
        }



    },
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        var date = moment(value.value());
        if (!date) {
            $(element).text("");
            $(element).val("");
            return;
        }
        if (date.year() == 1) { // DateTime.Min
            $(element).text("");
            $(element).val("");
            return;
        }


        var dateString = date.format(value.format).toString();
        if (dateString.indexOf("NaN") < 0) {
            $(element).text(dateString);
            $(element).val(dateString);
        }


    }
};


ko.bindingHandlers.kendoUpload = {
    init: function (element, valueAccessor) {
        var context = require(objectbuilders.datacontext),
             logger = require(objectbuilders.logger),
            value = valueAccessor();
        $(element).attr("name", "files").kendoUpload({
            async: {
                saveUrl: "/BinaryStore/Upload",
                removeUrl: "/BinaryStore/Remove",
                autoUpload: true
            },
            multiple: false,
            error: function (e) {
                logger.logError(e, e, this, true);
            },
            success: function (e) {
                logger.info('Your file has been ' + e.operation);

                var storeId = e.response.storeId,
                    uploaded = e.operation === "upload",
                    removed = e.operation != "upload",
                    oldFile = value();
                if (uploaded) {
                    value(storeId);
                    if (oldFile) {
                        context.post(JSON.stringify({ id: oldFile }), "/BinaryStore/Remove/");
                    }
                }
                if (removed) {
                    value("");
                }
            },
            remove: function () {
                var tcs = new $.Deferred(),
                    data = JSON.stringify({ id: value() });
                context.post(data, "/BinaryStore/Remove/")
                    .then(function (result) {
                        tcs.resolve(result);
                    });
                return tcs.promise();
            }
        });
    }
};

///user moment format
ko.bindingHandlers.kendoDate = {
    init: function (element, valueAccessor) {
        var value = valueAccessor(),
            $input = $(element),
            currentValue = ko.utils.unwrapObservable(value),
            date = moment(currentValue),
            changed = function (e) {
                console.log(e);
                var nv = this.value();
                if (typeof nv == "string") {
                    date = moment(nv, "DD/MM/YYYY");
                } else {
                    date = moment(nv);
                }
                // DO NOT fire update
                $input.data("stop", "true");
                value(date.format("YYYY-MM-DD"));
                $input.data("stop", "false");

            },
            picker = $input.kendoDatePicker({ format: "dd/MM/yyyy", change: changed }).data("kendoDatePicker");

        if (!date) {
            picker.value(null);
            return;
        }

        if (date.year() === 1) { // DateTime.Min
            picker.value(null);
            return;
        }

        picker.value(date.toDate());
    },
    update: function (element, valueAccessor) {
        var $input = $(element);
        if ($input.data("stop") == "true") return;

        var value = valueAccessor(),
            modelValue = ko.utils.unwrapObservable(value),
            date = moment(modelValue),
            picker = $input.data("kendoDatePicker");

        if (!date) {
            picker.value(null);
            return;
        }
        if (date.year() == 1) { // DateTime.Min
            picker.value(null);
            return;
        }

        picker.value(date.toDate());

    }
};

ko.bindingHandlers.slideVisible = {
    init: function (element, valueAccessor) {
        // Initially set the element to be instantly visible/hidden depending on the value
        var value = valueAccessor();
        $(element).toggle(ko.utils.unwrapObservable(value));
    },
    update: function (element, valueAccessor) {
        var value = valueAccessor();
        ko.utils.unwrapObservable(value) ? $(element).slideDown() : $(element).slideUp();
    }
};

ko.bindingHandlers.kendoEnable = {
    init: function (element, valueAccessor) {
        var value = valueAccessor();
        var enable = ko.utils.unwrapObservable(value);
        if (enable) {
            $(element).removeClass('k-state-disabled');
        } else {
            $(element).addClass('k-state-disabled');
        }
    },
    update: function (element, valueAccessor) {
        var value = valueAccessor();
        var enable = ko.utils.unwrapObservable(value);
        if (enable) {
            $(element).removeClass('k-state-disabled');
            $(element).removeAttr('disabled');
        } else {
            $(element).addClass('k-state-disabled');
            $(element).attr('disabled', 'disabled');
        }
    }
};

ko.bindingHandlers.command = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var action = valueAccessor(),
            $button = $(element),
            allBindings = allBindingsAccessor(),
            inputValue = $button.val();

        if (allBindings.isvisible) {
            var visible = typeof allBindings.isvisible === "function" ? allBindings.isvisible() : allBindings.isvisible;
            if (!visible) {
                $button.hide();
            } else {
                $button.show();
            }

            if (typeof allBindings.isvisible === "function") {
                allBindings.isvisible.subscribe(function (v) {
                    if (v)
                        $button.show();
                    else
                        $button.hide();
                });
            }
        }


        $button.click(function (e) {
            e.preventDefault();
            if (this.form) {
                if (!this.form.checkValidity()) return;
            }

            var $spinner = $("<i class='icon-spin icon-spinner icon-large'></i>");
            $spinner.css({ "margin-left": -($button.width() / 2) - 16, "position": "fixed", "margin-top": "10px" });
            $button.after($spinner).show();
            action()
                .then(function () {
                    $button
                        .button("complete")
                        .prop('disabled', true)
                        .val(inputValue)
                        .removeClass('btn-disabled');
                    $spinner.hide();

                });
            if ($button.data("loading-text")) {
                $button.button("loading");
            }
            $button.addClass('btn-disabled').prop('disabled', true);

        });
    }
};

ko.bindingHandlers.field = {
    init: function() {
    }
};
ko.bindingHandlers.unwrapClick = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var action = valueAccessor(),
            button = $(element),
            allBindings = allBindingsAccessor();


        button.click(function (e) {
            e.preventDefault();
            var prop = allBindings.property,
                accessor = allBindings.accessor,
                type = allBindings.field;
            /* if we can't get to the function , i.e. it's still object not ko.observable
            
            */
            if (prop) {
                if (typeof accessor[prop] === "undefined") {
                    console.log("Can't figure out the accessor.prop");
                }
                if (typeof accessor[prop] === "object") {
                    accessor[prop] = ko.observable(accessor[prop]);
                }
                if (typeof accessor[prop] === "function") {
                    action(accessor[prop],type);
                }
            } else {
                action(accessor,type);
            }

        });
    }
};

ko.bindingHandlers.fieldImage = {
    init: function (element, valueAccessor) {
        var type = valueAccessor(),
            img = $(element);

        var ft = typeof type === "function" ? type() : type;
        img.attr("src", "/image/index/" + ft);


    }
};


ko.bindingHandlers.stringArrayAutoComplete = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var value = valueAccessor(),
            allBindings = allBindingsAccessor(),
            options = allBindings.data();

        $(element).data("kendoAutoComplete") ||
           $(element).kendoAutoComplete({
               dataSource: options,
               change: function () {
                   var data = _(this.value().split(",")).filter(function (s) {
                       return s;
                   });
                   value(data);
               },
               filter: "startswith",
               placeholder: "....",
               separator: ","
           }).val(value());
    },
    update: function (element, valueAccessor) {
        var value = valueAccessor();
        $(element).data("kendoAutoComplete").value(value());
    }

};



ko.bindingHandlers.cssAutoComplete = {
    init: function (element, valueAccessor) {
        var value = valueAccessor();

        var bootstrap = _(document.styleSheets).find(function (s) {
            // TODO : what happend if were to combine the css with Bundle
            return s.href.indexOf("bootstrap") > -1;
        });
        var data = ['btn', 'btn-warning', 'btn-success', 'btn-link'];
        if (bootstrap) {
            data = _.chain(bootstrap.rules).filter(function (r) {
                return /^\./g.test(r.selectorText)
                    && !/:/g.test(r.selectorText)
                    && !/\s/g.test(r.selectorText)
                    && !/\+/g.test(r.selectorText)
                    && !/>/g.test(r.selectorText)
                    && !/\[/g.test(r.selectorText);
            }).map(function (s) {
                return s.selectorText.replace(/\./g, "");
            })
                .value();
        }
        $(element).data("kendoAutoComplete") ||
           $(element).kendoAutoComplete({
               dataSource: data,
               change: function () {
                   value(this.value());
               },
               filter: "startswith",
               placeholder: "Select class...",
               separator: " "
           }).data("kendoAutoComplete");

        $(element).change(function () {
            value($(this).val());
            console.log("new value", value());
        }).val(value());


    }

};


ko.bindingHandlers.pathAutoComplete = {
    init: function (element, valueAccessor) {
        var command = valueAccessor();
        var value = command.value;
        var type = command.type;

        if (!type()) return;

        $.get("/App/TriggerPathPickerJson/" + type())
            .done(function (json) {
                var tree = JSON.parse(json);
                var data = _.chain(tree)
                    .map(function (t) {
                        if (t.parent === "")
                            return t.name;
                        return undefined;
                    })
                    .filter(function (t) {
                        return typeof t !== "undefined";
                    })
                    .value();

                console.log(data);

                var dataSource = new kendo.data.DataSource({
                    data: data
                });
                var input = $(element).data("kendoAutoComplete") ||
                   $(element).kendoAutoComplete({
                       dataSource: tree,
                       dataTextField: "path",
                       change: function () {
                           var path = this.value();
                           console.log("selected path ", path);

                       },
                       filter: "startswith",
                       placeholder: "Select path...",
                       separator: ""
                   }).data("kendoAutoComplete");

                $(element)
                        .change(function () {
                            value($(this).val());
                            console.log("new value", value());
                        })
                       .val(value())
                    .on("keydown3", function (e) {
                        if (e.which === 110 || e.which === 190) {
                            var path = $(this).val() + ".";
                            console.log("show the auto complete", path);
                            var filtered = _.chain(tree)
                                .filter(function (t) {
                                    return t.parent === path;
                                })
                                .map(function (t) {
                                    return t.name;
                                })
                                .value();
                            console.log(filtered);
                            //input.setDataSource(dataSource);
                            dataSource.data(filtered);
                            input.refresh();
                        }
                    });
            });

    }

};

ko.bindingHandlers.commandWithParameter = {
    init: function (element, valueAccessor) {
        var command = valueAccessor();
        var callback = command.command;
        var parameter = command.commandParameter;

        var button = $(element);
        var completeText = button.data("complete-text") || button.html();
        button.click(function (e) {
            e.preventDefault();
            callback(parameter)
                .then(function () {
                    button.button("complete");
                    if (button.get(0).tagName == 'BUTTON' || button.get(0).tagName == 'A') {
                        button.html(completeText);
                    } else {
                        button.val(completeText);
                    }

                });
            if (button.data("loading-text")) {
                button.button("loading");
            }
        });
    }

};


ko.bindingHandlers.filter = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var value = valueAccessor(),
            bindingAccessor = allBindingsAccessor(),
            path = value.path,
            $element = $(element),
            $filterInput = $("<input type='search' class='search-query input-medium form-control' placeholder='Tapis..'>"),
            $serverLoadButton = $("<a href='/#' title='Carian server'><i class='add-on icon-search'></i><a>"),
            $form = $("<form class='form-search col-lg-4 col-lg-offset-8'>" +
                " <div class='input-group pull-right'>" +
                "<span class='input-group-addon'>" +
                " <span class='glyphicon glyphicon-remove'></span>" +
                "</span> " +
                "</div>" +
                " </form>"),
            pagedSearch = bindingAccessor.searchPaging;


        $form.find('span.input-group-addon').before($filterInput);
        if (pagedSearch) {
            $form.find('span.glyphicon-remove').after($serverLoadButton);
        }
        $element.before($form);

        $serverLoadButton.click(function (e) {
            e.preventDefault();
            var filter = $filterInput.val().toLowerCase(),
                tcs = new $.Deferred();
            if (!filter) {
                tcs.promise();
                return tcs.promise();
            }
            if (pagedSearch) {
                var query = {
                    "query": {
                        "bool": {
                            "must": [
                                {
                                    "query_string": { "query": filter }
                                }
                            ]
                        }
                    }
                };
                pagedSearch.query = query;
                return pagedSearch.search(query);
            }
            return tcs.promise();
        });



        var dofilter = function () {
            var $rows = $element.find(path),
                filter = $filterInput.val().toLowerCase();
            $rows.each(function () {
                var $tr = $(this);
                if ($tr.text().toLowerCase().indexOf(filter) > -1) {
                    $tr.show();
                } else {
                    $tr.hide();
                }
            });


        };

        var throttled = _.throttle(dofilter, 800);
        $filterInput.on('keyup', throttled).siblings('span.input-group-addon')
            .click(function () {
                $filterInput.val('');
                dofilter();
            });

        if ($filterInput.val()) {
            dofilter();
        }

    }
};

ko.bindingHandlers.serverPaging = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = valueAccessor(),
            entity = value.entity,
            query = value.query,
            list = value.list,
            map = value.map,
            $element = $(element),
            context = require('services/datacontext'),
            $pagerPanel = $('<div></div>'),
            $spinner = $('<img src="/Images/spinner-md.gif" alt="loading" class="absolute-center" />'),
            startLoad = function () {
                $spinner.show();
                $element.fadeTo("fast", 0.33);
            },
            endLoad = function () {
                $spinner.hide();
                $element.fadeTo("fast", 1);
            },
            setItemsSource = function (items) {
                if (map) {
                    items = _(items).map(map);
                }
                if (typeof list === "string") {
                    viewModel[list](items);
                }
                if (typeof list === "function") {
                    list(items);
                }
            },
            changed = function (page, size) {
                startLoad();
                context.loadAsync({
                    entity: entity,
                    page: page,
                    size: size,
                    includeTotal: true
                }, query)
                     .then(function (lo) {
                         setItemsSource(lo.itemCollection);
                         endLoad();
                     });
            };

        $element.after($pagerPanel).after($spinner)
            .fadeTo("slow", 0.33);

        var tcs = new $.Deferred();
        context.loadAsync({
            entity: entity,
            page: 1,
            size: 20,
            includeTotal: true
        }, query)
            .then(function (lo) {

                var options = {
                    element: $pagerPanel,
                    count: lo.rows,
                    changed: changed
                },
                    pager = new bespoke.utils.ServerPager(options);
                console.log(pager);
                setTimeout(function () {
                    setItemsSource(lo.itemCollection);
                    tcs.resolve(true);
                    endLoad();
                }, 500);

            });
        return tcs.promise();



    }
};


ko.bindingHandlers.searchPaging = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = valueAccessor(),
            entity = value.entity,
            query = value.query,
            executedQuery = value.initialQuery || {},
            list = value.list,
            map = value.map,
            searchButton = value.searchButton,
            $element = $(element),
            context = require('services/datacontext'),
            logger = require('services/logger'),
            cultures = require(objectbuilders.cultures),
            $pagerPanel = $('<div></div>'),
            $spinner = $('<img src="/Images/spinner-md.gif" alt="loading" class="absolute-center" />'),
            startLoad = function () {
                $spinner.show();
                $element.fadeTo("fast", 0.33);
            },
            endLoad = function () {
                $spinner.hide();
                $element.fadeTo("fast", 1);
            },
            setItemsSource = function (items) {

                _(items).each(function (v) {
                    v.pager = {
                        page: pager.page(),
                        size: pager.pageSize()
                    };
                });
                if (map) {
                    items = _(items).map(map);
                }
                if (typeof list === "string") {
                    viewModel[list](items);
                }
                if (typeof list === "function") {
                    list(items);
                }
            },
            pager = null,
            pageChanged = function (page, size) {
                startLoad();
                context.searchAsync({
                    entity: entity,
                    page: page,
                    size: size
                }, executedQuery)
                     .then(function (lo) {
                         setItemsSource(lo.itemCollection);
                         endLoad();
                     });
            },
            search = function (q, page, size) {
                executedQuery = q;
                var tcs1 = new $.Deferred();
                startLoad();
                context.searchAsync({
                    entity: entity,
                    page: page || 1,
                    size: size || 20
                }, q)
                    .then(function (lo) {
                        if (pager) {
                            pager.update(lo.rows);
                        } else {
                            var pagerOptions = {
                                element: $pagerPanel,
                                count: lo.rows,
                                changed: pageChanged
                            };
                            pager = new bespoke.utils.ServerPager(pagerOptions);

                        }

                        setTimeout(function () {
                            setItemsSource(lo.itemCollection);
                            tcs1.resolve(lo);
                            endLoad();
                        }, 500);

                    });
                return tcs1.promise();
            };

        //exposed the search function
        value.search = search;

        $element.after($pagerPanel).after($spinner)
            .fadeTo("slow", 0.33);

        if (searchButton) {
            $(document).on('click', searchButton, function (e) {
                e.preventDefault();
                if (!$(this).parents("form")[0].checkValidity()) {
                    logger.error(cultures.messages.FORM_IS_NOT_VALID);
                    return;
                }
                search(ko.toJS(query), 1, pager.pageSize());
            });

        }


        search(ko.toJS(executedQuery));
        return {
            search: search
        };

    }
};
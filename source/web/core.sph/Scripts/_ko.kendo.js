/// <reference path="knockout-3.2.0.debug.js" />
/// <reference path="underscore.js" />
/// <reference path="moment.js" />
/// <reference path="~/Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="~/Scripts/require.js" />
/// <reference path="~/kendo/js/kendo.all.js" />
/// <reference path="_pager.js" />
/// <reference path="/SphApp/services/datacontext.js" />
/// <reference path="/SphApp/objectbuilders.js" />



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
        var dd = $(element).data("kendoDropDownList");
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
            $(element).data("kendoDropDownList").value(modelValue);
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
            dd = $(element).data("kendoComboBox") ||
                $(element).kendoComboBox({
                    dataSource: allBindings.source()
                }).data("kendoComboBox");

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
            $(element).data("kendoComboBox").value(modelValue);
        }
    }
};


ko.bindingHandlers.decimal = {};
ko.bindingHandlers.money = {
    init: function (element, valueAccessor, allBindingsAccessor) {

        var value = valueAccessor(),
            allBindings = allBindingsAccessor(),
            decimal = function () {
                if (typeof allBindings.decimal === "undefined") {
                    return 2;
                }
                return parseInt(allBindings.decimal);
            },
            textbox = $(element),
            val = parseFloat(ko.unwrap(value) || "0"),
            fm = val.toFixed(decimal()).replace(/./g, function (c, i, a) {
                return i && c !== "." && !((a.length - i) % 3) ? "," + c : c;
            });


        if (element.tagName.toLowerCase() === "span") {
            textbox.text(fm);
            return;
        }

        textbox.val(fm);

        textbox.on("blur", function () {
            var tv = $(this).val().replace(/,/g, "");
            console.log(tv);
            value(parseFloat(tv));
        });

    },
    update: function (element, valueAccessor, allBindingsAccessor) {
        var value = valueAccessor(),
            allBindings = allBindingsAccessor(),
            decimal = function () {
                if (typeof allBindings.decimal === "undefined") {
                    return 2;
                }
                return parseInt(allBindings.decimal);
            },
             textbox = $(element),
             val = parseFloat(ko.unwrap(value) || "0"),
             fm = val.toFixed(decimal()).replace(/./g, function (c, i, a) {
                 return i && c !== "." && !((a.length - i) % 3) ? "," + c : c;
             });

        textbox.val(fm);

    }
};

///user moment format
ko.bindingHandlers.date = {
    init: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());

        if (!value) {
            $(element).text("");
            $(element).val("");
            return;
        }

        var dv = ko.unwrap(value.value),
            defaultFormat = (dv || "").indexOf("T") > -1 ? "YYYY-MM-DDTHH:mm:ss" : "YYYY-MM-DD",
            inputFormat = ko.unwrap(value.inputFormat) || defaultFormat,
            date = moment(dv, inputFormat),
            invalid = ko.unwrap(value.invalid) || "invalid date",
            format = ko.unwrap(value.format) || "DD/MM/YYYY";

        if (!value.format && typeof ko.unwrap(value) === "string") {
            dv = ko.unwrap(value);
            date = moment(dv);
        }

        $(element).on("change", function () {
            var nv = $(this).val();
            value.value(nv);
        });
        if (!dv) {
            $(element).text(invalid);
            $(element).val(invalid);
            return;
        }
        if (!date) {
            $(element).text("");
            $(element).val("");
            return;
        }
        if (date.year() === 1) { // DateTime.Min
            $(element).text("");
            $(element).val("");
            return;
        }


        var dateString = date.format(format).toString();
        if (dateString.indexOf("NaN") < 0) {
            $(element).text(dateString);
            $(element).val(dateString);
        }



    },
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());

        if (!value) {
            $(element).text("");
            $(element).val("");
            return;
        }

        var dv = ko.unwrap(value.value),
            defaultFormat = (dv || "").indexOf("T") > -1 ? "YYYY-MM-DDTHH:mm:ss" : "YYYY-MM-DD",
            inputFormat = ko.unwrap(value.inputFormat) || defaultFormat,
            date = moment(dv, inputFormat),
            invalid = ko.unwrap(value.invalid) || "invalid date",
            format = ko.unwrap(value.format) || "DD/MM/YYYY";

        if (!value.format && typeof ko.unwrap(value) === "string") {
            dv = ko.unwrap(value);
            date = moment(dv);
        }

        $(element).on("change", function () {
            var nv = $(this).val();
            value.value(nv);
        });
        if (!dv) {
            $(element).text(invalid);
            $(element).val(invalid);
            return;
        }
        if (!date) {
            $(element).text("");
            $(element).val("");
            return;
        }
        if (date.year() === 1) { // DateTime.Min
            $(element).text("");
            $(element).val("");
            return;
        }


        var dateString = date.format(format).toString();
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
             value = valueAccessor(),
             options = valueAccessor(),
                 extensions = [];
        if (options && typeof options === "object") {
            value = options.value;
            extensions = options.extensions;
        }

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
            select: function (e) {
                if (extensions.length === 0) {
                    return;
                }
                _(e.files).each(function (v) {
                    if (extensions.indexOf(v.extension) < 0) {
                        logger.error("Only " + extensions.join(",") + " files can be uploaded");
                        e.preventDefault();
                    }
                });
            },
            success: function (e) {
                logger.info("Your file has been " + e.operation);

                var storeId = e.response.storeId,
                    uploaded = e.operation === "upload",
                    removed = e.operation !== "upload",
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
    init: function (element, valueAccessor, allBindingsAccessor) {
        var value = valueAccessor(),
            $input = $(element),
            allBindings = allBindingsAccessor(),
            currentValue = ko.unwrap(value),
            date = moment(currentValue, "DD/MM/YYYY"),
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

        if (typeof allBindings.enable === "boolean") {
            if (!allBindings.enable) {
                picker.enable(false);
            }
        }
        if (typeof allBindings.enable === "function" && typeof allBindings.enable.subscribe === "function") {
            allBindings.enable.subscribe(function (enable) {
                picker.enable(enable);
            });
        }

        if (!date) {
            picker.value(null);
            return;
        }

        if (date.year() === 1) { // DateTime.Min
            picker.value(null);
            return;
        }

        picker.value(date.toDate());
        if (typeof currentValue === "undefined" && typeof value === "function") {
            value(date.format("YYYY-MM-DD"));
        }
    },
    update: function (element, valueAccessor, allBindingsAccessor) {
        var $input = $(element),
            allBindings = allBindingsAccessor();
        if ($input.data("stop") === "true") return;

        var value = valueAccessor(),
            modelValue = ko.utils.unwrapObservable(value),
            date = moment(modelValue),
            picker = $input.data("kendoDatePicker");

        if (!date) {
            picker.value(null);
            return;
        }
        if (date.year() === 1) { // DateTime.Min
            picker.value(null);
            return;
        }

        if (typeof allBindings.enable === "boolean") {
            if (!allBindings.enable) {
                picker.enable(false);
            }
        }
        if (typeof allBindings.enable === "function" && typeof allBindings.enable.subscribe === "function") {
            allBindings.enable.subscribe(function (enable) {
                picker.enable(enable);
            });
        }
        picker.value(date.toDate());

    }
};


///user moment format
ko.bindingHandlers.kendoDateTime = {
    init: function (element, valueAccessor) {
        var value = valueAccessor(),
            $input = $(element),
            currentValue = ko.utils.unwrapObservable(value),
            date = moment(currentValue, "DD/MM/YYYY "),
            changed = function (e) {
                console.log(e);
                var nv = this.value();
                if (typeof nv == "string") {
                    date = moment(nv, "DD/MM/YYYY hh:mm");
                } else {
                    date = moment(nv);
                }
                // DO NOT fire update
                $input.data("stop", "true");
                value(date.format());
                $input.data("stop", "false");

            },
            picker = $input.kendoDateTimePicker({ format: "dd/MM/yyyy HH:mm", change: changed }).data("kendoDateTimePicker");

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
        if ($input.data("stop") === "true") return;

        var value = valueAccessor(),
            modelValue = ko.utils.unwrapObservable(value),
            date = moment(modelValue),
            picker = $input.data("kendoDateTimePicker");

        if (!date) {
            picker.value(null);
            return;
        }
        if (date.year() === 1) { // DateTime.Min
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
            $(element).removeClass("k-state-disabled");
        } else {
            $(element).addClass("k-state-disabled");
        }
    },
    update: function (element, valueAccessor) {
        var value = valueAccessor();
        var enable = ko.utils.unwrapObservable(value);
        if (enable) {
            $(element).removeClass("k-state-disabled");
            $(element).removeAttr("disabled");
        } else {
            $(element).addClass("k-state-disabled");
            $(element).attr("disabled", "disabled");
        }
    }
};

ko.bindingHandlers.command = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var action = valueAccessor(),
            $button = $(element),
            allBindings = allBindingsAccessor(),
            inputValue = $button.val(),
            logger = require("services/logger");

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

        var $spinner = $("<i class='fa fa-spin fa-spinner'></i>").hide(),
            $warning = $("<i class='fa fa-warning' style='color:red'></i>").hide();
        $button.append($spinner);
        $button.append($warning);

        $button.click(function (e) {
            e.preventDefault();
            if (this.form) {
                if (!this.form.checkValidity()) return;
            }
            $spinner.show();
            $warning.hide();

            action()
                .fail(function (err, o, message) {
                    $button
                       .button("complete")
                       .prop("disabled", false)
                       .val(inputValue)
                       .removeClass("btn-disabled");
                    $spinner.hide();
                    $warning.show();
                    if (err.status === 404) {
                        logger.error(message);
                        logger.error(err.statusText);
                        return;
                    }
                    if (err.responseText) {
                        logger.error(err.responseText);
                        console.error(err.responseText);
                    }
                    if (err.responseJSON) {
                        logger.error(JSON.stringify(err.responseJSON));
                        console.error(err.responseJson);
                    }
                })
                .done(function () {
                    $button
                        .button("complete")
                        .prop("disabled", false)
                        .val(inputValue)
                        .removeClass("btn-disabled");
                    $spinner.hide();
                });
            if ($button.data("loading-text")) {
                $button.button("loading");
            }
            $button.addClass("btn-disabled").prop("disabled", true);

        });
    }
};

ko.bindingHandlers.field = {
    init: function () {
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
                type = allBindings.field,
                entity = allBindings.entity;
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
                    action(accessor[prop], type, entity);
                }
            } else {
                action(accessor, type, entity);
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
                    if (button.get(0).tagName === "BUTTON" || button.get(0).tagName === "A") {
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
            tooltip = value.tooltip || "Type to filter current page or type and [ENTER] to search the whole view",
            $element = $(element),
            $filterInput = $("<input data-toggle=\"tooltip\" title=\"" + tooltip + "\" type=\"search\" class=\"search-query input-medium form-control\" placeholder=\"Filter.. \">"),
            $serverLoadButton = $("<a href='/#' title='Carian server'><i class='add-on icon-search'></i><a>"),
            $form = $("<form class='form-search row'>" +
                " <div class='input-group pull-right' style='width:300px'>" +
                "<span class='input-group-addon'>" +
                " <span class='glyphicon glyphicon-remove'></span>" +
                "</span> " +
                "</div>" +
                " </form>"),
            pagedSearch = bindingAccessor.searchPaging;


        $form.find("span.input-group-addon").before($filterInput);
        if (pagedSearch) {
            $form.find("span.glyphicon-remove").after($serverLoadButton);
        }
        $element.before($form);

        $form.submit(function (e) {
            e.preventDefault();
            var filter = $filterInput.val().toLowerCase(),
                tcs = new $.Deferred();
            if (!filter) {
                return tcs.promise();
            }
            if (pagedSearch && typeof pagedSearch.query !== "undefined" && typeof pagedSearch.query.filterAndSearch === "function") {
                return pagedSearch.query.filterAndSearch(filter);
            }
            return tcs.promise();
        });



        var dofilter = function () {
            var $rows = $element.find(path),
                filter = $filterInput.val().toLowerCase();
            $rows.each(function () {
                var $tr = $(this),
                    text = $tr.text().toLowerCase().trim();
                if (!text) {
                    $("input", $tr).each(function (i, v) { text += " " + $(v).val() });
                    text = text.toLowerCase().trim();
                }
                if (text.indexOf(filter) > -1) {
                    $tr.show();
                } else {
                    $tr.hide();
                }
            });


        },
        throttled = _.throttle(dofilter, 800);

        $filterInput.on("keyup", throttled).siblings("span.input-group-addon")
            .click(function () {
                $filterInput.val("");
                dofilter();
            });

        if ($filterInput.val()) {
            dofilter();
        }
        $filterInput.tooltip();

    }
};

ko.bindingHandlers.serverPaging = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = valueAccessor(),
            entity = value.entity,
            query = value.query,
            list = value.list,
            map = value.map,
            pagerHidden = value.pagerHidden || false,
            $element = $(element),
            context = require("services/datacontext"),
            $pagerPanel = $("<div></div>"),
            $spinner = $("<img src=\"/Images/spinner-md.gif\" alt=\"loading\" class=\"absolute-center\" />"),
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
                changed: changed,
                hidden: pagerHidden
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
            executedQuery = value.query || value.initialQuery || {},
            list = value.list,
            map = value.map,
            pagerHidden = value.pagerHidden || false,
            searchButton = value.searchButton,
            $element = $(element),
            context = require("services/datacontext"),
            logger = require("services/logger"),
            cultures = require(objectbuilders.cultures),
            $pagerPanel = $("<div></div>"),
            $spinner = $("<img src=\"/Images/spinner-md.gif\" alt=\"loading\" class=\"absolute-center\" />"),
            startLoad = function () {
                $spinner.show();
                $element.fadeTo("fast", 0.33);
            },
            endLoad = function () {
                $spinner.hide();
                $element.fadeTo("fast", 1);
            },
            pager = null,
            setItemsSource = function (items) {

                if (!pagerHidden) {
                    _(items).each(function (v) {
                        v.pager = {
                            page: pager.page(),
                            size: pager.pageSize()
                        };
                    });
                }

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
                                changed: pageChanged,
                                hidden: pagerHidden
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
            },
            filterAndSearch = function (text) {
                var q = JSON.parse(ko.toJSON(value.query || value.initialQuery)),
                    q2 = {
                        "from": 0,
                        "size": 20,
                        "query": {}
                    };
                q.query = q.query || {};
                q.query.filtered = q.query.filtered || {};

                q2.query.filtered = q.query.filtered;
                q2.query.filtered.query = {
                    "query_string": {
                        "default_field": "_all",
                        "query": text
                    }
                };
                q2.sort = q.sort;
                pager.destroy();
                pager = null;
                search(q2);
            };

        //exposed the search function
        query.search = search;
        query.filterAndSearch = filterAndSearch;

        $element.after($pagerPanel).after($spinner)
            .fadeTo("slow", 0.33);

        if (searchButton) {
            $(document).on("click", searchButton, function (e) {
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
            search: search,
            filterAndSearch: filterAndSearch
        };

    }
};
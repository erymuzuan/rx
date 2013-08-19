/// <reference path="modernizr-2.6.2.js" />
/// <reference path="knockout-2.3.0.debug.js" />
/// <reference path="underscore.js" />
/// <reference path="moment.js" />
/// <reference path="~/Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="~/kendo/js/kendo.all.js" />

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

ko.bindingHandlers.kendoComboBoxValue = {
    init: function (element, valueAccessor) {
        var value = valueAccessor();
        var currentModelValue = ko.utils.unwrapObservable(value);
        var dd = $(element).data('kendoComboBox');
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
        var value = ko.utils.unwrapObservable(valueAccessor());
        var date = moment(value.value());
        if (date.year() == 1) { // DateTime.Min
            $(element).text("");
            $(element).val("");
        } else {
            var dateString = date.format(value.format).toString();
            if (dateString.indexOf("NaN") < 0) {
                $(element).text(dateString);
                $(element).val(dateString);
            }
        }


        $(element).on("change", function () {
            var nv = $(this).val();
            value.value(nv);
        });
    },
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        var date = moment(value.value());
        if (date.year() == 1) { // DateTime.Min
            $(element).text("");
            $(element).val("");

        } else {
            var dateString = date.format(value.format).toString();
            if (dateString.indexOf("NaN") < 0) {
                $(element).text(dateString);
                $(element).val(dateString);
            }

        }
    }
};

///user moment format
ko.bindingHandlers.kendoDate = {
    init: function (element, valueAccessor) {
        var value = valueAccessor();
        var currentValue = ko.utils.unwrapObservable(value);
        var date = moment(currentValue);

        var picker = $(element).data("kendoDatePicker") ||
            $(element).kendoDatePicker({ format: "dd/MM/yyyy" }).data("kendoDatePicker");

        if (null === date || date.year() === 1) { // DateTime.Min
            picker.value(null);
        } else {
            picker.value(date.toDate());
            value(date);
        }

        $(element).on("change", function () {
            var nv = $(this).val();
            if (typeof nv == "string") {
                date = moment(nv, "DD/MM/YYYY");
            } else {
                date = moment(nv);
            }
            // DO NOT fire update
            $(element).data("stop", "true");
            valueAccessor()(date.format("YYYY-MM-DD"));
            $(element).data("stop", "false");
        });
    },
    update: function (element, valueAccessor) {
        if ($(element).data("stop") == "true") return;
        var value = valueAccessor();
        var modelValue = ko.utils.unwrapObservable(value);

        var date = moment(modelValue);
        var picker = $(element).data("kendoDatePicker");
        if (date.year() == 1) { // DateTime.Min
            picker.value(null);
        } else {
            picker.value(date.toDate());
        }
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
            allBindings = allBindingsAccessor();

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

            var $spinner = $("<i class='icon-spin icon-spinner icon-large'></i>");
            $spinner.css({ "margin-left": -($button.width() / 2) - 16, "position" : "fixed" , "margin-top" : "10px" });
            $button.after($spinner).show();
            action()
                .then(function () {
                    $button
                        .button("complete")
                        .prop('disabled', true)
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

ko.bindingHandlers.unwrapClick = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var action = valueAccessor(),
            button = $(element),
            allBindings = allBindingsAccessor();


        button.click(function (e) {
            e.preventDefault();
            var prop = allBindings.property;
            var accessor = allBindings.accessor;
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
                    action(accessor[prop]);
                }
            } else {
                action(accessor);
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


ko.bindingHandlers.cssAutoComplete = {
    init: function (element, valueAccessor) {
        var value = valueAccessor();

        var data = ['btn', 'btn-warning', 'btn-success', 'btn-link'];

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
    init: function (element, valueAccessor) {
        var value = valueAccessor(),
            path = value.path,
            $element = $(element),
            $filterInput = $("<input type='search' class='search-query input-medium' placeholder='Tapis..'>"),
            $form = $("<form class='form-search'>" +
            " <div class='input-append pull-right'>" +
            " <i class='add-on icon-remove'></i>" +
            "</div>" +
            " </form>");
        $form.find('i').before($filterInput);
        $element.before($form);



        var dofilter = function () {
            var $rows = $element.find(path);
            var filter = $filterInput.val().toLowerCase();
            $rows.each(function () {
                var $tr = $(this);
                if ($tr.text().toLowerCase().indexOf(filter) > -1) {
                    $tr.slideDown();
                } else {
                    $tr.slideUp();
                }
            });
        };

        var throttled = _.throttle(dofilter, 800);
        $filterInput.on('keyup', throttled).siblings('.icon-remove')
            .click(function () {
                $filterInput.val('');
                dofilter();
            });

        if ($filterInput.val()) {
            dofilter();
        }

    }
};
/// <reference path="modernizr-2.6.2.js" />
/// <reference path="knockout-2.2.1.debug.js" />
/// <reference path="underscore.js" />
/// <reference path="moment.js" />
/// <reference path="~/Scripts/jquery-2.0.2.intellisense.js" />

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

        if (date.year() == 1) { // DateTime.Min
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
            valueAccessor()(date.format("DD/MM/YYYY"));
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
    init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        var action = valueAccessor(),
            button = $(element),
            allBindings = allBindingsAccessor();

        var completeText = button.data("complete-text") || button.html();

        button.click(function (e) {
            e.preventDefault();
            action()
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

ko.bindingHandlers.unwrapClick = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
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


ko.bindingHandlers.pathAutoComplete = {
    init: function (element, valueAccessor) {
        var command = valueAccessor();
        var value = command.value;
        var type = command.type;

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

                var input = $(element).data("kendoAutoComplete") ||
                   $(element).kendoAutoComplete({
                       dataSource: data,
                       change: function () {
                           var path = this.value();
                           console.log("selected path " , path);
                           value(path);

                           var filtered = _.chain(tree)
                               .filter(function (t) {
                                   return t.parent === path;
                               })
                               .map(function (t) {
                                   return t.name;
                               })
                               .value();
                           console.log(filtered);
                           var dataSource = new kendo.data.DataSource({
                               data: filtered
                           });
                           input.setDataSource(dataSource);
                       },
                       filter: "startswith",
                       placeholder: "Select path...",
                       separator: "."
                   }).data("kendoAutoComplete")
                    .value(value());
            });

    }

};

ko.bindingHandlers.commandWithParameter = {
    init: function (element, valueAccessor) {
        var command = valueAccessor();
        var callback = command.command;
        var parameter = command.commandParameter;

        var button = $(element);
        button.click(function (e) {
            e.preventDefault();
            callback(parameter)
                .then(function () {

                    if (button.data("complete-text")) {
                        button.button("complete");
                    }
                });
            if (button.data("loading-text")) {
                button.button("loading");
            }
        });
    }

};
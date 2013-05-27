/// <reference path="modernizr-2.6.2.js" />
/// <reference path="knockout-2.2.1.debug.js" />
/// <reference path="moment.js" />
/// <reference path="~/Scripts/jquery-1.9.1.intellisense.js" />
$(function () {

    if (!Modernizr.inputtypes.date) {
        $(function () {
            $('input[type="date"]')
                .css({ "min-width": "100px", "width": "200px" })
                .kendoDatePicker({
                    format: "yyyy-MM-dd"
                    });
        });
    }

    $('.k-datepicker')
        .css({ "min-width": "100px", "width": "200px" })
        .kendoDatePicker({
            format: "yyyy-MM-dd"
        });
    $('.k-datetimepicker')
        .css({ "min-width": "100px", "width": "200px" })
        .kendoDateTimePicker({
            format: "yyyy-MM-dd HH:mm"
        });

    $('.k-timepicker')
        .css({ "min-width": "100px", "width": "200px" })
        .kendoTimePicker({});

    $('.k-combobox')
        .css({ "min-width": "300px!important", "width": "300px" })
        .kendoComboBox();

    $('.k-dropdown')
        .css({ "min-width": "300px!important", "width": "300px" })
        .kendoDropDownList();


});




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


///user moment format
ko.bindingHandlers.date = {
    init: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        var date = moment(value.value());
        $(element).text(date.format(value.format));
        
        $(element).on("change", function () {
            var nv = $(this).val();
            value.value(nv);
        });
    },
    update: function (element, valueAccessor) {
      var value = ko.utils.unwrapObservable(valueAccessor());
      $(element).val(value.value());
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
    init: function (element, valueAccessor) {
        var callback = valueAccessor();
        var button = $(element);
        button.click(function(e) {
            e.preventDefault();
            callback()
                .then(function() {

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
ko.bindingHandlers.commandWithParameter = {
    init: function (element, valueAccessor) {
        var command = valueAccessor();
        var callback = command.command;
        var parameter = command.commandParameter;
        
        var button = $(element);
        button.click(function(e) {
            e.preventDefault();
            callback(parameter)
                .then(function() {

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
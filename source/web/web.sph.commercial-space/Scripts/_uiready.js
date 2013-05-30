/// <reference path="bootstrap.js" />
/// <reference path="google-maps-3-vs-1-0-vsdoc.js" />
/// <reference path="bootstrap-datepicker.js" />
/// <reference path="jquery.validate-vsdoc.js" />
/// <reference path="jquery-1.9.1.intellisense.js" />
/// <reference path="moment.js" />
/// <reference path="underscore.js" />
/// <reference path="modernizr-2.6.2.js" />


var _uiready = function () {
    var init = function (view) {
        $(view).find('.datepicker').datepicker({ format: 'dd/mm/yyyy' });

        if (!Modernizr.inputtypes.date) {
            $(function () {
                $('input[type="date"]')
                    .css({ "min-width": "100px", "width": "200px" })
                    .kendoDatePicker({
                        format: "yyyy-MM-dd"
                    });
            });
        }

        $(view).find('.k-datepicker')
            .css({ "min-width": "100px", "width": "200px" })
            .kendoDatePicker({
                format: "yyyy-MM-dd"
            });
        $(view).find('.k-datetimepicker')
            .css({ "min-width": "100px", "width": "200px" })
            .kendoDateTimePicker({
                format: "yyyy-MM-dd HH:mm"
            });

        $(view).find('.k-timepicker')
            .css({ "min-width": "100px", "width": "200px" })
            .kendoTimePicker({});

        $(view).find('.k-combobox')
            .css({ "min-width": "300px!important", "width": "300px" })
            .kendoComboBox();

        $(view).find('.k-dropdown')
            .css({ "min-width": "300px!important", "width": "300px" })
            .kendoDropDownList();


    };
    return {
        init: init
    };

}();

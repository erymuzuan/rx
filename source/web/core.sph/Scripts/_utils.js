﻿(function (window, $) {
    window.bespoke = window.bespoke || {};
    bespoke.utils = bespoke.utils || {};
    bespoke.utils.form = bespoke.utils.form || {};
    bespoke.utils.form.checkValidity = function (button) {
        if (button.form)
            return button.form.checkValidity();
        var form = $(button).attr("form");
        if (form) {
           return document.getElementById(form).checkValidity();
        }
        throw "cannot find the form for the button";
    };


    window.console = window.console || {};
    window.console.dir = window.console.dir || function (){};


})(window, jQuery);
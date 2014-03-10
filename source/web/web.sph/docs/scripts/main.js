/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />


define([], function () {

    $('#applicationHost').load('/docs/overview.html');
    $('#sidebar').load('/docs/sidebar.html');
    $('#sidebar').on('click', 'a', function(e) {
        e.preventDefault();
        e.stopPropagation();

        $('#applicationHost').load( this.href);
    });
});
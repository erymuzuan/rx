/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />


define(['types'], function (types) {

    $('#applicationHost').load('/docs/overview.html');
    $('#sidebar').load('/docs/sidebar.html');
    $('#sidebar').on('click', 'a', function (e) {
        e.preventDefault();
        e.stopPropagation();

        $('#applicationHost').load(this.href);
    });
    $('#applicationHost').on('click', 'a', function (e) {
        if (this.href.indexOf("/docs/#") < 0) {
            return;
        }
        e.preventDefault();
        e.stopPropagation();

        $('#applicationHost').load(this.href.replace("#", ""));
    });

    var paths = _(types.types).map(function (v) { return { path: v }; }),
                members = new Bloodhound({
                    datumTokenizer: function (d) {
                        return d.path.split(/s+/);
                    },
                    queryTokenizer: function (s) {
                        return s.split(/\./);
                    },
                    local: paths
                });
    members.initialize();

    $('#search').typeahead({
        minLength: 0,
        highlight: true,
    },
        {
            name: 'types-search',
            displayKey: 'path',
            source: members.ttAdapter()
        })
        .on('typeahead:closed', function () {
            console.log($(this).val());

            $('#applicationHost').load($(this).val() + ".html");
        });
});
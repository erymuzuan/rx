/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />

define(['types'], function (types) {

    var nav = function (href2) {
        $('#applicationHost').load(href2, null, function (responseText, textStatus) {
            if (textStatus === "error") {
                $('#applicationHost').html('<div class="alert alert-danger alert-dismissable">Cannot find the content for : ' + href2 + '</div>');
                return;
            }
            $('#applicationHost img').addClass('img-thumbnail');
            $('#applicationHost a').prepend('<i class="fa fa-link">');
        });
    },
        mapTopic = function (topicHash) {
            topicHash = topicHash.toLowerCase();
            if (topicHash.indexOf("trigger.setup") > -1)return "Trigger.html";

            if (topicHash.indexOf("entity.details") > -1)return "EntityDefinition.html";
            if (topicHash.indexOf("entity.form.designer") > -1)return "EntityForm.html";
            if (topicHash.indexOf("entity.view.designer") > -1)return "EntityView.html";
            if (topicHash.indexOf("entity.operation.details") > -1)return "EntityOperation.html";
            if (topicHash.indexOf("entityview") > -1) return "EntityView.html";
            if (topicHash.indexOf("entityform") > -1) return "EntityForm.html";
            if (topicHash.indexOf("entitydefinition") > -1) return "EntityDefinition.html";
            if (topicHash.indexOf("entityoperation") > -1) return "EntityOperation.html";

            if (topicHash.indexOf("reportdefinition") > -1)return "ReportDefinition.html";
            if (topicHash.indexOf("reportdelivery") > -1)return "reportdelivery.html";

            if (topicHash.indexOf("workflowdefinition.visual") > -1) return "WorkflowDefinition.html";
            if (topicHash.indexOf("workflow.definition.visual") > -1)return "WorkflowDesigner.html";
            if (topicHash.indexOf("workflow.debugger") > -1)return "Breakpoint.html";
            return "Overview.html";
        },
        topic = window.location.hash;
    $('#applicationHost').load('/docs/overview.html');
    $('#sidebar').load('/docs/sidebar.html');
    $('#sidebar').on('click', 'a', function (e) {
        e.preventDefault();
        e.stopPropagation();
        nav(this.href);
    });
    $('#applicationHost').on('click', 'a', function (e) {
        var $anchor = $(this),
            href = $anchor.attr('href');
        if (href.indexOf("http://") > -1) {
            return;
        }
        if (href.indexOf("https://") > -1) {
            return;
        }

        $('#applicationHost').load(href, null, function (responseText, textStatus) {
            if (textStatus === "error") {
                $('#applicationHost').html('<div class="alert alert-danger alert-dismissable">Cannot find the content for : ' + href + '</div>');
                return;
            }
            $('#applicationHost img').addClass('img-thumbnail');
            $('#applicationHost a').prepend('<i class="fa fa-link">');
            console.log("downloaded %s", this.href);
        });
        e.preventDefault();
        e.stopPropagation();

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

    if (topic) {
        var g = mapTopic(topic);
        nav(g);
    }
});
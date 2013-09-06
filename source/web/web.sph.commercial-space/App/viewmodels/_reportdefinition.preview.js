/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/nprogress.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/_task.js" />


define([],
    function() {

        var isBusy = ko.observable(false),
            rdl = ko.observable(),
            activate = function (reportDefinition) {
                rdl(reportDefinition);
            },
            showPreview = function() {
                $('#preview-parameters-dialog').modal();
                return Task.fromResult(true);
            },
            executePreview = function() {
                var $layout = $('#report-preview-panel');
                $layout.html('<img src="/Images/spinner-md.gif" alt="loading" class="absolute-center" />');

                NProgress.start();
                var tcs = new $.Deferred();
                var data = ko.mapping.toJSON(rdl);
                $.post("/App/ReportDefinitionPreview/", data, "html")
                    .done(function (html) {
                        NProgress.done();
                        $layout.html(html);
                        tcs.resolve(true);
                    })
                    .fail(function(error) {
                        $layout.html(error.responseText);
                    });

                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            rdl: rdl,
            showPreview: showPreview,
            executePreview : executePreview
        };

        return vm;

    });

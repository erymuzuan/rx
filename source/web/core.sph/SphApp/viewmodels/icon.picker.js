/// <reference path="../../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(["services/datacontext", "services/logger", "plugins/dialog"],
    function (context, logger, dialog) {

        var self = this,
            icons = ko.observableArray(),
            icon = ko.observable(),
            activate = function () {

            },
            attached = function (view) {
                self = this;
                $(view).on("click", "div.fa-hover>a", function (e) {
                    e.preventDefault();
                    icon("fa " + $(this).text().replace("(alias)","").trim());
                    console.log(icon());
                    dialog.close(self, "OK");

                });
                $("div.fa-hover>span").css("cursor","pointer");
                $(view).on("click", "div.fa-hover>span", function (e) {
                    e.preventDefault();
                    var span = $(this),
                        css = "";
                    if(span.hasClass("glyphicon")){
                        css = span.attr("class").replace("glyphicon ");
                    }else{
                        css = span.text();
                    }
                    icon("glyphicon " + css.trim());
                    console.log(icon());
                    dialog.close(self, "OK");

                });
            },
            okClick = function () {
                dialog.close(this, "OK");

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };

        var vm = {
            attached: attached,
            activate: activate,
            icon: icon,
            icons: icons,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });

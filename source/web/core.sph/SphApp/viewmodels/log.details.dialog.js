/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(["plugins/dialog"],
    function (dialog) {

        const okClick = function () {
            dialog.close(this, "OK");
        },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            },
            attached = function () {
                $(document).on("mouseenter", ".view-model-modal .modal-header", function (e) {
                    e.preventDefault();
                    const elem = $(this).parents(".view-model-modal"),
                        hasButton = elem.data("has-fullscreen");

                    if (!hasButton) {

                        elem.find("div.modal-header>button").
                            after(`<a class="pull-right fullscreen-dialog" id="fullscreen-dialog" data-dialog="${elem.attr("id")
                        }" href="#" title="Maximize this dialog window" style="margin-right: 10px; color: gray"><i class="fa fa-arrows-alt"></i></a>`);
                        elem.data("has-fullscreen", true);
                    }

                });

                $(document).on("click", "a.fullscreen-dialog", function (e) {
                    e.preventDefault();

                    const width = $(document).width(),
                        height = $(document).height();
                    $("div.modal-body").height(height - 250);
                    $("div.modal-dialog").width(width - 100);
                    $("section.view-model-modal").css({ top: -70, left: (400 - ($("div.modal-dialog").width()/2)) });
                });
            };

        const vm = {
            attached: attached(),
            log: ko.observable(),
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });

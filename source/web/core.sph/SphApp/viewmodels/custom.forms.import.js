/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(["plugins/dialog",objectbuilders.logger],
    function(dialog, logger) {

        var okClick = function(data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }

            },
            cancelClick = function() {
                dialog.close(this, "Cancel");
            },
        attached = function (view) {
            $("#import-custom-forms").kendoUpload({
                async: {
                    saveUrl: "/custom-forms/upload",
                    autoUpload: true
                },
                multiple: false,
                error: function (e) {
                    logger.logError(e, e, this, true);
                },
                success: function (e) {
                    if (!e.response.success) {
                        logger.error(e.response.message);
                        return;
                    }
                    var uploaded = e.operation === "upload";
                    if (uploaded) {
                    
                    }
                }
            });
        };

        var vm = {
            okClick: okClick,
            attached: attached,
            cancelClick: cancelClick
        };


        return vm;

    });

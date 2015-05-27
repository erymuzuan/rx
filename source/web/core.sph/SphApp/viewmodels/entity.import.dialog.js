/// <reference path="../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(["plugins/dialog", objectbuilders.datacontext],
    function (dialog, context) {

        var entity = ko.observable(new bespoke.sph.domain.EntityDefinition()),
        okClick = function (data, ev) {
            if (bespoke.utils.form.checkValidity(ev.target)) {
                dialog.close(this, "OK");
            }

        },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            },
            attached = function (view) {
                $("#import").kendoUpload({
                    async: {
                        saveUrl: "/entity-definition/upload",
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
                            var ed = e.response.ed,
                                o = context.toObservable(ed);
                            entity(o);
                        }
                    }
                });
            };

        var vm = {
            entity: entity,
            okClick: okClick,
            cancelClick: cancelClick,
            attached: attached
        };


        return vm;

    });

/// <reference path="../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(["plugins/dialog", objectbuilders.datacontext],
    function (dialog, context) {

        var entity = ko.observable(),
            zip = ko.observable(),
            folder = ko.observable(),
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }

            },
            importPackage = function () {
                var data = JSON.stringify({"zip": ko.unwrap(zip), "folder": ko.unwrap(folder)});

                return context.post(data, "/entity-definition/import")
                    .then(function (result) {


                    });
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
                        entity(null);
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
                            zip(e.response.zip);
                            folder(e.response.folder);
                        }
                    }
                });
            };

        return  {
            entity: entity,
            okClick: okClick,
            cancelClick: cancelClick,
            importPackage: importPackage,
            zip: zip,
            folder: folder,
            attached: attached
        };


    });

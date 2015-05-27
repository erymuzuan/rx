/// <reference path="../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(["services/datacontext", "services/logger"],
    function(context, logger) {

        var
            entities = ko.observableArray(),
            isBusy = ko.observable(false),
            activate = function() {

            },
            attached = function(view) {
                
                $("#import").kendoUpload({
                    async: {
                        saveUrl: "/entity-definition/import",
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
                            entities.push(o);
                        }
                    }
                });
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            entities: entities,
            toolbar : {
                addNew: {
                    location: "#/entity.details/0",
                    caption: "Add New Custom Entity"
                }
            }
        };

        return vm;

    });

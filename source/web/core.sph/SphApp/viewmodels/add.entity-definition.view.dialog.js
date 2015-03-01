
define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router, objectbuilders.system, objectbuilders.app, 'plugins/dialog'],
    function (context, logger, router, system, app, dialog) {

        var errors = ko.observableArray(),
            entity = ko.observable(new bespoke.sph.domain.EntityDefinition()),
            view = ko.observable(new bespoke.sph.domain.EntityView({ WebId: system.guid() })),
            activate = function (entityid, viewid) {
                console.log("view dialog activate");
                entityid = "blog";
                viewid = "0";

                var query = String.format("Id eq '{0}'", entityid),
                    tcs = new $.Deferred();

                context.loadOneAsync("EntityDefinition", query)
                    .done(function (b) {
                        entity(b);
                        window.typeaheadEntity = b.Name();
                        if (viewid === "0") {
                            tcs.resolve(true);
                        }

                    });


                view(new bespoke.sph.domain.EntityView({ WebId: system.guid() }));
                view().IconStoreId("sph-img-list");

                view().Name.subscribe(function (v) {
                    view().Route(v.toLowerCase().replace(/\W+/g, "-"));

                });
         
                view().EntityDefinitionId(entityid);
                return tcs.promise();

            },
            attached = function () {
            },
            okClick = function(data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }
            },
            cancelClick = function() {
                dialog.close(this, "Cancel");
            };

        var vm = {
            errors: errors,
            attached: attached,
            activate: activate,
            view: view,
            entity: entity,
            okClick: okClick,
            cancelClick: cancelClick
        };

        return vm;

    });

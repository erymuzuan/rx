/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />

bespoke.sph.domain.ReceivePortPartial = function (port) {

    const system = require("durandal/system"),
        isWizardOk = ko.computed(function () {
            const ok = ko.unwrap(port.Name) && ko.unwrap(port.Formatter) && ko.unwrap(port.Entity);
            if ( port.TextFormatter() && ko.isComputed(port.TextFormatter().isWizardOk))
                return ok && port.TextFormatter().isWizardOk();
            return ok;
        }),
        removeHeaderFieldMapping = function (child) {
            var self = this;
            return function () {
                self.FieldMappingCollection.remove(child);
            };
        },
        addHeaderFieldMapping = function () {
            const tcs = new $.Deferred(),
                br = new bespoke.sph.domain.HeaderFieldMapping({ WebId: system.guid() }),
                self = this;

            require(["viewmodels/receive.port.header.field.dialog", "durandal/app"], function (dialog, app) {
                dialog.field(br);
                dialog.port(self);
                app.showDialog(dialog)
                    .done(function (result) {
                        if (result === "OK") {
                            self.FieldMappingCollection.push(br);
                            tcs.resolve(br);
                        } else {
                            tcs.resolve(false);
                        }
                    });
            });

            return tcs.promise();
        },
        addUriFieldMapping = function () {
            const tcs = new $.Deferred(),
                br = new bespoke.sph.domain.UriFieldMapping({ WebId: system.guid() }),
                self = this;

            require(["viewmodels/receive.port.uri.field.dialog", "durandal/app"], function (dialog, app) {
                dialog.field(br);
                dialog.port(self);
                app.showDialog(dialog)
                    .done(function (result) {
                        if (result === "OK") {
                            self.FieldMappingCollection.push(br);
                            tcs.resolve(br);
                        } else {
                            tcs.resolve(false);
                        }
                    });
            });

            return tcs.promise();
        },
        editHeaderFieldMapping = function (br) {
            var self = this;
            return function () {
                require(["viewmodels/receive.port.header.field.dialog", "durandal/app"], function (dialog, app) {
                    var clone = ko.mapping.fromJS(ko.mapping.toJS(br));
                    dialog.location(clone);
                    dialog.port(self);
                    app.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {
                                self.FieldMappingCollection.replace(br, clone);
                            }
                        });
                });
            };
        },
        addReferencedAssembly = function () {
            var self = this;
            require(["viewmodels/assembly.dialog", "durandal/app"], function (dialog, app2) {
                app2.showDialog(dialog)
                    .done(function (result) {
                        if (!result) return;
                        if (result === "OK") {
                            _(dialog.selectedAssemblies()).each(function (v) {
                                self.ReferencedAssemblyCollection.push(v);
                            });
                        }
                    });

            });
        },
        editReferencedAssembly = function (dll) {
            console.warn(`Not implemented ${dll}`);
        },
        removeReferencedAssembly = function (dll) {
            var self = this;
            return function () {
                self.ReferencedAssemblyCollection.remove(dll);
            };
        };

    return {
        editReferencedAssembly: editReferencedAssembly,
        removeReferencedAssembly: removeReferencedAssembly,
        addReferencedAssembly: addReferencedAssembly,
        editHeaderFieldMapping: editHeaderFieldMapping,
        removeHeaderFieldMapping: removeHeaderFieldMapping,
        addHeaderFieldMapping: addHeaderFieldMapping,
        addUriFieldMapping: addUriFieldMapping,
        isWizardOk: isWizardOk
    };
};

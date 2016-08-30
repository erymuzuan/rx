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
        removeReceiveLocation = function (child) {
            var self = this;
            return function () {
                self.ReceiveLocationCollection.remove(child);
            };
        },
        addReceiveLocation = function () {
            var br = new bespoke.sph.domain.FolderReceiveLocation({ WebId: system.guid() });
            var self = this;

            require(["viewmodels/folder.receive.location.dialog", "durandal/app"], function (dialog, app) {
                dialog.location(br);
                app.showDialog(dialog)
                    .done(function (result) {
                        if (!result) return;
                        if (result === "OK") {
                            self.ReceiveLocationCollection.push(br);
                        }
                    });
            });
        },
        editReceiveLocation = function (br) {
            var self = this;
            return function () {
                require(["viewmodels/folder.receive.location.dialog", "durandal/app"], function (dialog, app) {
                    var clone = ko.mapping.fromJS(ko.mapping.toJS(br));
                    dialog.location(clone);
                    app.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {
                                self.ReceiveLocationCollection.replace(br, clone);
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
        editReceiveLocation: editReceiveLocation,
        removeReceiveLocation: removeReceiveLocation,
        addReceiveLocation: addReceiveLocation,
        isWizardOk: isWizardOk
    };
};

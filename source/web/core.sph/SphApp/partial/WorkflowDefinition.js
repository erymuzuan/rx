/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />


bespoke.sph.domain.WorkflowDefinitionPartial = function (model) {

    var system = require("durandal/system"),
        isBusy = ko.observable(false),
        context = require(objectbuilders.datacontext),
        elementNameOptions = ko.observableArray(),
        removeActivity = function (activity) {
            var self = this;
            return function () {
                self.ActivityCollection.remove(activity);
            };
        },
        setStartActivity = function (act) {
            var self = this;
            return function () {
                _(self.ActivityCollection()).each(function (v) {
                    v.IsInitiator(false);
                });
                act.IsInitiator(true);

            };
        },
        addActivity = function (type) {
            var self = this;
            return function () {
                var activity = new bespoke.sph.domain[type + "Activity"](system.guid());

                require(["viewmodels/activity." + type.toLowerCase(), "durandal/app"], function (dialog, app2) {
                    dialog.activity(activity);
                    if (typeof dialog.wd === "function") {
                        dialog.wd(self);
                    }
                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {
                                self.ActivityCollection.push(activity);
                            }
                        });

                });

            };
        },
        editActivity = function (activity) {
            var self = this;
            return function () {
                var clone = context.toObservable(ko.mapping.toJS(activity)),
                    type = ko.unwrap(activity.TypeName);

                isBusy(true);
                require(["viewmodels/activity." + type.toLowerCase(), "durandal/app"], function (dialog, app2) {
                    dialog.activity(clone);

                    if (typeof dialog.wd === "function") {
                        dialog.wd(self);
                    }

                    app2.showDialog(dialog)
                        .done(function (result) {
                            $("div.modalBlockout,div.modalHost").remove();
                            if (!result) return;
                            if (result === "OK") {
                                for (var g in activity) {
                                    if (activity.hasOwnProperty(g)) {
                                        var observable = activity[g].name === "observable" || activity[g].name === "d";
                                        if (typeof activity[g] === "function" && observable) {
                                            activity[g](ko.unwrap(clone[g]));
                                        } else {
                                            activity[g] = clone[g];
                                        }
                                    }
                                }
                            }
                        });

                });

            };
        },
        addVariable = function (type) {
            var self = this;
            return function () {
                var variable = new bespoke.sph.domain[type + "Variable"](system.guid());

                require(["viewmodels/variable." + type.toLowerCase(), "durandal/app"], function (dialog, app2) {
                    dialog.variable(variable);
                    if (typeof dialog.wd === "function") {
                        dialog.wd(self);
                    }
                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {
                                self.VariableDefinitionCollection.push(variable);
                            }
                        });

                });

            };
        },
        editVariable = function (variable) {
            var self = this;
            return function () {
                var variableType = ko.unwrap(variable.$type),
                    clone = ko.mapping.fromJS(ko.mapping.toJS(variable)),
                    pattern = /Bespoke\.Sph\.Domain\.(.*?)Variable,/,
                    type = pattern.exec(variableType)[1];

                require(["viewmodels/variable." + type.toLowerCase(), "durandal/app"], function (dialog, app2) {
                    dialog.variable(clone);
                    if (typeof dialog.wd === "function") {
                        dialog.wd(self);
                    }

                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {
                                for (var g in variable) {
                                    if (typeof variable[g] === "function" && variable[g].name === "observable") {
                                        variable[g](ko.unwrap(clone[g]));
                                    } else {
                                        variable[g] = clone[g];
                                    }
                                }
                            }
                        });

                });

            };
        },
        removeVariable = function (variable) {
            var self = this;
            return function () {
                self.VariableDefinitionCollection.remove(variable);
            };
        },
        addCorrelationType = function () {
            var self = this;
            var correlationType = new bespoke.sph.domain.CorrelationType(system.guid());

            require(["viewmodels/correlation.type.dialog", "durandal/app"], function (dialog, app2) {
                dialog.correlationType(correlationType);
                if (typeof dialog.wd === "function") {
                    dialog.wd(self);
                }
                app2.showDialog(dialog)
                    .done(function (result) {
                        if (!result) return;
                        if (result === "OK") {
                            self.CorrelationTypeCollection.push(correlationType);
                        }
                    });

            });


        },
        editCorrelationType = function (correlationType) {
            var self = this;
            return function () {
                var clone = ko.mapping.fromJS(ko.mapping.toJS(correlationType));

                require(["viewmodels/correlation.type.dialog", "durandal/app"], function (dialog, app2) {
                    dialog.correlationType(clone);
                    if (typeof dialog.wd === "function") {
                        dialog.wd(self);
                    }

                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {
                                for (var g in correlationType) {
                                    if (typeof correlationType[g] === "function" && correlationType[g].name === "observable") {
                                        correlationType[g](ko.unwrap(clone[g]));
                                    } else {
                                        correlationType[g] = clone[g];
                                    }
                                }
                            }
                        });

                });

            };
        },
        removeCorrelationType = function (correlationType) {
            var self = this;
            return function () {
                self.CorrelationTypeCollection.remove(correlationType);
            };
        },
        addCorrelationSet = function () {
            var self = this;
            var correlationSet = new bespoke.sph.domain.CorrelationSet(system.guid());

            require(["viewmodels/correlation.set.dialog", "durandal/app"], function (dialog, app2) {
                dialog.correlationSet(correlationSet);
                if (typeof dialog.wd === "function") {
                    dialog.wd(self);
                }
                app2.showDialog(dialog)
                    .done(function (result) {
                        if (!result) return;
                        if (result === "OK") {
                            self.CorrelationSetCollection.push(correlationSet);
                        }
                    });

            });


        },
        editCorrelationSet = function (correlationSet) {
            var self = this;
            return function () {
                var clone = ko.mapping.fromJS(ko.mapping.toJS(correlationSet));

                require(["viewmodels/correlation.set.dialog", "durandal/app"], function (dialog, app2) {
                    dialog.correlationSet(clone);
                    if (typeof dialog.wd === "function") {
                        dialog.wd(self);
                    }

                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {
                                for (var g in correlationSet) {
                                    if (typeof correlationSet[g] === "function" && correlationSet[g].name === "observable") {
                                        correlationSet[g](ko.unwrap(clone[g]));
                                    } else {
                                        correlationSet[g] = clone[g];
                                    }
                                }
                            }
                        });

                });

            };
        },
        removeCorrelationSet = function (correlationSet) {
            var self = this;
            return function () {
                self.CorrelationSetCollection.remove(correlationSet);
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
            alert("not implemented" + dll);
        },
        removeReferencedAssembly = function (dll) {
            var self = this;
            return function () {
                self.ReferencedAssemblyCollection.remove(dll);
            };
        },
        addTryScope = function () {
            var self = this;
            var tryScope = new bespoke.sph.domain.TryScope(system.guid());
            require(["viewmodels/try.scope.dialog", "durandal/app"], function (dialog, app2) {
                dialog.tryScope(tryScope);
                if (typeof dialog.wd === "function") {
                    dialog.wd(self);
                }
                app2.showDialog(dialog)
                    .done(function (result) {
                        if (!result) return;
                        if (result === "OK") {
                            self.TryScopeCollection.push(tryScope);
                        }
                    });

            });


        },
        editTryScope = function (tryScope) {
            var self = this;
            return function () {
                var clone = ko.mapping.fromJS(ko.mapping.toJS(tryScope));

                require(["viewmodels/try.scope.dialog", "durandal/app"], function (dialog, app2) {
                    dialog.tryScope(clone);
                    if (typeof dialog.wd === "function") {
                        dialog.wd(self);
                    }

                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {
                                for (var g in tryScope) {
                                    if (typeof tryScope[g] === "function" && tryScope[g].name === "observable") {
                                        tryScope[g](ko.unwrap(clone[g]));
                                    } else {
                                        tryScope[g] = clone[g];
                                    }
                                }
                            }
                        });

                });

            };
        },
        removeTryScope = function (tryScope) {
            var self = this;
            return function () {
                self.TryScopeCollection.remove(tryScope);
            };
        },
        loadSchema = function (storeId) {
            var id = storeId || this.SchemaStoreId();
            $.get("/api/workflow-definitions/xsd-elements/" + id)
                .then(function (result) {
                    elementNameOptions(result);
                });
        };

    model.SchemaStoreId.subscribe(loadSchema);

    var vm = {
        isBusy: isBusy,
        removeCorrelationType: removeCorrelationType,
        addCorrelationType: addCorrelationType,
        editCorrelationType: editCorrelationType,
        removeCorrelationSet: removeCorrelationSet,
        addCorrelationSet: addCorrelationSet,
        editCorrelationSet: editCorrelationSet,
        removeVariable: removeVariable,
        addVariable: addVariable,
        editVariable: editVariable,
        removeReferencedAssembly: removeReferencedAssembly,
        addReferencedAssembly: addReferencedAssembly,
        editReferencedAssembly: editReferencedAssembly,
        addActivity: addActivity,
        editActivity: editActivity,
        removeActivity: removeActivity,
        loadSchema: loadSchema,
        xsdElements: elementNameOptions,
        setStartActivity: setStartActivity,
        addTryScope: addTryScope,
        editTryScope: editTryScope,
        removeTryScope: removeTryScope
    };

    return vm;
};

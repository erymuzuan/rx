/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />

var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


bespoke.sph.domain.ActivityPartial = function () {

    var system = require("durandal/system"),
        hasError = ko.observable(),
        breakpoint = ko.observable(false),
        selected = ko.observable(false),
        hit = ko.observable(false),
        errors = ko.observableArray();
    return {
        breakpoint: breakpoint,
        hit: hit,
        selected: selected,
        hasError: hasError,
        errors: errors
    };
};
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/require.js" />

bespoke.sph.domain.AssemblyFieldPartial = function () {
    var system = require(objectbuilders.system),
        addParameter = function () {
            this.MethodArgCollection.push(new bespoke.sph.domain.MethodArg(system.guid()));
        },
        removeParameter = function(p) {
            var self = this;
            return function() {
                self.MethodArgCollection.remove(p);
            };
        };
    return {
        addParameter: addParameter,
        removeParameter: removeParameter
    };
};
/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />


bespoke.sph.domain.BarChartItemPartial = function () {
    var system = require('durandal/system'),
        addSeries = function() {
            this.ChartSeriesCollection.push(new bespoke.sph.domain.ChartSeries(system.guid()));
        },
        removeSeries = function (series) {
            var self = this;
            return function() {
                self.ChartSeriesCollection.remove(series);
            };

        };
    return {
        addSeries: addSeries,
        removeSeries: removeSeries
    };
};
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />

bespoke.sph.domain.BusinessRulePartial = function (model) {

    var system = require('durandal/system'),
        addFilter = function () {
            var self = this,
                br = new bespoke.sph.domain.Rule(system.guid());

            br.Left({ Name: ko.observable("+ Field") });
            br.Right({ Name: ko.observable("+ Field") });

            self.FilterCollection.push(br);

        },
        removeFilter = function (br) {
            var self = this;
            return function () {
                self.FilterCollection.remove(br);
            };
        },
        addRule = function () {
            var self = this,
                br = new bespoke.sph.domain.Rule(system.guid());

            br.Left({ Name: ko.observable("+ Field") });
            br.Right({ Name: ko.observable("+ Field") });

            self.RuleCollection.push(br);

        },
        removeRule = function (br) {
            var self = this;
            return function () {
                self.RuleCollection.remove(br);
            };
        };



    return {
        addFilter: addFilter,
        removeFilter: removeFilter,
        addRule: addRule,
        removeRule: removeRule
    };
};

/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../SphApp/schemas/form.designer.g.js" />

bespoke.sph.domain.ButtonPartial = function () {

    var editCommand = function () {
        var self = this,
            w = window.open("/sph/editor/ace?mode=javascript", "_blank", "height=" + screen.height + ",width=" + screen.width + ",toolbar=0,location=0,fullscreen=yes"),
            wdw = w.window || w,
            init = function () {
                wdw.code = ko.unwrap(self.Command);
                if (!w.code) {
                    w.code = "//insert your code here";
                }
                wdw.saved = function (code, close) {
                    self.Command(code);
                    if (close) {
                        w.close();
                    }
                };
            };
        if (wdw.attachEvent) { // for ie
            wdw.attachEvent("onload", init);
        } else {
            init();
        }
    },
        editOperationSuccessCallback = function () {
            var self = this,
                w = window.open("/sph/editor/ace?mode=javascript", "_blank", "height=" + screen.height + ",width=" + screen.width + ",toolbar=0,location=0,fullscreen=yes"),
                wdw = w.window || w,
                init = function () {
                    wdw.code = ko.unwrap(self.OperationSuccessCallback);
                    if (!w.code) {
                        w.code = "//insert your code here";
                    }
                    wdw.saved = function (code, close) {
                        self.OperationSuccessCallback(code);
                        if (close) {
                            w.close();
                        }
                    };
                };
            if (wdw.attachEvent) { // for ie
                wdw.attachEvent("onload", init);
            } else {
                init();
            }
        },
    editOperationFailureCallback = function () {
        var self = this,
            w = window.open("/sph/editor/ace?mode=javascript", "_blank", "height=" + screen.height + ",width=" + screen.width + ",toolbar=0,location=0,fullscreen=yes"),
            wdw = w.window || w,
            init = function () {
                wdw.code = ko.unwrap(self.OperationFailureCallback);
                if (!w.code) {
                    w.code = "//insert your code here";
                }
                wdw.saved = function (code, close) {
                    self.OperationFailureCallback(code);
                    if (close) {
                        w.close();
                    }
                };
            };
        if (wdw.attachEvent) { // for ie
            wdw.attachEvent("onload", init);
        } else {
            init();
        }
    },
        canSetSuccessCallback = ko.computed(function () {

        });
    return {
        editCommand: editCommand,
        editOperationSuccessCallback: editOperationSuccessCallback,
        editOperationFailureCallback: editOperationFailureCallback,
        canSetSuccessCallback: canSetSuccessCallback
    };
};
bespoke.sph.domain.CatchScopePartial = function () {


};
/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/durandal/system.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />


bespoke.sph.domain.ChildEntityListViewPartial = function (model) {
    // ViewColumn
    var system = require('durandal/system'),
        addViewColumn = function () {
            this.ViewColumnCollection.push(new bespoke.sph.domain.ViewColumn({ WebId: system.guid(),Header :'<New Column>'}));
        },
        removeViewColumn = function (obj) {
            var self = this;
            return function () {
                self.ViewColumnCollection.remove(obj);
            };

        },
        addSort = function () {
            this.SortCollection.push(new bespoke.sph.domain.Sort(system.guid()));
        },
        removeSort = function (obj) {
            var self = this;
            return function () {
                self.SortCollection.remove(obj);
            };

        },
    editViewColumn = function (vc) {
        return function () {
            var clone = ko.mapping.fromJS(ko.mapping.toJS(vc));

            require(['viewmodels/view.column.dialog', 'durandal/app'], function (dialog, app2) {
                dialog.column(clone);
                dialog.entity(model.Entity());

                app2.showDialog(dialog)
                    .done(function (result) {
                        if (!result) return;
                        if (result === "OK") {
                            for (var g in vc) {
                                if (typeof vc[g] === "function" && vc[g].name === "observable") {
                                    vc[g](ko.unwrap(clone[g]));
                                } else {
                                    vc[g] = clone[g];
                                }
                            }
                        }
                    });

            });

        };
    };
    return {
        addSort: addSort,
        removeSort: removeSort,
        addViewColumn: addViewColumn,
        editViewColumn: editViewColumn,
        removeViewColumn: removeViewColumn
    };
};
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/durandal/system.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />




bespoke.sph.domain.ChildWorkflowActivityPartial = function () {

    var system = require("durandal/system"),
        removeMapping = function (map) {
            var self = this;
            return function () {
                self.PropertyMappingCollection.remove(map);
            };
        },
        addMapping = function () {
            var map = new bespoke.sph.domain.PropertyMapping(system.guid());
            this.PropertyMappingCollection.push(map);
        },
        removeExecutedMapping = function (map) {
            var self = this;
            return function () {
                self.ExecutedPropertyMappingCollection.remove(map);
            };
        },
        addExecutedMapping = function () {
            var map = new bespoke.sph.domain.PropertyMapping(system.guid());
            this.ExecutedPropertyMappingCollection.push(map);
        };

    var vm = {
        addExecutedMapping: addExecutedMapping,
        removeExecutedMapping: removeExecutedMapping,
        addMapping: addMapping,
        removeMapping: removeMapping

    };

    return vm;
};

/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="/Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="/Scripts/knockout-3.4.0.debug.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="/Scripts/require.js" />

bespoke.sph.domain.ComboBoxPartial = function () {
    var system = require('durandal/system'),
        addItem = function () {
            var self = this;
            var item = new bespoke.sph.domain.ComboBoxItem(system.guid());
            self.ComboBoxItemCollection.push(item);

        },
        removeItem = function (item) {
            var self = this;
            return function () {
                self.ComboBoxItemCollection.remove(item);
            };
        };
    return {
        addItem: addItem,
        removeItem: removeItem
    };
};
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="/Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="/Scripts/knockout-3.4.0.debug.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="/Scripts/require.js" />

bespoke.sph.domain.ComplexMemberPartial = function(model) {
    model.Name.subscribe(function(name) {
        if (!ko.unwrap(model.TypeName)) {
            $.get("/entity-definition/singular/" + name).done(model.TypeName);
        }
    });

}
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />

bespoke.sph.domain.ConstantFieldPartial = function (model, option) {

    var vm = {
        Type: ko.observable(),
        Value: ko.observable(option.Value)
    };

    vm.Value.subscribe(function(v) {
        if (!ko.unwrap(model.Name)) {
            model.Name(v);
        }
    });
    return vm;
};
/// <reference path="../../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />


bespoke.sph.domain.CorrelationTypePartial = function () {

    var system = require('durandal/system'),
        removeCorrelationProperty = function (prop) {
            var self = this;
            return function () {
                self.CorrelationPropertyCollection.remove(prop);
            };
        },
        addCorrelationProperty = function () {
            this.CorrelationPropertyCollection.push(new bespoke.sph.domain.CorrelationProperty(system.guid()));
        };
    return {
        removeCorrelationProperty: removeCorrelationProperty,
        addCorrelationProperty: addCorrelationProperty

    };
};
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="/Scripts/require.js" />
/// <reference path="/Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="/Scripts/knockout-3.1.0.debug.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />



bespoke.sph.domain.CreateEntityActivityPartial = function () {
    var system = require('durandal/system'),
        addPropertyMapping = function (type) {
            var self = this;
            return function () {
                var mapping = new bespoke.sph.domain[type + 'Mapping'](system.guid());
                self.PropertyMappingCollection.push(mapping);
            };
        },
        removePropertyMapping = function (mapping) {
            var self = this;
            return function () {
                self.PropertyMappingCollection.remove(mapping);
            };
        };
    return {
        addPropertyMapping: addPropertyMapping,
        removePropertyMapping: removePropertyMapping
    };
};
/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />



bespoke.sph.domain.DataGridItemPartial = function () {

    // DataGridGroup
    var system = require('durandal/system'),
        addDataGridGroupDefinition = function () {
            this.DataGridGroupDefinitionCollection.push(new bespoke.sph.domain.DataGridGroupDefinition(system.guid()));
        },
        removeDataGridGroupDefinition = function (obj) {
            var self = this;
            return function () {
                self.DataGridGroupDefinitionCollection.remove(obj);
            };

        },
        addColumn = function () {
            var self = this;
            self.DataGridColumnCollection.push(new bespoke.sph.domain.DataGridColumn(system.guid()));
        },
        removeColumn = function (column) {
            var self = this;
            return function () {
                self.DataGridColumnCollection.remove(column);
            };
        };

    return {
        addColumn: addColumn,
        removeColumn: removeColumn,
        addDataGridGroupDefinition: addDataGridGroupDefinition,
        removeDataGridGroupDefinition: removeDataGridGroupDefinition
    };
};
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="/Scripts/require.js" />
/// <reference path="/Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="/Scripts/knockout-3.1.0.debug.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />




bespoke.sph.domain.DecisionActivityPartial = function () {
    var system = require('durandal/system'),
        addBranch = function() {
            this.DecisionBranchCollection.push(new bespoke.sph.domain.DecisionBranch(system.guid()));
        },
        removeBranch = function(branch) {
            var self = this;
            return function() {
                self.DecisionBranchCollection.remove(branch);
            };
        },
        multipleEndPoints = function() {
            return this.DecisionBranchCollection();
        };
    return {
        addBranch: addBranch,
        removeBranch: removeBranch,
        multipleEndPoints: multipleEndPoints
    };
};
/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />

bespoke.sph.domain.DelayActivityPartial = function () {

    return {
        isAsync:true
    };
};
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />



bespoke.sph.domain.DelimitedTextFormatterPartial = function (model) {

    const system = require("durandal/system"),
        selectedRow = ko.observable(new bespoke.sph.domain.FlatFileDetailTag()),
        parentOptions = ko.observableArray(["$record"]),
        childNameChanged = function (name) {
            if (name) {
                if (parentOptions().indexOf(name) < 0) {
                    parentOptions.push(name);
                }
            }
        },
        removeDetailsRow = function (child) {
            var self = this;
            return function () {
                self.DetailRowCollection.remove(child);
                if (child.nameChangedSubscription) {
                    child.nameChangedSubscription.dispose();
                }
            };
        },
        addDetailsRow = function () {
            const child = new bespoke.sph.domain.FlatFileDetailTag(system.guid());
            child.nameChangedSubscription = child.Name.subscribe(childNameChanged);
            this.DetailRowCollection.push(child);
            selectedRow(child);
        },
        selectRow = function(row) {
            selectedRow(row);
        },
        isWizardOk = ko.computed(function() {
            return ko.unwrap(model.SampleStoreId) && ko.unwrap(model.Delimiter);
        });

    const vm = {
        isWizardOk : isWizardOk,
        selectedRow: selectedRow,
        selectRow : selectRow,
        addDetailsRow: addDetailsRow,
        removeDetailsRow: removeDetailsRow,
        parentOptions: parentOptions

    };

    return vm;
};


bespoke.sph.domain.EntityChartPartial = function (model) {
    var context = require(objectbuilders.datacontext),
        pin = function () {
            if (typeof model.IsDashboardItem === "function") {
                model.IsDashboardItem(true);
            } else {
                model.IsDashboardItem = ko.observable(true);
            }

            var tcs = new $.Deferred();

            context.post(ko.mapping.toJSON(model), '/sph/entitychart/save')
                .done(tcs.resolve);

            return tcs.promise();
        },
        unpin = function() {
            if (typeof model.IsDashboardItem === "function") {
                model.IsDashboardItem(false);
            } else {
                model.IsDashboardItem = ko.observable(false);
            }

            var tcs = new $.Deferred();

            context.post(ko.mapping.toJSON(model), '/sph/entitychart/save')
                .done(tcs.resolve);

            return tcs.promise();
        };
    return {
        pin: pin,
        unpin : unpin
    };
};
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="/Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="/Scripts/knockout-3.4.0.debug.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="/Scripts/require.js" />

bespoke.sph.domain.ServiceContractPartial = function () {
    var context = require(objectbuilders.datacontext),
        publish = function (ed) {
            return context.post(ko.toJSON(ed), "/entity-definition/publish/service-contract");
        };

    return {
        publish: publish
    };
}
bespoke.sph.domain.EntityDefinitionPartial = function () {
    var system = require("durandal/system"),
        app = require("durandal/app"),
        context = require(objectbuilders.datacontext),
        addMember = function () {
            this.MemberCollection.push(new bespoke.sph.domain.Member({
                WebId: system.guid(),
                Boost: 1
            }));
        },
        removeMember = function (floor) {
            var self = this;
            return function () {
                self.MemberCollection.remove(floor);
            };
        },
        addEntityOperation = function () {
            this.EntityOperationCollection.push(new bespoke.sph.domain.EntityOperation({
                WebId: system.guid()
            }));
        }, save = function () {
            var self = this;
            context.post(ko.toJSON(self), "/entity-definition")
            .then(function (result) {
                if (result.success) {
                }
            });
        },
        removeEntityOperation = function (operation) {
            var self = this;
            return function () {
                var tcs = new $.Deferred();
                app.showMessage("Are you sure you want to remove this operation, this operation cannot be undone and will commit the entire changes to your EntityDefinition", "Reactive Developer", ["Yes", "No"])
                    .done(function (dialogResult) {
                        if (dialogResult === "Yes") {
                            self.EntityOperationCollection.remove(operation);
                            save.call(self);
                        }
                    });

                return tcs.promise();
            };
        },
        editMember = function (member) {
            var self = this;
            return function () {
                require(["viewmodels/member.dialog", "durandal/app"], function (dialog, app) {
                    var clone = ko.mapping.fromJS(ko.mapping.toJS(member));
                    dialog.member(clone);
                    app.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {
                                self.BlockCollection.replace(member, clone);
                            }
                        });
                });
            };
        },
        addBusinessRule = function () {
            var br = new bespoke.sph.domain.BusinessRule({ WebId: system.guid() });
            var self = this;

            require(["viewmodels/business.rule.dialog", "durandal/app"], function (dialog, app) {
                dialog.rule(br);
                app.showDialog(dialog)
                    .done(function (result) {
                        if (!result) return;
                        if (result === "OK") {
                            self.BusinessRuleCollection.push(br);
                        }
                    });
            });

        },
        editBusinessRule = function (rule) {
            var self = this;
            return function () {
                require(["viewmodels/business.rule.dialog", "durandal/app"], function (dialog, app) {
                    var clone = context.toObservable(ko.mapping.toJS(rule));
                    dialog.rule(clone);
                    app.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result == "OK") {
                                self.BusinessRuleCollection.replace(rule, clone);
                            }
                        });
                });
            };
        },
        removeBusinessRule = function (rule) {
            var self = this;
            return function () {
                self.BusinessRuleCollection.remove(rule);
            };
        },
        editMemberMap = function (member) {
            var building = this;
            return function () {
                console.log("show map ", building);
                console.log(" on member ", member);
                require(["viewmodels/member.map", "durandal/app"], function (dialog, app) {
                    dialog.init(building.BuildingId(), member.MemberPlanStoreId());
                    app.showDialog(dialog)
                        .done(function (result) {
                            if (result == "OK") {
                                member.MemberPlanStoreId(dialog.spatialStoreId());
                            }
                        });

                });

            };
        };
    return {
        addEntityOperation: addEntityOperation,
        removeEntityOperation: removeEntityOperation,
        addMember: addMember,
        editMember: editMember,
        removeMember: removeMember,
        addBusinessRule: addBusinessRule,
        editBusinessRule: editBusinessRule,
        removeBusinessRule: removeBusinessRule,
        editMemberMap: editMemberMap
    };
};
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../schemas/form.designer.g.js" />

bespoke.sph.domain.EntityFormPartial = function () {

    var editOperationSuccessCallback = function () {
        var self = this,
            w = window.open("/sph/editor/ace?mode=javascript", "_blank", "height=" + screen.height + ",width=" + screen.width + ",toolbar=0,location=0,fullscreen=yes"),
            wdw = w.window || w,
            init = function () {
                wdw.code = ko.unwrap(self.OperationSuccessCallback);
                if (!w.code) {
                    w.code = "//insert your code here";
                }
                wdw.saved = function (code, close) {
                    self.OperationSuccessCallback(code);
                    if (close) {
                        w.close();
                    }
                };
            };
        if (wdw.attachEvent) { // for ie
            wdw.attachEvent("onload", init);
        } else {
            init();
        }
    },
    editOperationFailureCallback = function () {
        var self = this,
            w = window.open("/sph/editor/ace?mode=javascript", "_blank", "height=" + screen.height + ",width=" + screen.width + ",toolbar=0,location=0,fullscreen=yes"),
            wdw = w.window || w,
            init = function () {
                wdw.code = ko.unwrap(self.OperationFailureCallback);
                if (!w.code) {
                    w.code = "//insert your code here";
                }
                wdw.saved = function (code, close) {
                    self.OperationFailureCallback(code);
                    if (close) {
                        w.close();
                    }
                };
            };
        if (wdw.attachEvent) { // for ie
            wdw.attachEvent("onload", init);
        } else {
            init();
        }
    },
        canSetSuccessCallback = ko.computed(function () {

        });
    return {
        editOperationSuccessCallback: editOperationSuccessCallback,
        editOperationFailureCallback: editOperationFailureCallback,
        canSetSuccessCallback: canSetSuccessCallback
    };
};
/// <reference path="../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />

bespoke.sph.domain.EntityLookupElementPartial = function () {

    var editDisplayTemplate = function () {
        var self = this,
            w = window.open("/sph/editor/ace?mode=javascript", "_blank", `height=${screen.height},width=${screen.width},toolbar=0,location=0,fullscreen=yes`),
            wdw = w.window || w,
            init = function () {
                wdw.code = ko.unwrap(self.Command);
                if (!w.code) {
                    w.code = "//insert your code here";
                }
                wdw.saved = function (code, close) {
                    self.DisplayTemplate(code);
                    if (close) {
                        w.close();
                    }
                };
            };
        if (wdw.attachEvent) { // for ie
            wdw.attachEvent("onload", init);
        } else {
            init();
        }
    },
    editColumns = function () {
            var self = this;
            require(["viewmodels/members.selector.dialog", "durandal/app"], function (dialog, app2) {
                dialog.entity(self.Entity());
                dialog.selectedMembers(self.LookupColumnCollection());
                app2.showDialog(dialog,self.Entity())
                    .done(function (result) {
                        if (!result) return;
                        if (result === "OK") {
                            self.LookupColumnCollection(dialog.selectedMembers());
                        }
                    });

            });
        },
        editFilter = function() {

            var self = this;
            require(["viewmodels/entity.lookup.filter.dialog", "durandal/app"], function (dialog, app2) {
                dialog.entity(self.Entity());
                dialog.selectedFilters(self.FilterCollection());
                app2.showDialog(dialog, self.Entity())
                    .done(function (result) {
                        if (!result) return;
                        if (result === "OK") {
                            self.FilterCollection(dialog.selectedFilters());
                        }
                    });

            });
        };

    return {
        editDisplayTemplate: editDisplayTemplate,
        editColumns: editColumns,
        editFilter: editFilter
    };
};
/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="/SphApp/services/system.js" />
/// <reference path="/SphApp/schemas/form.designer.g.js" />


bespoke.sph.domain.EntityViewPartial = function () {

    // Filter
    var system = require("durandal/system"),
        addConditionalFormatting = function () {
            this.ConditionalFormattingCollection.push(new bespoke.sph.domain.ConditionalFormatting(system.guid()));
        },
        removeConditionalFormatting = function (cf) {
            var self = this;
            return function () {
                self.ConditionalFormattingCollection.remove(cf);
            };

        },
        addRouteParameter = function () {
            this.RouteParameterCollection.push(new bespoke.sph.domain.RouteParameter(system.guid()));
        },
        removeRouteParameter = function (obj) {
            var self = this;
            return function () {
                self.RouteParameterCollection.remove(obj);
            };

        },
        addViewColumn = function () {
            this.ViewColumnCollection.push(new bespoke.sph.domain.ViewColumn(system.guid()));
        },
        removeViewColumn = function (obj) {
            var self = this;
            return function () {
                self.ViewColumnCollection.remove(obj);
            };

        },
        addFilter = function () {
            this.FilterCollection.push(new bespoke.sph.domain.Filter(system.guid()));
        },
        removeFilter = function (obj) {
            var self = this;
            return function () {
                self.FilterCollection.remove(obj);
            };

        },
        addSort = function () {
            this.SortCollection.push(new bespoke.sph.domain.Sort(system.guid()));
        },
        removeSort = function (obj) {
            var self = this;
            return function () {
                self.SortCollection.remove(obj);
            };

        };
    return {
        addRouteParameter: addRouteParameter,
        removeRouteParameter: removeRouteParameter,
        addConditionalFormatting: addConditionalFormatting,
        removeConditionalFormatting: removeConditionalFormatting,
        addViewColumn: addViewColumn,
        removeViewColumn: removeViewColumn,
        addSort: addSort,
        removeSort: removeSort,
        addFilter: addFilter,
        removeFilter: removeFilter
    };
};
/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />



bespoke.sph.domain.ExecutedActivityPartial = function () {

    var system = require('durandal/system'),
        breakpoint = ko.observable(false),
        hit = ko.observable(false),
        errors = ko.observableArray();
    return {
        breakpoint: breakpoint,
        hit: hit,
        errors: errors
    };
};
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />



bespoke.sph.domain.FilterPartial = function () {

    var system = require("durandal/system"),
          showFieldDialog = function (accessor, field, path) {
              require(["viewmodels/" + path, "durandal/app"], function (dialog, app2) {
                  dialog.field(field);
                  if (typeof dialog.entity === "function") {
                      //dialog.entity();
                      console.log("found the entity dialog :" + dialog.entity.name);
                  }
                  app2.showDialog(dialog)
                  .done(function (result) {
                      if (!result) return;
                      if (result === "OK") {
                          accessor(field);
                      }
                  });

              });
          },
          addField = function (accessor, type) {
              var field = new bespoke.sph.domain[type + "Field"](system.guid());
              showFieldDialog(accessor, field, "field." + type.toLowerCase());
          },
          editField = function (field) {
              var self = this;
              return function () {
                  var fieldType = ko.unwrap(field.$type),
                      clone = ko.mapping.fromJS(ko.mapping.toJS(field)),
                      pattern = /Bespoke\.Sph\.Domain\.(.*?)Field,/,
                      type = pattern.exec(fieldType)[1];


                  showFieldDialog(self.Field, clone, "field." + type.toLowerCase());
              };
          };

    var vm = {
        addField: addField,
        editField: editField

    };

    return vm;
};

/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />



bespoke.sph.domain.FixedLengthTextFormatterPartial = function (model) {

    const system = require("durandal/system"),
        parentOptions = ko.observableArray(["$record"]),
        childNameChanged = function (name) {
            if (name) {
                if (parentOptions().indexOf(name) < 0) {
                    parentOptions.push(name);
                }
            }
        },
        removeDetailsRow = function (child) {
            var self = this;
            return function () {
                self.DetailRowCollection.remove(child);
                if (child.nameChangedSubscription) {
                    child.nameChangedSubscription.dispose();
                }
            };
        },
        addDetailsRow = function () {
            const child = new bespoke.sph.domain.FlatFileDetailTag(system.guid());
            child.nameChangedSubscription = child.Name.subscribe(childNameChanged);
            this.DetailRowCollection.push(child);
        },
        isWizardOk = ko.computed(function () {
            return ko.unwrap(model.SampleStoreId) && ko.unwrap(model.Delimiter);
        }),
        addField = function () {
            const child = new bespoke.sph.domain.FixedLengthTextFieldMapping(system.guid());
            this.FieldMappingCollection.push(child);
        },
        removeField = function (f) {
            var self = this;
            return function () {
                self.FieldMappingCollection.remove(f);

            };

        },
        move = function (array, from, to) {
            if (to === from) return;

            const target = array[from],
                increment = to < from ? -1 : 1;

            for (let k = from; k !== to; k += increment) {
                array[k] = array[k + increment];
            }
            array[to] = target;
        },
        arrange = function (f, step) {
            const list = model.FieldMappingCollection,
                temps = ko.unwrap(list),
                index = temps.indexOf(f);

            move(temps, index, index + step);
            list(temps);
            list().forEach((x, i) => x.Order(i + 1));
        },
        moveDown = function (f) {
            arrange(f, 1);
        },
        moveUp = function (f) {
            arrange(f, -1);
        };

    const vm = {
        isWizardOk: isWizardOk,
        addDetailsRow: addDetailsRow,
        removeDetailsRow: removeDetailsRow,
        parentOptions: parentOptions,
        moveUp: moveUp,
        moveDown: moveDown,
        addField: addField,
        removeField: removeField

    };

    return vm;
};

/// <reference path="../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../schemas/form.designer.g.js" />

bespoke.sph.domain.FormDialogPartial = function (model) {

    var system = require("durandal/system"),
        init = function() {

            var buttons = ko.unwrap(model.DialogButtonCollection);
            _(buttons).each(function (v) {
                v.canMoveDown(true);
                v.canMoveUp(true);
            });
            var first = _(buttons).first(),
                last = _(buttons).last();
            if (first) {
                first.canMoveUp(false);
            }
            if (last) {
                last.canMoveDown(false);
            }
        },
        addDialogButton = function () {
            model.DialogButtonCollection.push(new bespoke.sph.domain.DialogButton({ WebId: system.guid(), canMoveDown: false }));
            init();
        },
        removeDialogButton = function (btn) {
            model.DialogButtonCollection.remove(btn);
            init();
        },
        move = function (array, from, to) {
            if (to === from) return;

            var target = array[from];
            var increment = to < from ? -1 : 1;

            for (var k = from; k !== to; k += increment) {
                array[k] = array[k + increment];
            }
            array[to] = target;
        },
        arrange = function (btn, step) {

            var index = model.DialogButtonCollection().indexOf(btn);
            var temps = ko.unwrap(model.DialogButtonCollection);
            move(temps, index, index + step);

            _(temps).each(function (v) {
                v.canMoveDown(true);
                v.canMoveUp(true);
            });
            _(temps).first().canMoveUp(false);
            _(temps).last().canMoveDown(false);
            model.DialogButtonCollection(temps);
        },
        moveDown = function (btn) {
            arrange(btn, 1);
        },
        moveUp = function (btn) {
            arrange(btn, -1);
        };

    init();

    return {
        removeDialogButton: removeDialogButton,
        addDialogButton: addDialogButton,
        moveDown: moveDown,
        moveUp: moveUp
    };
};
bespoke.sph.domain.DialogButtonPartial = function (model) {

    var canMoveUp = ko.observable(true),
        canMoveDown = ko.observable(true);

    return {
        canMoveDown: canMoveDown,
        canMoveUp: canMoveUp
    };
};
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />



bespoke.sph.domain.FormElementPartial = function () {
    
    return {
    };
};


/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />



bespoke.sph.domain.HtmlElementPartial = function () {

    var editHtml = function () {
        var self = this,
            w = window.open("/sph/editor/ace?mode=html", '_blank', 'height=' + screen.height + ',width=' + screen.width + ',toolbar=0,location=0,fullscreen=yes'),
            init = function () {
                w.code = ko.unwrap(self.Text);
                w.saved = function (code, close) {
                    self.Text(code);
                    if (close) {
                        w.close();
                    }
                };
            };
        if (w.attachEvent) { // for ie
            w.attachEvent('onload', init);
        } else {
            init();
        }
    };
    return {
        editHtml: editHtml
    };
};

bespoke.sph.domain.FormTemplatePartial = function () {

    var vm = {
    };
    return vm;
}
/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />

bespoke.sph.domain.IntervalSchedulePartial = function () {
    var icon = ko.observable(),
        name = ko.observable();
    return {
        icon: icon,
        name : name
    };
};
/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />

bespoke.sph.domain.LineChartItemPartial = function () {
    var system = require('durandal/system'),
        addSeries = function() {
            this.ChartSeriesCollection.push(new bespoke.sph.domain.ChartSeries(system.guid()));
        },
        removeSeries = function (series) {
            var self = this;
            return function() {
                self.ChartSeriesCollection.remove(series);
            };

        };
    return {
        addSeries: addSeries,
        removeSeries: removeSeries
    };
};
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="/Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="/Scripts/knockout-3.1.0.debug.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />


bespoke.sph.domain.ListenActivityPartial = function () {
    var system = require('durandal/system'),
        addBranch = function () {
            var self = this;
            var branch = new bespoke.sph.domain.ListenBranch(system.guid());
            self.ListenBranchCollection.push(branch);

        },
        removeBranch = function (branch) {
            var self = this;
            return function () {
                self.ListenBranchCollection.remove(branch);
            };
        },
        multipleEndPoints = function () {
            return this.ListenBranchCollection();
        };
    return {
        addBranch: addBranch,
        removeBranch: removeBranch,
        multipleEndPoints: multipleEndPoints
    };
};
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />



bespoke.sph.domain.ListViewPartial = function (model) {

    // ListViewColumn
    var system = require('durandal/system'),
        addListViewColumn = function (type) {
            var self = this;
            return function () {
                var column = new bespoke.sph.domain.ListViewColumn(system.guid()),
                    input = bespoke.sph.domain[type](system.guid());


                column.Path.subscribe(function(v) {
                    input.Path(v);
                });
                column.Label.subscribe(function(v) {
                    input.Label(v);
                });

                column.Input(input);
                input.isSelected = ko.observable(false);
                self.ListViewColumnCollection.push(column);
            };
        },
        removeListViewColumn = function (obj) {
            var self = this;
            return function () {
                self.ListViewColumnCollection.remove(obj);
            };

        };

    _(model.ListViewColumnCollection()).each(function(v) {
        v.Input().isSelected = ko.observable(false);
    });

    return {
        addListViewColumn: addListViewColumn,
        removeListViewColumn: removeListViewColumn
    };
};
/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />


bespoke.sph.domain.ListViewColumnPartial = function (model) {

    try {

        var pattern1 = /Bespoke\.Sph\.Domain\.(.*?),/,
            input1 = ko.unwrap(model.Input),
            name1 = null,
            icon1 = null;
        if (input1) {
            name1 = pattern1.exec(ko.unwrap(input1.$type))[1],
            icon1 = '/images/form.element.' + name1 + '.png';
        }


    } catch (err) {

    }
    icon1 = icon1 || '/images/form.element.textbox.png';
    var icon = ko.observable(icon1);
    model.Input.subscribe(function (c) {
        if (!c.$type) {
            return;
        }

        var pattern = /Bespoke\.Sph\.Domain\.(.*?),/,
            name = pattern.exec(ko.unwrap(c.$type))[1];
        icon('/images/form.element.' + name + '.png');


    });
    return {
        icon: icon
    };
};
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="/Scripts/require.js" />
/// <reference path="/Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="/Scripts/knockout-3.4.0.debug.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />



bespoke.sph.domain.MappingActivityPartial = function () {
    var system = require("durandal/system"),
        addMappingSource = function () {
            var self = this;
            var mapping = new bespoke.sph.domain["MappingSource"](system.guid());
            self.MappingSourceCollection.push(mapping);

        },
        removeMappingSource = function (mapping) {
            var self = this;
            return function () {
                self.MappingSourceCollection.remove(mapping);
            };
        };
    return {
        addMappingSource: addMappingSource,
        removeMappingSource: removeMappingSource
    };
};
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../schemassystem.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="/Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="/Scripts/knockout-3.4.0.debug.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="/Scripts/require.js" />


bespoke.sph.domain.MemberPartial = function (model) {
    const system = require("durandal/system"),
        addMember = function () {
            this.MemberCollection.push(new bespoke.sph.domain.Member(system.guid()));
        },
        editMember = function (member) {
            var self = this;
            return function () {
                require(["viewmodels/member.dialog", "durandal/app"], function (dialog, app) {
                    var clone = ko.mapping.fromJS(ko.mapping.toJS(member));
                    dialog.member(clone);
                    app.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {
                                self.BlockCollection.replace(member, clone);
                            }
                        });
                });
            };
        },
        editPermission = function (member) {
            var self = this;
            require(["viewmodels/field.permission.dialog", "durandal/app"], function (dialog, app) {
                var clone = ko.mapping.fromJS(ko.mapping.toJS(member));
                dialog.member(clone);
                app.showDialog(dialog)
                    .done(function (result) {
                        if (!result) return;
                        if (result === "OK") {
                            self.FieldPermissionCollection(clone.FieldPermissionCollection());
                        }
                    });
            });

        },
        removeMember = function (floor) {
            var self = this;
            return function () {
                self.MemberCollection.remove(floor);
            };
        },
        editMemberMap = function (member) {
            var building = this;
            return function () {
                console.log("show map ", building);
                console.log(" on member ", member);
                require(["viewmodels/member.map", "durandal/app"], function (dialog, app) {
                    dialog.init(building.BuildingId(), member.MemberPlanStoreId());
                    app.showDialog(dialog)
                        .done(function (result) {
                            if (result === "OK") {
                                member.MemberPlanStoreId(dialog.spatialStoreId());
                            }
                        });

                });

            };
        },
        showFieldDialog = function (accessor, field, path) {
            require([`viewmodels/${path}`, "durandal/app"], function (dialog, app2) {
                dialog.field(field);


                app2.showDialog(dialog)
                    .done(function (result) {
                        if (!result) return;
                        if (result === "OK") {
                            accessor(field);
                        }
                    });

            });
        },
        addField = function (accessor, type) {
            var field = new bespoke.sph.domain[type + "Field"](system.guid());
            showFieldDialog(accessor, field, "field." + type.toLowerCase());
        },
        editField = function (field) {
            var self = this;
            return function () {
                var fieldType = ko.unwrap(field.$type),
                    clone = ko.mapping.fromJS(ko.mapping.toJS(field)),
                    pattern = /Bespoke\.Sph\.Domain\.(.*?)Field,/,
                    type = pattern.exec(fieldType)[1];


                showFieldDialog(self.Field || self.DefaultValue, clone, "field." + type.toLowerCase());
            };
        },
        removeField = function () {
            const self = this,
                accessor = self.Field || self.DefaultValue;
            return function () {
                accessor({ Name: ko.observable("[Select a value]") });
            };
        };


    model.AllowMultiple.subscribe(function(array) {
        if (array) {
            model.DefaultValue(null);
        } else {
            model.DefaultValue({ Name: ko.observable("[Select a value]") });
        }
    });

    return {
        editPermission: editPermission,
        addMember: addMember,
        editMember: editMember,
        editMemberMap: editMemberMap,
        removeMember: removeMember,
        addField: addField,
        removeField: removeField,
        editField: editField,
        attachedProperties: ko.observableArray(),
        isBusyLoadingAttachedProperties : ko.observable(true)
    };
};
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />



bespoke.sph.domain.MethodArgPartial = function () {

    var system = require('durandal/system'),
        showFieldDialog = function (accessor, field, path) {
            require(['viewmodels/' + path, 'durandal/app'], function (dialog, app2) {
                dialog.field(field);


                app2.showDialog(dialog)
                .done(function (result) {
                    if (!result) return;
                    if (result === "OK") {
                        accessor(field);
                    }
                });

            });
        },
        addField = function (accessor, type) {
            var field = new bespoke.sph.domain[type + 'Field'](system.guid());
            showFieldDialog(accessor, field, 'field.' + type.toLowerCase());
        },
        editField = function (field) {
            var self = this;
            return function () {
                var fieldType = ko.unwrap(field.$type),
                    clone = ko.mapping.fromJS(ko.mapping.toJS(field)),
                    pattern = /Bespoke\.Sph\.Domain\.(.*?)Field,/,
                    type = pattern.exec(fieldType)[1];


                showFieldDialog(self.Field, clone, 'field.' + type.toLowerCase());
            };
        };

    var vm = {
        addField: addField,
        editField: editField

    };

    return vm;
};

/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />


bespoke.sph.domain.NotificationActivityPartial = function(model) {
    
    if (!model.From()) {
        model.From("admin@@sph.my");
    }
    return {
    };
};
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/durandal/system.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />



bespoke.sph.domain.OperationEndpointPartial = function () {

    const system = require("durandal/system"),
        removeChildAction = function (child) {
            var self = this;
            return function() {
                self.SetterActionChildCollection.remove(child);
            };
        },
        addChildAction = function() {
            var child = new bespoke.sph.domain.SetterActionChild(system.guid());
            child.Field({ Name: ko.observable("+ Field") });
            this.SetterActionChildCollection.push(child);
        },
        removePatchPath = function (child) {
            var self = this;
            return function() {
                self.PatchPathCollection.remove(child);
            };
        },
        addPatchPath = function () {
            var child = new bespoke.sph.domain.PatchSetter({ WebId: system.guid() , IsRequired: true});
            this.PatchPathCollection.push(child);
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
        };

    var vm = {
        editReferencedAssembly: editReferencedAssembly,
        removeReferencedAssembly: removeReferencedAssembly,
        addReferencedAssembly: addReferencedAssembly,
        removePatchPath: removePatchPath,
        addPatchPath: addPatchPath,
        addChildAction: addChildAction,
        removeChildAction: removeChildAction

    };

    return vm;
};

/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="/Scripts/require.js" />
/// <reference path="/Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="/Scripts/knockout-3.1.0.debug.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />



bespoke.sph.domain.ParallelActivityPartial = function () {
    var system = require('durandal/system'),
        addBranch = function() {
            this.ParallelBranchCollection.push(new bespoke.sph.domain.ParallelBranch(system.guid()));
        },
        removeBranch = function(branch) {
            var self = this;
            return function() {
                self.ParallelBranchCollection.remove(branch);
            };
        },
        multipleEndPoints = function() {
            return this.ParallelBranchCollection();
        };
    return {
        addBranch: addBranch,
        removeBranch: removeBranch,
        multipleEndPoints: multipleEndPoints
    };
};
/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="/SphApp/services/system.js" />
/// <reference path="/SphApp/schemas/form.designer.g.js" />


bespoke.sph.domain.QueryEndpointPartial = function (model) {

    // Filter
    var system = require("durandal/system"),
        addRouteParameter = function () {
            this.RouteParameterCollection.push(new bespoke.sph.domain.RouteParameter(system.guid()));
        },
        removeRouteParameter = function (obj) {
            var self = this;
            return function () {
                self.RouteParameterCollection.remove(obj);
            };

        },
        addFilter = function () {
            this.FilterCollection.push(new bespoke.sph.domain.Filter(system.guid()));
        },
        removeFilter = function (obj) {
            var self = this;
            return function () {
                self.FilterCollection.remove(obj);
            };

        },
        addSort = function () {
            this.SortCollection.push(new bespoke.sph.domain.Sort(system.guid()));
        },
        removeSort = function (obj) {
            var self = this;
            return function () {
                self.SortCollection.remove(obj);
            };

        },
        allowQueryCache = ko.observable(ko.unwrap(model.CacheFilter));
    return {
        addRouteParameter: addRouteParameter,
        removeRouteParameter: removeRouteParameter,
        addSort: addSort,
        removeSort: removeSort,
        addFilter: addFilter,
        removeFilter: removeFilter,
        allowQueryCache: allowQueryCache
    };
};
bespoke.sph.domain.ReceiveActivityPartial = function () {
    //
    var system = require('durandal/system'),
        removeCorrelationProperty = function (prop) {
            var self = this;
            return function () {
                self.CorrelationPropertyCollection.remove(prop);
            };
        },
        addCorrelationProperty = function () {
            this.CorrelationPropertyCollection.push(new bespoke.sph.domain.CorrelationProperty(system.guid()));
        };

    return {
        removeCorrelationProperty: removeCorrelationProperty,
        addCorrelationProperty: addCorrelationProperty

    };
};
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

/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />




bespoke.sph.domain.DailySchedulePartial = function () {

    return {
        name: ko.observable("Daily Schedule"),
        icon: ko.observable("fa fa-calendar")
    };
};
bespoke.sph.domain.WeeklySchedulePartial = function () {

    return {
        name: ko.observable("Weekly Schedule"),
        icon: ko.observable("fa fa-th-list")
    };
};
bespoke.sph.domain.MonthlySchedulePartial = function () {

    return {
        name: ko.observable("Monthly Schedule"),
        icon: ko.observable("fa fa-calendar-o"),
        dateOptions : _.range(1,31)
    };
};


/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../prototypes/IntervalScheduleContainer.js" />
/// <reference path="../../App/durandal/system.js" />

bespoke.sph.domain.ReportDeliveryPartial = function () {

    return new bespoke.sph.domain.IntervalScheduleContainer();

};
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />


bespoke.sph.domain.RouteParameterPartial = function (model) {

    model.Type.subscribe(function(v) {
        if (v === "System.DateTime, mscorlib") {
            model.DefaultValue(null);
        }
    });
    return {};
};
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />

bespoke.sph.domain.RouteParameterFieldPartial = function (model) {
    // TODO : In QueryEndpoint filter, if term were defined, then maybe we could suggest the name and type
    model.IsOptional.subscribe(function(opt) {
        if (!opt) {
            model.DefaultValue("");
        }
    });
    return {};
};

/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />



bespoke.sph.domain.RulePartial = function (model) {

    const system = require("durandal/system"),
        showFieldDialog = function (accessor, field, path) {
            require([`viewmodels/${path}`, "durandal/app"], function (dialog, app2) {
                dialog.field(field);


                app2.showDialog(dialog)
                .done(function (result) {
                    if (!result) return;
                    if (result === "OK") {
                        accessor(field);
                    }
                });

            });
        },
        addField = function (accessor, type) {
            const field = new bespoke.sph.domain[type + "Field"](system.guid());
            showFieldDialog(accessor, field, "field." + type.toLowerCase());
        },
        editField = function (field, accessor) {
            return function () {
                const fieldType = ko.unwrap(field.$type),
                    clone = ko.mapping.fromJS(ko.mapping.toJS(field)),
                    pattern = /Bespoke\.Sph\.Domain\.(.*?)Field,/,
                    type = pattern.exec(fieldType)[1];


                showFieldDialog(accessor, clone, "field." + type.toLowerCase());

            };
        },
        binaryOperator = ko.computed(function() {
            if (ko.unwrap(model.Operator) === "IsNull") {
                return false;
            }
            if (ko.unwrap(model.Operator) === "IsNotNull") {
                return false;
            }

            return true;
        });

    const vm = {
        binaryOperator: binaryOperator,
        addField: addField,
        editField: editField

    };

    return vm;
};

/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />
/// <reference path="../prototypes/IntervalScheduleContainer.js" />



bespoke.sph.domain.ScheduledTriggerActivityPartial = function (model) {

    var b = new bespoke.sph.domain.IntervalScheduleContainer();
    
    return b;
};

/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />



bespoke.sph.domain.SetterActionPartial = function () {

    var system = require("durandal/system"),
        removeChildAction = function (child) {
            var self = this;
            return function() {
                self.SetterActionChildCollection.remove(child);
            };
        },
        addChildAction = function() {
            var child = new bespoke.sph.domain.SetterActionChild(system.guid());
            child.Field({ Name: ko.observable("+ Field") });
            this.SetterActionChildCollection.push(child);
        };

    var vm = {
        addChildAction: addChildAction,
        removeChildAction: removeChildAction

    };

    return vm;
};

/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="~/Scripts/durandal/system.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />



bespoke.sph.domain.SetterActionChildPartial = function () {

    var system = require('durandal/system'),
        showFieldDialog = function (accessor, field, path, entity) {
            require(['viewmodels/' + path, 'durandal/app'], function (dialog, app2) {
                dialog.field(field);
                if (typeof dialog.entity === "function") {
                    dialog.entity(entity);
                    console.log("found the entity dialog :" + dialog.entity.name);
                }

                app2.showDialog(dialog)
                .done(function (result) {
                    if (!result) return;
                    if (result === "OK") {
                        accessor(field);
                    }
                });

            });
        },
        addField = function (accessor, type, entity) {
            var field = new bespoke.sph.domain[type + 'Field'](system.guid());
            showFieldDialog(accessor, field, 'field.' + type.toLowerCase(), entity);
        },
        editField = function (field) {
            var self = this;
            return function () {
                var fieldType = ko.unwrap(field.$type),
                    clone = ko.mapping.fromJS(ko.mapping.toJS(field)),
                    pattern = /Bespoke\.Sph\.Domain\.(.*?)Field,/,
                    type = pattern.exec(fieldType)[1];


                showFieldDialog(self.Field, clone, 'field.' + type.toLowerCase());
            };
        };

    var vm = {
        addField: addField,
        editField: editField

    };

    return vm;
};

///<reference path="../schemas/form.designer.g.js"/>
///<reference path="~/Scripts/require.js"/>
///<reference path="~/Scripts/knockout-3.4.0.debug.js"/>
///<reference path="~/Scripts/jquery-2.2.0.intellisense.js"/>


bespoke.sph.domain.Adapters = bespoke.sph.domain.Adapters || {};
bespoke.sph.domain.Adapters.Polling = function(model) {
    model = model || {};
    return {
        Interval: ko.observable( model.Interval || 5 * 60 * 1000), // 5 minutes
        IntervalPeriod: ko.observable("minute"), // 5 minutes
        Query: ko.observable(model.Query || ""),
        StartDate : ko.observable(model.Query)
    }
}
bespoke.sph.domain.Adapters.SqlServerReceiveLocation = function (model) {
    const v = new bespoke.sph.domain.ReceiveLocation(model);
    v.Id = ko.observable(null);
    v.Server = ko.observable("(localdb)\\Projects");
    v.Database = ko.observable(null);
    v.Trusted = ko.observable(true);
    v.UseQueryNotification = ko.observable(true);
    v.UserId = ko.observable(null);
    v.Password = ko.observable(null);
    v.Query = ko.observable("");
    v.Polling = ko.observable(new bespoke.sph.domain.Adapters.Polling()),
    v["$type"] = "Bespoke.Sph.Integrations.Adapters.SqlServerReceiveLocation, sqlserver.adapter";

    var context = require("services/datacontext");
    if (model && typeof model === "object") {
        for (let n in model) {
            if (model.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && "push" in v[n]) {
                    const values = model[n].$values || model[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) { return context.toObservable(ai); }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](model[n]);
                }
            }
        }
    }
    if (model && typeof model === "string") {
        v.WebId(model);
    }

    if (bespoke.sph.domain.SqlServerReceiveLocationPartial) {
        return _(v).extend(new bespoke.sph.domain.FolderReceiveLocationPartial(v, model));
    }
    return v;
};
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/durandal/system.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />




bespoke.sph.domain.StartWorkflowActionPartial = function () {

    var system = require("durandal/system"),
        removeMapping = function (map) {
            var self = this;
            return function () {
                self.WorkflowTriggerMapCollection.remove(map);
            };
        },
        addMapping = function () {
            var child = new bespoke.sph.domain.WorkflowTriggerMap(system.guid());
            child.Field({ Name: ko.observable("+ Field") });
            this.WorkflowTriggerMapCollection.push(child);
        },
        editMapping = function (child) {
            this.WorkflowTriggerMapCollection.push(child);
        };

    var vm = {
        editMapping: editMapping,
        addMapping: addMapping,
        removeMapping: removeMapping

    };

    return vm;
};

/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="/Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="/Scripts/knockout-3.4.0.debug.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="/Scripts/require.js" />

bespoke.sph.domain.TabControlPartial = function (tab) {
    var system = require("durandal/system"),
        addItem = function () {
            var item = new bespoke.sph.domain.TabPanel(system.guid());
            tab.TabPanelCollection.push(item);

        },
        removeItem = function (item) {
            return function () {
                tab.TabPanelCollection.remove(item);
            };
        };
    if (tab.TabPanelCollection().length === 0) {
        tab.TabPanelCollection.push(new bespoke.sph.domain.TabPanel({ WebId: system.guid() , Header : "Tab 1"}));
        tab.TabPanelCollection.push(new bespoke.sph.domain.TabPanel({ WebId: system.guid() , Header : "Tab 2"}));
    }
    return {
        addItem: addItem,
        removeItem: removeItem
    };
};
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="/Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="/Scripts/knockout-3.4.0.debug.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="/Scripts/require.js" />

bespoke.sph.domain.TimePeriodPartial = function (period) {
    var toString = function() {
        return ko.unwrap(period.Count) + " " + ko.unwrap(period.Unit);
    };
    return {
        toString: toString
    };
};
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />



bespoke.sph.domain.TransformDefinitionPartial = function () {

    var addReferencedAssembly = function () {
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
        };

    var vm = {
        editReferencedAssembly: editReferencedAssembly,
        removeReferencedAssembly: removeReferencedAssembly,
        addReferencedAssembly: addReferencedAssembly
    };

    return vm;
};

/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />



bespoke.sph.domain.TriggerPartial = function () {

    var system = require('durandal/system'),
        removeAction = function (action) {
            var self = this;
            return function () {
                self.ActionCollection.remove(action);
            };
        },
        addAction = function (type) {
            var self = this;
            return function () {
                var t = type.toLowerCase().replace(", ", ",");
                require(["viewmodels/action." + t, "durandal/app"], function (dialog, app2) {
                    if (typeof dialog.action !== "function") {
                        console.error("The dialog for " + t + " do not implement action as observable");
                        return;
                    }

                    // initialize the action properties
                    var clone = ko.mapping.fromJS(ko.toJS(dialog.action));
                    if(typeof clone.Title === "function"){
                        clone.Title("");
                    }
                    for (var n in clone) {
                        if (clone.hasOwnProperty(n)) {
                            var obj = clone[n];
                            if (n === "$type") {
                                continue;
                            }
                            if (ko.isObservable(obj) && typeof obj.destroyAll !== "function") {
                                obj(null);
                            }
                            if (ko.isObservable(obj) && typeof obj.destroyAll === "function") {
                                obj([]);
                            }
                        }
                    }
                    dialog.action(clone);

                    if (typeof dialog.trigger === "function") {
                        dialog.trigger(self);
                    }

                    app2.showDialog(dialog)
                    .done(function (result) {
                        if (!result) return;
                        if (result === "OK") {

                            var action = dialog.action();

                            action.WebId(system.guid());
                            action.IsActive(true);
                            self.ActionCollection.push(action);
                        }
                    });

                });

            };
        },
        editAction = function (action) {
            var self = this;
            return function () {
                var type = ko.unwrap(action.$type),
                    clone = ko.mapping.fromJS(ko.mapping.toJS(action));

                require(["viewmodels/action." + type.toLowerCase().replace(", ", ","), "durandal/app"], function (dialog, app2) {
                    dialog.action(clone);
                    if (typeof dialog.trigger === "function") {
                        dialog.trigger(self);
                    }

                    app2.showDialog(dialog)
                    .done(function (result) {
                        if (!result) return;
                        if (result === "OK") {
                            self.ActionCollection.replace(action, clone);
                        }
                    });

                });

            };
        },
         addRule = function () {
             var rule = new bespoke.sph.domain.Rule(system.guid());
             rule.Left({ Name: ko.observable("+ Field") });
             rule.Right({ Name: ko.observable("+ Field") });
             this.RuleCollection.push(rule);
         },
        removeRule = function (rule) {
            var self = this;
            return function () {
                self.RuleCollection.remove(rule);
            };
        },
        
        addReferencedAssembly = function () {
            var self = this;
            require(['viewmodels/assembly.dialog', 'durandal/app'], function (dialog, app2) {
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
            alert('not implemented' + dll);
        },
        removeReferencedAssembly = function (dll) {
            var self = this;
            return function () {
                self.ReferencedAssemblyCollection.remove(dll);
            };
        };

    var vm = {
        editReferencedAssembly: editReferencedAssembly,
        removeReferencedAssembly: removeReferencedAssembly,
        addReferencedAssembly: addReferencedAssembly,
        addRule: addRule,
        removeRule: removeRule,
        removeAction: removeAction,
        addAction: addAction,
        editAction: editAction

    };

    return vm;
};

bespoke.sph.domain.TryScopePartial = function () {


    var
        addCatchScope = function(wd) {
            return function() {
                var system = require('durandal/system');


                var self2 = this;
                var catchScope = new bespoke.sph.domain.CatchScope(system.guid());

                require(['viewmodels/catch.scope.dialog', 'durandal/app'], function(dialog, app2) {
                    dialog.catchScope(catchScope);

                    if (typeof dialog.wd === "function") {
                        dialog.wd(wd());
                    }

                    app2.showDialog(dialog)
                        .done(function(result) {
                            if (!result) return;
                            if (result === "OK") {
                                self2.CatchScopeCollection.push(catchScope);
                            }

                        });

                });
            };
        },
        editCatchScope = function (catchScope, wdOutside) {
            var self = this, wd = wdOutside;
            var clone = ko.mapping.fromJS(ko.mapping.toJS(catchScope));

            return function () {
                require(['viewmodels/catch.scope.dialog', 'durandal/app'], function (dialog, app2) {
                    dialog.catchScope(clone);

                    if (typeof dialog.wd === "function") {
                        dialog.wd(wd());
                    }

                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {
                                for (var g in catchScope) {
                                    if (typeof catchScope[g] === "function" && catchScope[g].name === "observable") {
                                        catchScope[g](ko.unwrap(clone[g]));
                                    } else {
                                        catchScope[g] = clone[g];
                                    }
                                }
                            }
                        });

                });
            };

        },
        removeCatchScope = function (catchScope, wdOutside) {
            var self = this, wd = wdOutside;
            
            return function () {
                wd().ActivityCollection().forEach(function (act) {
                    if (act.CatchScope() === catchScope.Id()) {
                        act.CatchScope("");
                    }
                });

                self.CatchScopeCollection.remove(catchScope);
            };
            
        };

    return {
        addCatchScope: addCatchScope,
        editCatchScope: editCatchScope,
        removeCatchScope: removeCatchScope
    };
};
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="/Scripts/require.js" />
/// <reference path="/Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="/Scripts/knockout-3.1.0.debug.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />



bespoke.sph.domain.UpdateEntityActivityPartial = function () {
    var system = require('durandal/system'),
        addPropertyMapping = function (type) {
            var self = this;
            return function () {
                var mapping = new bespoke.sph.domain[type + 'Mapping'](system.guid());
                self.PropertyMappingCollection.push(mapping);
            };
        },
        removePropertyMapping = function (mapping) {
            var self = this;
            return function () {
                self.PropertyMappingCollection.remove(mapping);
            };
        };
    return {
        addPropertyMapping: addPropertyMapping,
        removePropertyMapping: removePropertyMapping
    };
};
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="/Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="/Scripts/knockout-3.4.0.debug.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="/Scripts/require.js" />

bespoke.sph.domain.ValueObjectDefinitionPartial = function () {
    var system = require("durandal/system"),
        app = require("durandal/app"),
        context = require(objectbuilders.datacontext),
        addMember = function () {
            this.MemberCollection.push(new bespoke.sph.domain.Member({
                WebId: system.guid(),
                Boost: 1
            }));
        },
        removeMember = function (floor) {
            var self = this;
            return function () {
                self.MemberCollection.remove(floor);
            };
        },
        editMember = function (member) {
            var self = this;
            return function () {
                require(["viewmodels/member.dialog", "durandal/app"], function (dialog, app) {
                    var clone = ko.mapping.fromJS(ko.mapping.toJS(member));
                    dialog.member(clone);
                    app.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {
                                self.BlockCollection.replace(member, clone);
                            }
                        });
                });
            };
        },
        addBusinessRule = function () {
            var br = new bespoke.sph.domain.BusinessRule({ WebId: system.guid() });
            var self = this;

            require(["viewmodels/business.rule.dialog", "durandal/app"], function (dialog, app) {
                dialog.rule(br);
                app.showDialog(dialog)
                    .done(function (result) {
                        if (!result) return;
                        if (result === "OK") {
                            self.BusinessRuleCollection.push(br);
                        }
                    });
            });

        },
        editBusinessRule = function (rule) {
            var self = this;
            return function () {
                require(["viewmodels/business.rule.dialog", "durandal/app"], function (dialog, app) {
                    var clone = context.toObservable(ko.mapping.toJS(rule));
                    dialog.rule(clone);
                    app.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {
                                self.BusinessRuleCollection.replace(rule, clone);
                            }
                        });
                });
            };
        },
        removeBusinessRule = function (rule) {
            var self = this;
            return function () {
                self.BusinessRuleCollection.remove(rule);
            };
        },
        editMemberMap = function (member) {
            var building = this;
            return function () {
                console.log("show map ", building);
                console.log(" on member ", member);
                require(["viewmodels/member.map", "durandal/app"], function (dialog, app) {
                    dialog.init(building.BuildingId(), member.MemberPlanStoreId());
                    app.showDialog(dialog)
                        .done(function (result) {
                            if (result === "OK") {
                                member.MemberPlanStoreId(dialog.spatialStoreId());
                            }
                        });

                });

            };
        };
    return {
        addMember: addMember,
        editMember: editMember,
        removeMember: removeMember,
        addBusinessRule: addBusinessRule,
        editBusinessRule: editBusinessRule,
        removeBusinessRule: removeBusinessRule,
        editMemberMap: editMemberMap
    };
};

/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />



bespoke.sph.domain.VariablePartial = function () {

    var system = require('durandal/system'),
        hasError = ko.observable(),
        errors = ko.observableArray();
    return {
        hasError: hasError,
        errors: errors
    };
};
/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />


bespoke.sph.domain.ViewColumnPartial = function (model) {


    model.Path.subscribe(function (p) {
        if (!ko.unwrap(model.Header)) {
            model.Header(p.replace(".", " ")
                .replace(/([A-Z])/g, " $1").trim());
        }
    });
    // Filter
    var system = require("durandal/system"),
        addConditionalFormatting = function () {
            this.ConditionalFormattingCollection.push(new bespoke.sph.domain.ConditionalFormatting(system.guid()));
        },
        removeConditionalFormatting = function (cf) {
            var self = this;
            return function () {
                self.ConditionalFormattingCollection.remove(cf);
            };

        },
        removeIconCssClass = function () {
            this.IconCssClass("");
        },
        removeIconStoreId = function () {
            this.IconStoreId("");
        };
    return {
        addConditionalFormatting: addConditionalFormatting,
        removeConditionalFormatting: removeConditionalFormatting,
        removeIconStoreId: removeIconStoreId,
        removeIconCssClass: removeIconCssClass
    };
};
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

/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />



bespoke.sph.domain.WorkflowDesignerPartial = function () {
  
    return {
    };
};
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../schemas/form.designer.g.js" />

bespoke.sph.domain.WorkflowFormPartial = function () {

    var editOperationSuccessCallback = function () {
        var self = this,
            w = window.open("/sph/editor/ace?mode=javascript", "_blank", "height=" + screen.height + ",width=" + screen.width + ",toolbar=0,location=0,fullscreen=yes"),
            wdw = w.window || w,
            init = function () {
                wdw.code = ko.unwrap(self.OperationSuccessCallback);
                if (!w.code) {
                    w.code = "//insert your code here";
                }
                wdw.saved = function (code, close) {
                    self.OperationSuccessCallback(code);
                    if (close) {
                        w.close();
                    }
                };
            };
        if (wdw.attachEvent) { // for ie
            wdw.attachEvent("onload", init);
        } else {
            init();
        }
    },
    editOperationFailureCallback = function () {
        var self = this,
            w = window.open("/sph/editor/ace?mode=javascript", "_blank", "height=" + screen.height + ",width=" + screen.width + ",toolbar=0,location=0,fullscreen=yes"),
            wdw = w.window || w,
            init = function () {
                wdw.code = ko.unwrap(self.OperationFailureCallback);
                if (!w.code) {
                    w.code = "//insert your code here";
                }
                wdw.saved = function (code, close) {
                    self.OperationFailureCallback(code);
                    if (close) {
                        w.close();
                    }
                };
            };
        if (wdw.attachEvent) { // for ie
            wdw.attachEvent("onload", init);
        } else {
            init();
        }
    },
        canSetSuccessCallback = ko.computed(function () {

        });
    return {
        editOperationSuccessCallback: editOperationSuccessCallback,
        editOperationFailureCallback: editOperationFailureCallback,
        canSetSuccessCallback: canSetSuccessCallback
    };
};
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />


bespoke.sph.domain.WorkflowTriggerMapPartial = function () {
    var self = this,
        system = require('durandal/system'),
        showFieldDialog = function (accessor, field, path) {
            require(['viewmodels/' + path, 'durandal/app'], function (dialog, app2) {
                dialog.field(field);


                app2.showDialog(dialog)
                .done(function (result) {
                    if (!result) return;
                    if (result === "OK") {
                        accessor(field);
                    }
                });

            });
        };

    self.addField = function(accessor, type) {
        var field = new bespoke.sph.domain[type + 'Field'](system.guid());
            showFieldDialog(accessor, field, 'field.' + type.toLowerCase());
    };

    self.editField = function(field, accessor) {
        return function() {
            var fieldType = ko.unwrap(field.$type),
                clone = ko.mapping.fromJS(ko.mapping.toJS(field)),
                pattern = /Bespoke\.Sph\.Domain\.(.*?)Field,/,
                type = pattern.exec(fieldType)[1];


            showFieldDialog(accessor, clone, 'field.' + type.toLowerCase());

        };
    };

    return self;
};

bespoke.sph.domain.WorkflowTriggerMapPartial.prototype = new bespoke.sph.domain.FieldContainer();

/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../schemassystem.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="/Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="/Scripts/knockout-3.4.0.debug.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="/Scripts/require.js" />


bespoke.sph.domain.XmlTextFormatterPartial = function (model) {
    const system = require("durandal/system"),
        addParentElement = function () {
            this.ParentElementValueCollection.push(new bespoke.sph.domain.XmlElementTextFieldMapping(system.guid()));
        },
        removeParentElement = function (element) {
            var self = this;
            return function () {
                self.ParentElementValueCollection.remove(element);
            };
        },
        addParentAttribute = function () {
            this.ParentAttributeValueCollection.push(new bespoke.sph.domain.XmlAttributeTextFieldMapping({ WebId: system.guid(), TypeName: "System.String, mscorlib"}));
        },
        removeParentAttribute = function (attr) {
            var self = this;
            return function () {
                self.ParentAttributeValueCollection.remove(attr);
            };
        };

    

    return {
        addParentElement: addParentElement,
        removeParentElement: removeParentElement,
        addParentAttribute: addParentAttribute,
        removeParentAttribute: removeParentAttribute
    };
};
﻿///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\Activity.js
/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />

var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


bespoke.sph.domain.ActivityPartial = function () {

    var system = require('durandal/system'),
        hasError = ko.observable(),
        breakpoint = ko.observable(false),
        hit = ko.observable(false),
        errors = ko.observableArray();
    return {
        breakpoint: breakpoint,
        hit: hit,
        hasError: hasError,
        errors: errors
    };
};
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\AssemblyField.js
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
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\BarChartItem.js
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
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\BusinessRule.js
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

///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\ButtonPartial.js
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />

bespoke.sph.domain.ButtonPartial = function () {

    var editCommand = function () {
        var self = this,
            w = window.open("/sph/editor/ace?mode=javascript", '_blank', 'height=' + screen.height + ',width=' + screen.width + ',toolbar=0,location=0,fullscreen=yes'),
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
            wdw.attachEvent('onload', init);
        } else {
            init();
        }
    };
    return {
        editCommand: editCommand
    };
};
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\ComboBox.js
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="/Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="/Scripts/knockout-3.1.0.debug.js" />
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
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\ChildEntityListView.js
/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/durandal/system.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />

var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


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
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\ConstantField.js
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />

bespoke.sph.domain.ConstantFieldPartial = function (model) {

    var vm = {
        Type: ko.observable(),
        Value: ko.observable()
    };

    vm.Value.subscribe(function(v) {
        if (!ko.unwrap(model.Name)) {
            model.Name(v);
        }
    });
    return vm;
};
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\CreateEntityActivity.js
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
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\DataGridItem.js
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
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\DecisionActivity.js
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
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\DelayActivity.js
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
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\EntityDefinition.js
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="/Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="/Scripts/knockout-3.1.0.debug.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="/Scripts/require.js" />

bespoke.sph.domain.EntityDefinitionPartial = function () {
    var system = require('durandal/system'),
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
        },
        removeEntityOperation = function (operation) {
            var self = this;
            return function () {
                self.EntityOperationCollection.remove(operation);
            };
        },
        editMember = function (member) {
            var self = this;
            return function () {
                require(['viewmodels/member.dialog', 'durandal/app'], function (dialog, app) {
                    var clone = ko.mapping.fromJS(ko.mapping.toJS(member));
                    dialog.member(clone);
                    app.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result == "OK") {
                                self.BlockCollection.replace(member, clone);
                            }
                        });
                });
            };
        },
        addBusinessRule = function () {
            var br = new bespoke.sph.domain.BusinessRule({ WebId: system.guid() });
            var self = this;

            require(['viewmodels/business.rule.dialog', 'durandal/app'], function (dialog, app) {
                dialog.rule(br);
                app.showDialog(dialog)
                    .done(function (result) {
                        if (!result) return;
                        if (result == "OK") {
                            self.BusinessRuleCollection.push(br);
                        }
                    });
            });

        },
        editBusinessRule = function (rule) {
            var self = this;
            return function () {
                require(['viewmodels/business.rule.dialog', 'durandal/app'], function (dialog, app) {
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
                require(['viewmodels/member.map', 'durandal/app'], function (dialog, app) {
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
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\EntityOperation.js
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



bespoke.sph.domain.EntityOperationPartial = function () {

    var system = require('durandal/system'),
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

///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\EntityChart.js

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
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\EntityLookupElement.js
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
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
            w = window.open("/sph/editor/ace?mode=javascript", '_blank', 'height=' + screen.height + ',width=' + screen.width + ',toolbar=0,location=0,fullscreen=yes'),
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
            wdw.attachEvent('onload', init);
        } else {
            init();
        }
    },
        editColumns = function () {
            var self = this;
            require(['viewmodels/members.selector.dialog', 'durandal/app'], function (dialog, app2) {
                dialog.entity(self.Entity());
                app2.showDialog(dialog,self.Entity())
                    .done(function (result) {
                        if (!result) return;
                        if (result === "OK") {
                            self.LookupColumnCollection(dialog.selectedMembers());
                        }
                    });

            });
        };
    return {
        editDisplayTemplate: editDisplayTemplate,
        editColumns: editColumns
    };
};
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\EntityView.js
/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="/SphApp/services/system.js" />
/// <reference path="/SphApp/schemas/form.designer.g.js" />


bespoke.sph.domain.EntityViewPartial = function () {

    // Filter
    var system = require('durandal/system'),
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
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\ExecutedActivity.js
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
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\Filter.js
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

    var system = require('durandal/system'),
          showFieldDialog = function (accessor, field, path) {
              require(['viewmodels/' + path, 'durandal/app'], function (dialog, app2) {
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

///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\FormElement.js
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />



bespoke.sph.domain.FormElementPartial = function () {
    
    return {
    };
};


///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\HtmlElement.js
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
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\IFormTemplate.js

bespoke.sph.domain.FormTemplatePartial = function () {

    var vm = {
    };
    return vm;
}
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\IntervalSchedule.js
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
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\LineChartItem.js
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
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\ListenActivity.js
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
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\ListView.js
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
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\ListViewColumn.js
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
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\Member.js
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../schemassystem.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="/Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="/Scripts/knockout-3.1.0.debug.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="/Scripts/require.js" />


bespoke.sph.domain.MemberPartial = function () {
    var system = require('durandal/system'),
        addMember = function () {
            this.MemberCollection.push(new bespoke.sph.domain.Member(system.guid()));
        },
        editMember = function (member) {
            var self = this;
            return function () {
                require(['viewmodels/member.dialog', 'durandal/app'], function (dialog, app) {
                    var clone = ko.mapping.fromJS(ko.mapping.toJS(member));
                    dialog.member(clone);
                    app.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result == "OK") {
                                self.BlockCollection.replace(member, clone);
                            }
                        });
                });
            };
        },
        editPermission = function (member) {
            var self = this;
            require(['viewmodels/field.permission.dialog', 'durandal/app'], function (dialog, app) {
                var clone = ko.mapping.fromJS(ko.mapping.toJS(member));
                dialog.member(clone);
                app.showDialog(dialog)
                    .done(function (result) {
                        if (!result) return;
                        if (result == "OK") {
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
                require(['viewmodels/member.map', 'durandal/app'], function (dialog, app) {
                    dialog.init(building.BuildingId(), member.MemberPlanStoreId());
                    app.showDialog(dialog)
                        .done(function (result) {
                            if (result == "OK") {
                                member.MemberPlanStoreId(dialog.spatialStoreId());
                            }
                        });

                });

            };
        },
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
    return {
        editPermission: editPermission,
        addMember: addMember,
        editMember: editMember,
        editMemberMap: editMemberMap,
        removeMember: removeMember,
        addField: addField,
        editField: editField
    };
};
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\MethodArg.js
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

///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\NotificationActivity.js
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
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\ParallelActivity.js
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
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\report.js
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


///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\ReportDelivery.js
/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../prototypes/IntervalScheduleContainer.js" />
/// <reference path="../../App/durandal/system.js" />

bespoke.sph.domain.ReportDeliveryPartial = function () {

    return new bespoke.sph.domain.IntervalScheduleContainer();

};
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\Rule.js
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />



bespoke.sph.domain.RulePartial = function () {

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
        editField = function (field, accessor) {
            return function () {
                var fieldType = ko.unwrap(field.$type),
                    clone = ko.mapping.fromJS(ko.mapping.toJS(field)),
                    pattern = /Bespoke\.Sph\.Domain\.(.*?)Field,/,
                    type = pattern.exec(fieldType)[1];


                showFieldDialog(accessor, clone, 'field.' + type.toLowerCase());

            };
        };

    var vm = {
        addField: addField,
        editField: editField

    };

    return vm;
};

///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\ScheduledTriggerActivity.js
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

///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\ScreenActivity.js
/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />


bespoke.sph.domain.ScreenActivityPartial = function () {

    return {
        isAsync: true,
        canStart: true
    };
};
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\SearchDefinition.js
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />



bespoke.sph.domain.SearchDefinitionPartial = function () {

    var system = require('durandal/system'),
        removeFilter = function (child) {
            var self = this;
            return function() {
                self.FilterCollection.remove(child);
            };
        },
        addFilter = function() {
            var child = new bespoke.sph.domain.Filter(system.guid());
            this.FilterCollection.push(child);
        };

    var vm = {
        addFilter: addFilter,
        removeFilter: removeFilter

    };

    return vm;
};

///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\SetterAction.js
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />



bespoke.sph.domain.SetterActionPartial = function () {

    var system = require('durandal/system'),
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

///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\SetterActionChild.js
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

///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\StartWorkflowAction.js
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />




bespoke.sph.domain.StartWorkflowActionPartial = function () {

    var system = require('durandal/system'),
        removeMapping = function (map) {
            var self = this;
            return function() {
                self.WorkflowTriggerMapCollection.remove(map);
            };
        },
        addMapping = function() {
            var child = new bespoke.sph.domain.WorkflowTriggerMap(system.guid());
            child.Field({ Name: ko.observable("+ Field") });
            this.WorkflowTriggerMapCollection.push(child);
        };

    var vm = {
        addMapping: addMapping,
        removeMapping: removeMapping

    };

    return vm;
};

///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\Trigger.js
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
        removeAction = function(action) {
            var self = this;
            return function() {
                self.ActionCollection.remove(action);
            };
        },
        addAction = function (type) {
            var self = this;
            return function () {
                var action = new bespoke.sph.domain[type + 'Action'](system.guid());
                
                require(['viewmodels/action.' + type.toLowerCase(), 'durandal/app'], function (dialog, app2) {
                    dialog.action(action);
                    app2.showDialog(dialog)
                    .done(function (result) {
                        if (!result) return;
                        if (result === "OK") {
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
                var actionType = ko.unwrap(action.$type),
                    clone = ko.mapping.fromJS(ko.mapping.toJS(action)),
                    pattern = /Bespoke\.Sph\.Domain\.(.*?)Action,/,
                    type = pattern.exec(actionType)[1];

                require(['viewmodels/action.' + type.toLowerCase(), 'durandal/app'], function (dialog, app2) {
                    dialog.action(clone);
                    
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
            return function() {
                self.RuleCollection.remove(rule);
            };
        };

    var vm = {
        addRule: addRule,
        removeRule: removeRule,
        removeAction: removeAction,
        addAction: addAction,
        editAction: editAction

    };

    return vm;
};

///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\UpdateEntityActivity.js
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="/Scripts/require.js" />
/// <reference path="/Scripts/jquery-2.1.0.intellisense.js" />
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
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\Variable.js
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
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\ViewColumn.js
/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />


bespoke.sph.domain.ViewColumnPartial = function () {

    // Filter
    var system = require('durandal/system'),
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
            this.IconCssClass('');
        },
        removeIconStoreId = function () {
            this.IconStoreId('');
        };
    return {
        addConditionalFormatting: addConditionalFormatting,
        removeConditionalFormatting: removeConditionalFormatting,
        removeIconStoreId: removeIconStoreId,
        removeIconCssClass: removeIconCssClass
    };
};
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\WorkflowDefinition.js
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />


bespoke.sph.domain.WorkflowDefinitionPartial = function (model) {

    var system = require('durandal/system'),
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
                var activity = new bespoke.sph.domain[type + 'Activity'](system.guid());

                require(['viewmodels/activity.' + type.toLowerCase(), 'durandal/app'], function (dialog, app2) {
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
                var activityType = ko.unwrap(activity.$type),
                    clone = context.toObservable(ko.mapping.toJS(activity)),
                    pattern = /Bespoke\.Sph\.Domain\.(.*?)Activity,/,
                    type = pattern.exec(activityType)[1];

                isBusy(true);
                require(['viewmodels/activity.' + type.toLowerCase(), 'durandal/app'], function (dialog, app2) {
                    dialog.activity(clone);

                    if (typeof dialog.wd === "function") {
                        dialog.wd(self);
                    }

                    app2.showDialog(dialog)
                        .done(function (result) {
                            $('div.modalBlockout,div.modalHost').remove();
                            if (!result) return;
                            if (result === "OK") {
                                for (var g in activity) {
                                    if (typeof activity[g] === "function" && activity[g].name === "observable") {
                                        activity[g](ko.unwrap(clone[g]));
                                    } else {
                                        activity[g] = clone[g];
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
                var variable = new bespoke.sph.domain[type + 'Variable'](system.guid());

                require(['viewmodels/variable.' + type.toLowerCase(), 'durandal/app'], function (dialog, app2) {
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

                require(['viewmodels/variable.' + type.toLowerCase(), 'durandal/app'], function (dialog, app2) {
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
        },
        loadSchema = function (storeId) {
            var id = storeId || this.SchemaStoreId();
            $.get("/WorkflowDefinition/GetXsdElementName/" + id)
                .then(function (result) {
                    elementNameOptions(result);
                });

        };

    model.SchemaStoreId.subscribe(loadSchema);

    var vm = {
        isBusy: isBusy,
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
        setStartActivity: setStartActivity
    };

    return vm;
};

///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\WorkflowDesigner.js
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
///#source 1 1 C:\project\work\sph\source\web\core.sph\SphApp\partial\WorkflowTriggerMap.js
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />


bespoke.sph.domain.WorkflowTriggerMapPartial = function () {
    var self = this;
    return self;
};

bespoke.sph.domain.WorkflowTriggerMapPartial.prototype = new bespoke.sph.domain.FieldContainer();

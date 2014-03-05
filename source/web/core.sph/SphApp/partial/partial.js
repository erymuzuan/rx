﻿///#source 1 1 /SphApp/partial/Activity.js
/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
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
///#source 1 1 /SphApp/partial/AssemblyField.js
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
///#source 1 1 /SphApp/partial/BarChartItem.js
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
///#source 1 1 /SphApp/partial/BusinessRule.js
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />

bespoke.sph.domain.BusinessRulePartial = function (model) {

    var system = require('durandal/system'),
        context = require(objectbuilders.datacontext),
        logger = require(objectbuilders.logger),
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
        addRule: addRule,
        removeRule: removeRule
    };
};

///#source 1 1 /SphApp/partial/ButtonPartial.js
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
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
            init = function () {
                w.code = ko.unwrap(self.Command);
                if (!w.code) {
                    w.code = "//insert your code here";
                }
                w.saved = function (code, close) {
                    self.Command(code);
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
        editCommand: editCommand
    };
};
///#source 1 1 /SphApp/partial/ComboBox.js
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="/Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="/Scripts/knockout-3.0.0.debug.js" />
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
///#source 1 1 /SphApp/partial/ConstantField.js
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />

bespoke.sph.domain.ConstantFieldPartial = function () {
   
    return {
        Type: ko.observable(),
        Value: ko.observable()
    };
};
///#source 1 1 /SphApp/partial/CreateEntityActivity.js
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="/Scripts/require.js" />
/// <reference path="/Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="/Scripts/knockout-3.0.0.debug.js" />
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
///#source 1 1 /SphApp/partial/DataGridItem.js
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
///#source 1 1 /SphApp/partial/DecisionActivity.js
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="/Scripts/require.js" />
/// <reference path="/Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="/Scripts/knockout-3.0.0.debug.js" />
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
///#source 1 1 /SphApp/partial/DelayActivity.js
/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />

bespoke.sph.domain.DelayActivityPartial = function () {

    return {
        isAsync:true
    };
};
///#source 1 1 /SphApp/partial/EntityDefinition.js
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="/Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="/Scripts/knockout-3.0.0.debug.js" />
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
        removeMember = function (floor) {
            var self = this;
            return function () {
                self.MemberCollection.remove(floor);
            };
        },
        addBusinessRule = function () {
            this.BusinessRuleCollection.push(new bespoke.sph.domain.BusinessRule({ WebId: system.guid(), Name: '<New Rule>' }));
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
        addMember: addMember,
        editMember: editMember,
        removeMember: removeMember,
        addBusinessRule: addBusinessRule,
        editBusinessRule: editBusinessRule,
        removeBusinessRule: removeBusinessRule,
        editMemberMap: editMemberMap
    };
};
///#source 1 1 /SphApp/partial/EntityView.js
/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />


bespoke.sph.domain.EntityViewPartial = function () {

    // Filter
    var system = require('durandal/system'),
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
        addViewColumn: addViewColumn,
        removeViewColumn: removeViewColumn,
        addSort: addSort,
        removeSort: removeSort,
        addFilter: addFilter,
        removeFilter: removeFilter
    };
};
///#source 1 1 /SphApp/partial/ExecutedActivity.js
/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
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
///#source 1 1 /SphApp/partial/Filter.js
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />



bespoke.sph.domain.FilterPartial = function () {

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

///#source 1 1 /SphApp/partial/FormElement.js
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />



bespoke.sph.domain.FormElementPartial = function () {
    
    return {
    };
};


///#source 1 1 /SphApp/partial/HtmlElement.js
/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
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
///#source 1 1 /SphApp/partial/IFormTemplate.js

bespoke.sph.domain.FormTemplatePartial = function () {

    var vm = {
    };
    return vm;
}
///#source 1 1 /SphApp/partial/IntervalSchedule.js
/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
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
///#source 1 1 /SphApp/partial/LineChartItem.js
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
///#source 1 1 /SphApp/partial/ListenActivity.js
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="/Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="/Scripts/knockout-3.0.0.debug.js" />
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
///#source 1 1 /SphApp/partial/ListView.js
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
///#source 1 1 /SphApp/partial/ListViewColumn.js
/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />


bespoke.sph.domain.ListViewColumnPartial = function (model) {

    try {

        var pattern1 = /Bespoke\.Sph\.Domain\.(.*?),/,
            input1 = ko.unwrap(model.Input),
            name1 = pattern1.exec(ko.unwrap(input1.$type))[1],
            icon1 = '/images/form.element.' + name1 + '.png';


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
///#source 1 1 /SphApp/partial/Member.js
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../schemassystem.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="/Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="/Scripts/knockout-3.0.0.debug.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="/Scripts/require.js" />


bespoke.sph.domain.MemberPartial = function () {
    var system = require('durandal/system'),
        addMember = function() {
            this.MemberCollection.push(new bespoke.sph.domain.Member(system.guid()));
        },
        editMember = function(member) {
            var self = this;
            return function() {
                require(['viewmodels/member.dialog', 'durandal/app'], function(dialog, app) {
                    var clone = ko.mapping.fromJS(ko.mapping.toJS(member));
                    dialog.member(clone);
                    app.showDialog(dialog)
                        .done(function(result) {
                            if (!result) return;
                            if (result == "OK") {
                                self.BlockCollection.replace(member, clone);
                            }
                        });
                });
            };
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
        };
    return {
        addMember: addMember,
        editMember: editMember,
        editMemberMap: editMemberMap,
        removeMember: removeMember
    };
};
///#source 1 1 /SphApp/partial/MethodArg.js
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
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

///#source 1 1 /SphApp/partial/NotificationActivity.js
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
///#source 1 1 /SphApp/partial/ParallelActivity.js
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="/Scripts/require.js" />
/// <reference path="/Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="/Scripts/knockout-3.0.0.debug.js" />
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
///#source 1 1 /SphApp/partial/report.js
/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />




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


///#source 1 1 /SphApp/partial/ReportDelivery.js
/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../prototypes/IntervalScheduleContainer.js" />
/// <reference path="../../App/durandal/system.js" />

bespoke.sph.domain.ReportDeliveryPartial = function () {

    return new bespoke.sph.domain.IntervalScheduleContainer();

};
///#source 1 1 /SphApp/partial/Rule.js
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
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

///#source 1 1 /SphApp/partial/ScheduledTriggerActivity.js
/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
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

///#source 1 1 /SphApp/partial/ScreenActivity.js
/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />


bespoke.sph.domain.ScreenActivityPartial = function () {

    return {
        isAsync: true,
        canStart: true
    };
};
///#source 1 1 /SphApp/partial/SetterAction.js
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
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

///#source 1 1 /SphApp/partial/SetterActionChild.js
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />



bespoke.sph.domain.SetterActionChildPartial = function () {

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

///#source 1 1 /SphApp/partial/StartWorkflowAction.js
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

///#source 1 1 /SphApp/partial/Trigger.js
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
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

///#source 1 1 /SphApp/partial/UpdateEntityActivity.js
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="/Scripts/require.js" />
/// <reference path="/Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="/Scripts/knockout-3.0.0.debug.js" />
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
///#source 1 1 /SphApp/partial/Variable.js
/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
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
///#source 1 1 /SphApp/partial/WorkflowDefinition.js
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
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
        removeActivity: removeActivity,
        removeVariable: removeVariable,
        addActivity: addActivity,
        editActivity: editActivity,
        addVariable: addVariable,
        editVariable: editVariable,
        loadSchema: loadSchema,
        xsdElements: elementNameOptions,
        setStartActivity: setStartActivity
    };

    return vm;
};

///#source 1 1 /SphApp/partial/WorkflowDesigner.js
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />



bespoke.sph.domain.WorkflowDesignerPartial = function () {
  
    return {
    };
};
///#source 1 1 /SphApp/partial/WorkflowTriggerMap.js
/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />


bespoke.sph.domain.WorkflowTriggerMapPartial = function () {
    var self = this;
    return self;
};

bespoke.sph.domain.WorkflowTriggerMapPartial.prototype = new bespoke.sph.domain.FieldContainer();


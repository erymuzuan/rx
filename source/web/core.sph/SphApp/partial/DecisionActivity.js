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
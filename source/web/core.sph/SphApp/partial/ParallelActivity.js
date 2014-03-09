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
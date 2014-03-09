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
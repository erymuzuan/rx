/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="/Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="/Scripts/knockout-3.0.0.debug.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />

var bespoke = bespoke || {};

bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


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
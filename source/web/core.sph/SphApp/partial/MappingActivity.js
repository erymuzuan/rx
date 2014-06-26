﻿/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="/Scripts/require.js" />
/// <reference path="/Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="/Scripts/knockout-3.1.0.debug.js" />
/// <reference path="/Scripts/knockout.mapping-latest.debug.js" />



bespoke.sph.domain.MappingActivityPartial = function () {
    var system = require('durandal/system'),
        addMappingSource = function () {
            var self = this;
            var mapping = new bespoke.sph.domain['MappingSource'](system.guid());
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
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />

var bespoke = bespoke || {};
bespoke.sphcommercialspace = bespoke.sphcommercialspace || {};
bespoke.sphcommercialspace.domain = bespoke.sphcommercialspace.domain || {};

bespoke.sphcommercialspace.domain.CommercialSpacePartial = function () {

    var system = require('durandal/system'),
        getCustomField = function (name) {
        var cs = _(this.CustomFieldValueCollection()).find(function (v) {
            return v.Name() === name;
        });
        if (!cs) {
            throw "Cannot find custom field for " + name + " in CommercialSpace";
        }
        return cs.Value;
    },
        addPhoto = function() {
            this.PhotoCollection.push(new bespoke.sphcommercialspace.domain.Photo(system.guid()));
        },
        removePhoto = function(photo) {
            var self = this;
            return function () {
                self.PhotoCollection.remove(photo);
            };
        };
    return {
        CustomField: getCustomField,
        StaticMap: ko.observable("/images/no-image.png"),
        ApplicationTemplateOptions: ko.observableArray([]),
        addPhoto: addPhoto,
        removePhoto : removePhoto
    };
};

bespoke.sphcommercialspace.domain.RolePartial = function() {

    return {
        permissions: ko.observableArray()
    };
};
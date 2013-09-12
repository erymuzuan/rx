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
        getCustomList = function (name) {
            var cs = _(this.CustomListValueCollection()).find(function (v) {
                return v.Name() === name;
            });
            if (!cs) {
                throw "Cannot find custom list named " + name + " in Building";
            }
            return cs;
        },
        addCustomListItem = function (name) {

            return function () {
                var list = _(this.CustomListValueCollection()).find(function (v) {
                    return v.Name() === name;
                });
                var row = new bespoke.sphcommercialspace.domain.CustomListRow(system.guid());
                var columns = _(list.CustomFieldCollection()).map(function (f) {
                    var webid = system.guid();
                    var v = new bespoke.sphcommercialspace.domain.CustomFieldValue(webid);
                    v.Name(f.Name());
                    v.Type(f.Type());
                    return v;
                });

                row.CustomFieldValueCollection(columns);
                list.CustomListRowCollection.push(row);

            };
        },
        addPhoto = function() {
            this.PhotoCollection.push(new bespoke.sphcommercialspace.domain.Photo(system.guid()));
        },
        editPhoto = function (photo) {
            var self = this;
            return function () {
                require(['viewmodels/photo.dialog', 'durandal/app'], function (dialog, app2) {
                    var clone = ko.mapping.fromJS(ko.mapping.toJS(photo));
                    dialog.photo(clone);
                    app2.showModal(dialog)
                        .done(function (result) {
                            if (result == "OK") {
                                self.PhotoCollection.replace(photo, clone);
                            }
                        });

                });
            };
        },
        addPhoto = function () {
            var self = this;
            require(['viewmodels/photo.dialog', 'durandal/app'], function (dialog, app2) {
                var photo = new bespoke.sphcommercialspace.domain.Photo(system.guid());
                dialog.photo(photo);

                app2.showModal(dialog)
                .done(function (result) {
                    if (!result) return;
                    if (result == "OK") {
                        self.PhotoCollection.push(photo);
                    }
                });

            });
        },
        removePhoto = function (photo) {
            var self = this;
            return function () {
                self.PhotoCollection.remove(photo);
            };
        };
    return {
        CustomField: getCustomField,
        CustomList: getCustomList,
        addCustomListItem: addCustomListItem,
        StaticMap: ko.observable("/images/no-image.png"),
        ApplicationTemplateOptions: ko.observableArray([]),
        addPhoto: addPhoto,
        editPhoto: editPhoto,
        removePhoto: removePhoto
    };
};

bespoke.sphcommercialspace.domain.RolePartial = function () {

    return {
        permissions: ko.observableArray()
    };
};
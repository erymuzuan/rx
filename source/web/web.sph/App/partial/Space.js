/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/google-maps-3-vs-1-0-vsdoc.js" />


var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};

bespoke.sph.domain.SpacePartial = function () {

    var system = require('durandal/system'),
        context = require(objectbuilders.datacontext),
        logger = require(objectbuilders.logger),
        getCustomField = function (name) {
            var cs = _(this.CustomFieldValueCollection()).find(function (v) {
                return v.Name() === name;
            });
            if (!cs) {
                throw "Cannot find custom field for " + name + " in Space";
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
                var row = new bespoke.sph.domain.CustomListRow(system.guid());
                var columns = _(list.CustomFieldCollection()).map(function (f) {
                    var webid = system.guid();
                    var v = new bespoke.sph.domain.CustomFieldValue(webid);
                    v.Name(f.Name());
                    v.Type(f.Type());
                    return v;
                });

                row.CustomFieldValueCollection(columns);
                list.CustomListRowCollection.push(row);

            };
        },
        removeCustomListItem = function (name, row) {
            var self = this,
                list = _(self.CustomListValueCollection()).find(function (v) {
                    return v.Name() === name;
                });
            return function () {
                list.CustomListRowCollection.remove(row);
            };
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
                var photo = new bespoke.sph.domain.Photo(system.guid());
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
        },
        staticMap = ko.observable(),
        mapChanged = function (id) {

            if (!id) return;
            var pathTask = $.get("/Building/GetEncodedPath/" + id),
                centerTask = $.get("/Building/GetCenter/" + id);
            $.when(pathTask, centerTask)
                .then(function (path, center) {
                    if (center[0]) {
                        var location = center[0],
                            url = String.format("http://maps.google.com/maps/api/staticmap?center={0},{1}&"
                                + "size=640x300&markers={0},{1}&sensor=false", location.Lat, location.Lng);
                        staticMap(url);
                    }
                });
        },
        saveMap = function (map) {
            var tcs = new $.Deferred();
            context
                 .post(JSON.stringify(map), "/Space/SaveMap")
                 .then(function (e) {
                     tcs.resolve(true);
                     logger.log("Map has been successfully saved ", e, "buildingdetail", true);
                     mapChanged(map.buildingId);

                 });
            return tcs.promise();
        };
    

    return {
        CustomField: getCustomField,
        CustomList: getCustomList,
        addCustomListItem: addCustomListItem,
        removeCustomListItem: removeCustomListItem,
        staticMap: staticMap,
        ApplicationTemplateOptions: ko.observableArray([]),
        addPhoto: addPhoto,
        editPhoto: editPhoto,
        removePhoto: removePhoto,
        saveMap: saveMap
    };
};

bespoke.sph.domain.RolePartial = function () {

    return {
        permissions: ko.observableArray()
    };
};
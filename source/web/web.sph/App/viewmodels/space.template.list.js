/// <reference path="../objectbuilders.js" />
/// <reference path="../services/cultures.my.js" />
/// <reference path="/Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="/Scripts/knockout-2.3.0.debug.js" />
/// <reference path="/Scripts/string.js" />


define([objectbuilders.datacontext, objectbuilders.cultures],
    function (context, cultures) {
        var
            activate = function () {
                return true;
            };


        var vm = {
            activate: activate,
            templates: ko.observableArray(),
            toolbar: {
                addNew: {
                    location: '/#/template.space-id.0/0',
                    caption: 'Tambah Templat Baru'
                }
            },
            cultures: cultures
        };

        return vm;
    });

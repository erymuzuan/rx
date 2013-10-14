/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define([objectbuilders.datacontext],
    function (context) {

        var isBusy = ko.observable(false),
            activate = function () {
                return true;
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            lands: ko.observableArray(),
            searchTerm: {
                location: ko.observable(),
                keyword: ko.observable()
            },
            toolbar: {
                addNew: {
                    location: '/#/land.detail/0',
                    caption: 'Tanah Baru'
                }
            }
        };

        return vm;

    });

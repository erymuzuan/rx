/// <reference path="../../Scripts/jquery-2.1.0.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />



define(['services/datacontext', 'services/logger', 'plugins/dialog'],
    function(context, logger, dialog) {

        var definitionOptions = ko.observableArray(),
            destinationOptions = ko.observableArray(),
            wd = ko.observable(),
            activate = function(){
                destinationOptions(wd().VariableDefinitionCollection());
            },
            okClick = function (data, ev) {
            if (bespoke.utils.form.checkValidity(ev.target)) {
                dialog.close(this, "OK");
            }

        },
           cancelClick = function () {
              dialog.close(this, "Cancel");
           };

        var vm = {
            activate : activate,
            activity: ko.observable(new bespoke.sph.domain.MappingActivity()),
            okClick: okClick,
            cancelClick: cancelClick,
            definitionOptions : definitionOptions,
            destinationOptions : destinationOptions,
            wd : wd
        };


        return vm;

    });

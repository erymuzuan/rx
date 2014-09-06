/// <reference path="../../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', 'plugins/dialog'],
    function (context, logger, dialog) {

        var members = ko.observableArray(),
            selectedMembers = ko.observableArray(),
            _entity = ko.observable(),
            activate = function (ent) {
                var tcs = new $.Deferred();

                $.get('/sph/entitydefinition/GetVariablePath/' + ko.unwrap(ent))
                    .done(function(list){
                        members(list);
                        tcs.resolve(true);
                    });
                return tcs.promise();
            },
            attached = function(view){
                $(view).on('click', 'input[type=checkbox]', function(e){
                    if($(this).is(':checked')){
                        selectedMembers.push($(this).val());
                    }else  {
                        selectedMembers.remove($(this).val());
                    }
                });
            },
            okClick = function (data, ev) {
                    dialog.close(this, "OK");

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };

        var vm = {
            activate: activate,
            attached: attached,
            entity: _entity,
            selectedMembers: selectedMembers,
            members: members,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });



    define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router, objectbuilders.system, objectbuilders.app, objectbuilders.eximp, objectbuilders.dialog],
        function (context, logger, router, system, app, eximp, dialog) {

            var runningInDialog = ko.observable(),
                entityOptions = ko.observableArray(),
                formOptions = ko.observableArray(),
                wd = ko.observable(new bespoke.sph.domain.WorkflowDefinition(system.guid())),
                activity = ko.observable(new bespoke.sph.domain.ScreenActivity()),

                activate = function (wdid, screenid) {
                    var id = wd().Id();;

                    context.getListAsync("EntityDefinition", null, "Name")
                        .then(function(entities) {
                            var list = _(entities).map(function(v) {
                                return {
                                    text : v,
                                    value :v
                                };
                            });

                            list.push({ text: 'UserProfile*', value: 'UserProfile' });
                            list.push({ text: 'Designation*', value: 'Designation' });
                            list.push({ text: 'Department*', value: 'Department' });
                            entityOptions(list);
                        });
                    context.getListAsync("ScreenActivityForm", null, "Id")
                        .then(function (forms) {
                            formOptions(forms);
                        });

              
                    console.log(typeof wdid);
                    if (wdid && screenid) {
                        var query = String.format("Id eq '{0}'", id),
                        tcs = new $.Deferred();

                        context.loadOneAsync("WorkflowDefinition", query)
                            .done(function(b) {
                                wd(b);
                                tcs.resolve(true);
                                b.loadSchema();
                                var act = _(b.ActivityCollection()).find(function(v) { return v.WebId() == screenid; });
                                activity(act);
                            });

                 
                        return tcs.promise();

                    } else {
                        wd().loadSchema(); 
                    }
                    return Task.fromResult(true);
                },
                attached = function(view) {
                    runningInDialog(window.location.href.indexOf("screen.editor") < 0);
                    if (!activity().InvitationMessageBody())
                        activity().InvitationMessageBody("@@Model.Screen.Name task is assigned to you go here @@Model.Url");
                    if (!activity().InvitationMessageSubject())
                        activity().InvitationMessageSubject("[Sph] @@Model.Screen.Name  task is assigned to you");

                    if (!activity().CancelMessageBody())
                        activity().CancelMessageBody("@@Model.Screen.Name task was cancelled this url is not longer valid @@Model.Url");
                    if (!activity().CancelMessageSubject())
                        activity().CancelMessageSubject("[Sph] @@Model.Screen.Name was cancelled");

           
         
                
                },
                supportsHtml5Storage = function () {
                    try {
                        return 'localStorage' in window && window['localStorage'] !== null;
                    } catch (e) {
                        return false;
                    }
                },
                okClick = function (data, ev) {
                    if (bespoke.utils.form.checkValidity(ev.target)) {

                        dialog.close(this, "OK");
                        if (supportsHtml5Storage()) {
                            localStorage.removeItem(activity().WebId());
                        }
                    }
                },
                cancelClick = function () {
                    if (supportsHtml5Storage()) {
                        localStorage.removeItem(activity().WebId());
                    }
                    dialog.close(this, "Cancel");
                }

            var vm = {
                entityOptions: entityOptions,
                formOptions: formOptions,
                attached: attached,
                activate: activate,
                activity: activity,
                wd :wd,
                okClick: okClick,
                cancelClick: cancelClick,
                toolbar : {
                    commands :ko.observableArray([
                    ])
                }
            };
            

            return vm;

        });


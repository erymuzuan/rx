define(["services/datacontext", "services/logger", "plugins/router", "durandal/system", "services/validation", "services/jsonimportexport", "plugins/dialog", "services/watcher", "services/config", "durandal/app"],
   function (context, logger, router, system, validation, eximp, dialog, watcher, config, app) {

       var item = ko.observable(new bespoke.sph.domain.SimpleScreenWorkflow(system.guid())),
          errors = ko.observableArray(),
          watching = ko.observable(false),
          id = ko.observable(),
          form = ko.observable(new bespoke.sph.domain.EntityForm());


       var activate = function (itemId) {
           id(itemId);
           var query = String.format("Id eq '{0}'", itemId),
               tcs = new $.Deferred(),
               itemTask = context.loadOneAsync("Simple Screen Workflow", query),
               formTask = context.loadOneAsync("EntityForm", "Route eq 'test-form'"),
               watcherTask = watcher.getIsWatchingAsync("Simple Screen Workflow", itemId);
           $.when(itemTask, formTask, watcherTask).done(function (b, f, w) {
               if (b) {
                   var item2 = context.toObservable(b);
                   item(item2);
               }
               else {
                   item(new bespoke.sph.domain.SimpleScreenWorkflow(system.guid()));
               }
               form(f);
               watching(w);
               tcs.resolve(true);

           });
           return tcs.promise();

       },
          attached = function (view) {
              // validation
              validation.init($('#test-form-form'), form());

          },
          save = function ($data) {
              if (!validation.valid()) {
                  return Task.fromResult(false);
              }
              var tcs = new $.Deferred(),
                data = ko.toJSON(item);
              context.post(data, "/Simple Screen Workflow/Save")
                      .then(function (result) {
                          tcs.resolve(result);
                          item().Id(result.id);
                          app.showMessage("Your Simple Screen Workflow has been successfully saved", "Reactive Developer platform showcase", ["Ok"]);
                      });
              return tcs.promise();

          },
          buttonClick = function () {
              return moment();
          };

       var vm = {
           activate: activate,
           config: config,
           attached: attached,
           item: item,
           errors: errors,
           save: save,
           toolbar: {
               commands: ko.observableArray([{ caption: "", command: buttonClick, icon: "" }])
           }
       }
       return vm;

   }
);

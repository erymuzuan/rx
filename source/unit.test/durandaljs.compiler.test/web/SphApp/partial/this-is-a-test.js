define(["services/datacontext", "services/logger", "plugins/router", "durandal/system", "services/validation", "services/jsonimportexport", "plugins/dialog", "services/watcher", "services/config", "durandal/app", "partial/this-is-a-test"],
   function(context, logger, router, system, validation, eximp, dialog, watcher, config, app, partial){

   var item = ko.observable(new bespoke.sph.domain.Course(system.guid())),
      errors = ko.observableArray(),
      watching = ko.observable(false),
      id = ko.observable(),
      form = ko.observable(new bespoke.sph.domain.EntityForm());


var attached  = function(view){ 
   TODO : compiled attached method Bespoke.Sph.Domain.EntityForm
   };

   var vm = { 
      activate : activate,
      attached : attached
   }
   return vm;

}
);

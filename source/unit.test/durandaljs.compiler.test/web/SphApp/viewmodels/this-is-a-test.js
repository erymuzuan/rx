define(["services/datacontext", "services/logger", "plugins/router", "durandal/system", "services/validation", "services/jsonimportexport", "plugins/dialog", "services/watcher", "services/config", "durandal/app", "partial/this-is-a-test"],
   function(context, logger, router, system, validation, eximp, dialog, watcher, config, app, partial){

   var item = ko.observable(new bespoke.sph.domain.Course(system.guid())),
      errors = ko.observableArray(),
      watching = ko.observable(false),
      id = ko.observable(),
      form = ko.observable(new bespoke.sph.domain.EntityForm());


var activate  = function(itemId){ 
      id(itemId);
       var query = String.format("Id eq '{0}'", itemId),
           tcs = new $.Deferred(),
           itemTask = context.loadOneAsync("Course", query),
           formTask = context.loadOneAsync("EntityForm", "Route eq 'this-is-a-test'"),
           watcherTask = watcher.getIsWatchingAsync("Course", itemId);
       $.when(itemTask, formTask, watcherTask).done(function(b,f,w) {
           if (b) { 
               var item2 = context.toObservable(b);
               item(item2);
           }
           else {
               item(new bespoke.sph.domain.Course(system.guid()));
           }
           form(f);
           watching(w);
           if(typeof partial.activate === "function"){
               var pt = partial.activate(item());
               if(typeof pt.done === "function"){
                   pt.done(tcs.resolve);
               }
               else{
                   tcs.resolve(true);
               }
           }
       
       });
       return tcs.promise();

   },
   attached  = function(view){ 
           // validation
        validation.init($('#this-is-a-test-form'), form());
        if( typeof partial.attached === "function"){
           partial.attached(view);
         }

   },
   email  = function(){ 
   //CreateEmailCourseBespoke.Sph.Domain.EntityForm
   },
   registerCourse  = function($data){ 
          if (!validation.valid()) {
          return Task.fromResult(false);
       }
       
       var tcs = new $.Deferred(),
           data = ko.mapping.toJSON(item);
       
       context.post(data, "course/RegisterCourse")
       .then(function (result) {
           if (result.success) {
               logger.info(result.message);
               item().Id(result.id);
               errors.removeAll();
 
                        app.showMessage("Ok done", "Reactive Developer platform showcase", ["OK"])
	                        .done(function () {
                                window.location="/sph#course";
	                        });
                                 
           } else {
               errors.removeAll();
               _(result.rules).each(function(v){
               errors(v.ValidationErrors);
           });
           
           logger.error("There are errors in your entity, !!!");
           }
           tcs.resolve(result);
       });

       return tcs.promise();

   },
   deregisterCourse  = function($data){ 
          if (!validation.valid()) {
          return Task.fromResult(false);
       }
       
       var tcs = new $.Deferred(),
           data = ko.mapping.toJSON(item);
       
       context.post(data, "course/DeregisterCourse")
       .then(function (result) {
           if (result.success) {
               logger.info(result.message);
               item().Id(result.id);
               errors.removeAll();

           } else {
               errors.removeAll();
               _(result.rules).each(function(v){
               errors(v.ValidationErrors);
           });
           
           logger.error("There are errors in your entity, !!!");
           }
           tcs.resolve(result);
       });

       return tcs.promise();

   },
   save  = function($data){ 
       if (!validation.valid()) {
           return Task.fromResult(false);
       }
     var tcs = new $.Deferred(),
       data = ko.toJSON(item);
       context.post(data, "/business-rule/Course/validate/Rule1;Rule2")
           .then(function(result) {
               if(result.success){
                   context.post(data, "/Course/Save")
                       .then(function(sr) {
                           tcs.resolve(sr);
                           item().Id(sr.id);
                           app.showMessage("Your Course has been successfully saved", "Reactive Developer platform showcase", ["Ok"]);
                       });
               }
               else {
                   var ve = _(result.validationErrors).map(function(v){ return { Message : v.message}; });
                   errors(ve);
                   logger.error("There are errors in your entity, !!!");
                   tcs.resolve(result);
               }
           });
       return tcs.promise();

   },
   buttonClick  = function(){ 
   return moment();
   };

   var vm = { 
      registerCourse : registerCourse,
      deregisterCourse : deregisterCourse,
      activate : activate,
      config : config,
      attached : attached,
      item : item,
      errors : errors,
      save : save,
      toolbar : {
       emailCommand : {
           entity : "Course",
           id :id
       },
       commands : ko.observableArray([{ caption :"", command : buttonClick, icon:"" }]) 
      },
      partial : partial
   }
   return vm;

}
);

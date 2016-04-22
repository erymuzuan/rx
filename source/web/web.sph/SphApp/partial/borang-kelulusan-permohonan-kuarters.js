define([objectbuilders.datacontext], function(context){
    var activate = function(message, id){
            
        return context.loadOneAsync("Workflow", "Id eq '" + id + "'")
            .then(function(wf){
                if(wf){
                    message.NoDaftar(ko.unwrap(wf.NoDaftar));
                }
            });


        },
        attached  = function(view){
        
        };

    return {
        activate : activate,
        attached : attached
    };

});
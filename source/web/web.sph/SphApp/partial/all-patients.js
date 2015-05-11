define([], function(){
    var activate = function(list){
            
            var tcs = new $.Deferred();
            setTimeout(function(){
                tcs.resolve(true);
            }, 500);

            return tcs.promise();


        },
        attached  = function(view){
        
        };

    return {
        activate : activate,
        attached : attached
    };

});
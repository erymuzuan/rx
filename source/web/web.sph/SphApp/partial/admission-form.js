define([], function(){
    var activate = function(patient){
            console.log("From partial me");
            var tcs = new $.Deferred();
            setTimeout(function(){
                tcs.resolve(true);
            }, 165);

            return tcs.promise();


        },
        attached  = function(view){
        
        };

    return {
        activate : activate,
        attached : attached
    };

});
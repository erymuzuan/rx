define([], function(){
    var map  = function(p){
            p.Bil = ko.observable(500);
            return p;
        };

    return {
        map : map
    };

}); 
define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.system, objectbuilders.app], 
    function(context, logger, router, system, app){
        var list = ko.observableArray(),
            pageList = ko.observableArray(),
            size = 20,
            activate = function(id){
            
                for (var i = 0; i < 10; i++) {
                     list.push({
                         "Column1" : (1 +i).toString(),
                         "Column2" : (1 +i).toString(),
                         "Column3" : (1 +i).toString(),
                         "Column4" : (1 +i).toString()
                     });
                }
                
                return true;
    
    
            },
            attached  = function(view){
                var pager = new bespoke.utils.ServerPager({
                    element : $("#ima-pager"),
                    count : list().length,
                    changed : function(pageNo, pageSize){
                        var currentPage = [];
                        for (var i = ((pageNo-1) * pageSize); i < (pageNo * pageSize); i++) {
                            if(i < list().length){
                                currentPage.push(list()[i]);
                            }
                        }
                        pageList(currentPage);
                    }
                });
                
                var page1 = [];
                for (var i = 0; i < size; i++) {
                    if(i < list().length){
                        page1.push(list()[i]);
                    }
                }
                pageList(page1);
            };
    
        return {
            pageList  : pageList ,
            list : list,
            activate : activate,
            attached : attached
        };

});
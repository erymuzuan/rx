define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.system, objectbuilders.app], function(context, logger, router, system, app){
    var from = ko.observable(moment().subtract(1, "day").format("YYYY-MM-DD")),
        to = ko.observable(moment().add(1, "day").format("YYYY-MM-DD")),
        patients = ko.observableArray(),
        pageFrom = ko.observable(0),
        pageSize = ko.observable(20),
        query = 
        {
           "query": {
              "range": {
                 "RegisteredDate": {
                    "from": from,
                    "to": to
                 }
              }
           },
           "fields": [
              "FullName",
              "Mrn"
           ],
           "from": pageFrom,
           "size": pageSize
        },
        total = ko.observable(0),
        searchAsync = function(){
            return context.post(ko.toJSON(query), "/api/patients/search")
                .then(function (result) {
                    total(result.hits.total);
                    var list  = _(result.hits.hits).map(function(v){
                        return {
                            "FullName":v.fields.FullName[0],
                            "Mrn": v.fields.Mrn[0]
                        };
                    });
                    patients(list);
                });
        },
        activate = function(){
            return true;
        },
        attached  = function(view){
            var option = 
                {
                    ranges: {
                        'Today': [moment(), moment()],
                        'Yesterday': [moment().subtract(1, "days"), moment().subtract(1, "days")],
                        'Last 7 Days': [moment().subtract(6, "days"), moment()],
                        'Last 30 Days': [moment().subtract(29, "days"), moment()],
                        'This Month': [moment().startOf("month"), moment().endOf("month")],
                        'Last Month': [
                            moment().subtract(1, "month").startOf("month"),
                            moment().subtract(1, "month").endOf("month")
                        ]
                    },
                    startDate: moment().subtract(29, "days"),
                    endDate: moment()
                },
                changed = function(start, end){
                    from(start.format("YYYY-MM-DD"));
                    to(end.format("YYYY-MM-DD"));
                    return searchAsync();
                };
           $(view).find("div.date-range").daterangepicker(option, changed);
           total.subscribe(function(count){
                var pager = new bespoke.utils.ServerPager({
                    element : $("#patient-pager"),
                    count : count,
                    changed : function(pageNo, size){
                        pageFrom((pageNo - 1) * size);
                        pageSize(size);
                        
                        return searchAsync();
                        
                    }
                });
           });
           
        };

    return {
        activate : activate,
        attached : attached,
        from: from,
        to : to,
        patients : patients
    };
    

});
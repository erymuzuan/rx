define(['services/datacontext'],
    function () {
        var activate = function () {
            return true;
        };

        var vm = {
            activate: activate,
            reports: ko.observableArray([]),
            toolbar: {
                addNew: {
                    location :'#/reportdefinition.edit/0',
                    caption : 'Create new report'
                },
            }
        };

        return vm;

    });
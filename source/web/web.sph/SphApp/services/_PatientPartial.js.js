define(["services/datacontext"],
    function(context)
        {

            var activate = function() {
                    return true;

                },
                attached = function(view) {

                };

            var vm = {
            activate : activate,
            attached: attached
        };


        return vm;

    });
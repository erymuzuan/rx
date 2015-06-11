
define([], function(){
    
    
    
    ko.bindingHandlers['customPaging'] = {
        makeTemplateValueAccessor: function(valueAccessor) {
            return function() {
                var modelValue = valueAccessor(),
                    unwrappedValue = ko.utils.peekObservable(modelValue);    // Unwrap without setting a dependency here
    
                // If unwrappedValue is the array, pass in the wrapped value on its own
                // The value will be unwrapped and tracked within the template binding
                // (See https://github.com/SteveSanderson/knockout/issues/523)
                if ((!unwrappedValue) || typeof unwrappedValue.length == "number")
                    return { 'foreach': modelValue, 'templateEngine': ko.nativeTemplateEngine.instance };
    
                // If unwrappedValue.data is the array, preserve all relevant options and unwrap again value so we get updates
                ko.utils.unwrapObservable(modelValue);
                return {
                    'foreach': unwrappedValue['data'],
                    'as': unwrappedValue['as'],
                    'includeDestroyed': unwrappedValue['includeDestroyed'],
                    'afterAdd': unwrappedValue['afterAdd'],
                    'beforeRemove': unwrappedValue['beforeRemove'],
                    'afterRender': unwrappedValue['afterRender'],
                    'beforeMove': unwrappedValue['beforeMove'],
                    'afterMove': unwrappedValue['afterMove'],
                    'templateEngine': ko.nativeTemplateEngine.instance
                };
            };
        },
        'displayItems' : function(element, template, list){
            var html = "";
            _(ko.unwrap(list)).each(function(v){
                // TODO -  still looking how to produce the HTML from the template
                html += template;
            });
            $(element).html(html);

        },
        'init': function(element, valueAccessor, allBindings, viewModel, bindingContext) {
            
            var pageList = ko.observableArray(),
                value = valueAccessor(),
                list = ko.unwrap(value.list),
                pager = ko.unwrap(value.pager),
                size = 20,
                template = $(element).html();

              var pager = new bespoke.utils.ServerPager({
                    element : $(pager),
                    count : list.length,
                    changed : function(pageNo, pageSize){
                        var currentPage = [];
                        for (var i = ((pageNo-1) * pageSize); i < (pageNo * pageSize); i++) {
                            if(i < list.length){
                                currentPage.push(list[i]);
                            }
                        }
                        pageList(currentPage);
                        ko.bindingHandlers['customPaging'].displayItems(element, template, pageList);
                    }
                });
                
            
            var page1 = [];
            for (var i = 0; i < size; i++) {
                if(i < list.length){
                    page1.push(list[i]);
                }
            }
            pageList(page1);
            ko.bindingHandlers['customPaging'].displayItems(element, template, pageList);
                
        },
        'update': function(element, valueAccessor, allBindings, viewModel, bindingContext) {
            return ko.bindingHandlers['template']['update'](element, ko.bindingHandlers['customPaging'].makeTemplateValueAccessor(valueAccessor), allBindings, viewModel, bindingContext);
        }
    };
        
    ko.expressionRewriting.bindingRewriteValidators['customPaging'] = false; // Can't rewrite control flow bindings
    ko.virtualElements.allowedBindings['customPaging'] = true;

    return {};
});
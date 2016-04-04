/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../../core.sph/Scripts/require.js" />
/// <reference path="../../../core.sph/Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../../core.sph/Scripts/jstree.min.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />


define(["services/datacontext", "services/logger", "plugins/router"],
    function (context, logger, router) {

        var isBusy = ko.observable(false),
            operationEndpoints = ko.observableArray(),
            queryEndpoints = ko.observableArray(),
            entities = ko.observableArray(),
            permissions = ko.observableArray(),
            selected = ko.observable(),
            activate = function () {
                return context.loadAsync("OperationEndpoint")
                    .then(function (lo) {
                        operationEndpoints(lo.itemCollection);
                        return context.loadAsync("QueryEndpoint");
                    })
                    .then(function (lo) {
                        queryEndpoints(lo.itemCollection);
                        return context.loadAsync("EntityDefinition");
                    })
                    .then(function (lo) {
                        entities(lo.itemCollection);
                        
                        return $.getJSON("/management-api/endpoint-permissions");
                    })
                    .then(permissions);
            },
            singleClick = function (e, data) {
                selected(data.node.data);
            },
            click = function (e) {
                e.stopPropagation();
                var data = selected().node.data;
                console.log(data);
            },
            searchPermission = function(parent, controller, action){
                var perm = _(ko.unwrap(permissions)).find(function(v){
                            return ko.unwrap(v.Parent) === parent && 
                                    ko.unwrap(v.Controller) === controller && 
                                    ko.unwrap(v.Action) === action;
                        });
                if(perm){
                    return perm;
                }
                return  {
                    Parent : parent, 
                    Controller : controller, 
                    Action : action, 
                    Claims : ko.observableArray()
                };
            },
            attached = function (view) {
                var $panel = $(view).find("#endpoints-tree-panel"),
                    items = _(ko.unwrap(entities)).map(function (v) {
                        
                        var perm = _(ko.unwrap(permissions)).find(function(v){
                            return ko.unwrap(v.Parent) === ko.unwrap(v.Name) && !ko.unwrap(v.Controller) && !ko.unwrap(v.Action);
                        }) || {
                                Parent: ko.unwrap(v.Name),
                                Controller: null,
                                Action : null,
                                Claims : ko.observableArray()
                            };
                        return {
                            data: perm,
                            parent: "#",
                            text: ko.unwrap(v.Name),
                            icon: ko.unwrap(v.IconClass),
                            children: [{
                                data : searchPermission(ko.unwrap(v.Name),ko.unwrap(v.Name) + "ServiceContract", "Search"),
                                text : "Search",
                                icon : "fa fa-search"
                            },{
                                data : searchPermission(ko.unwrap(v.Name),ko.unwrap(v.Name) + "ServiceContract", "GetOneByIdAsync"),
                                text : "GetOneByIdAsync",
                                icon : "fa fa-file-o"
                            },{
                                data :  searchPermission(ko.unwrap(v.Name),ko.unwrap(v.Name) + "ServiceContract", "OdataApi"),
                                text : "OdataApi",
                                icon : "fa fa-database"
                            }]
                        };
                    });
                _(queryEndpoints()).each(function (v) {
                    var parent = _(items).find(function (k) {
                        return k.text === ko.unwrap(v.Entity);
                    });
                    if (!parent) {
                        return;
                    }
                    
                      
                    var perm = _(ko.unwrap(permissions)).find(function(v){
                        return ko.unwrap(v.Parent) === ko.unwrap(v.Entity) && ko.unwrap(v.Controller) && !ko.unwrap(v.Action);
                    }) || {
                            Parent: ko.unwrap(v.Name),
                            Controller: null,
                            Action : null,
                            Claims : ko.observableArray()
                        };
                            
                    var q = {
                        data: perm,
                        text: ko.unwrap(v.Name),
                        icon: "fa fa-cubes",
                        children : []
                    };                    
                    parent.children.push(q);
                    var getOneNode = {
                        data: perm,
                        text: "GetAction",
                        icon: "fa fa-list"
                    };
                    q.children.push(getOneNode);
                    q.children.push({
                        data: perm,
                        text: "GetCount",
                        icon: "fa fa-tachometer"}
                       );

                });
                
                _(operationEndpoints()).each(function (v) {
                    var parent = _(items).find(function (k) {
                        return k.text === ko.unwrap(v.Entity);
                    });
                    if (!parent) {
                        return;
                    }
                    
                      
                    var perm = _(ko.unwrap(permissions)).find(function(v){
                        return ko.unwrap(v.Parent) === ko.unwrap(v.Entity) && ko.unwrap(v.Controller) && !ko.unwrap(v.Action);
                    }) || {
                            Parent: ko.unwrap(v.Name),
                            Controller: null,
                            Action : null,
                            Claims : ko.observableArray()
                        };
                            
                    var q = {
                        data: {
                            Parent: ko.unwrap(v.Entity),
                            Controller: ko.unwrap(),
                            Action : ko.unwrap(),
                            Claims: ko.observableArray()
                        },
                        text: ko.unwrap(v.Name),
                        icon: "fa fa-cogs",
                        children : []
                    };
                    parent.children.push(q);
                    
                    if(ko.unwrap(v.IsHttpPost)){                        
                        var postNode = {
                            data: perm,
                            text: "HTTP POST",
                            icon: "fa fa-plus"
                        };
                        q.children.push(postNode);
                    }
                    
                    
                    if(ko.unwrap(v.IsHttpPut)){                        
                        var putNode = {
                            data: perm,
                            text: "HTTP PUT",
                            icon: "fa fa-file-text-o"
                        };
                        q.children.push(putNode);
                    }
                    
                    if(ko.unwrap(v.IsHttpPatch)){                        
                        var patchNode = {
                            data: perm,
                            text: "HTTP PATCH",
                            icon: "fa fa-pencil-square"
                        };
                        q.children.push(patchNode);
                    }

                    if(ko.unwrap(v.IsHttpDelete)){                        
                        var deleteNode = {
                            data: perm,
                            text: "HTTP DELETE",
                            icon: "fa fa-minus-circle"
                        };
                        q.children.push(deleteNode);
                    }


                });

                $panel.jstree({
                    'core': {
                        'data': items
                    },
                    "search": {
                        "case_sensitive": false,
                        "show_only_matches": true,
                        "show_only_matches_children": true,
                        "search_callback": function (text, node) {
                            return (node.text.indexOf(text) > -1);
                        }
                    },
                    "plugins": ["search"]
                });

                $panel.on("select_node.jstree", singleClick);
                $panel.delegate("a", "dblclick", click);
            },
            addClaims = function() {
                selected().Claims.push({
                    Type : ko.observable(),
                    Value: ko.observable(),
                    Permission : ko.observable("Inherited allow")
                });
            },
            save = function() {
                var json = ko.toJSON(permissions);
                return context.post(json, "/management-api/endpoint-permissions");
                
            };

        return {
            operationEndpoints: operationEndpoints,
            queryEndpoints: queryEndpoints,
            selected: selected,
            addClaims: addClaims,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            toolbar : {
                saveCommand : save
            }
        };
    });

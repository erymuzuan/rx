/**
 * Durandal 2.1.0 Copyright (c) 2012 Blue Spire Consulting, Inc. All Rights Reserved.
 * Available via the MIT license.
 * see: http://durandaljs.com or https://github.com/BlueSpire/Durandal for details.
 */
/**
 * The system module encapsulates the most basic features used by other modules.
 * @module system
 * @requires require
 * @requires jquery
 */
define('durandal/system',['require', 'jquery'], function(require, $) {
    var isDebugging = false,
        nativeKeys = Object.keys,
        hasOwnProperty = Object.prototype.hasOwnProperty,
        toString = Object.prototype.toString,
        system,
        treatAsIE8 = false,
        nativeIsArray = Array.isArray,
        slice = Array.prototype.slice;

    //polyfill from https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/String/Trim
    if (!String.prototype.trim) {
        String.prototype.trim = function () {
            return this.replace(/^\s+|\s+$/g, '');
        };
    }

    //see http://patik.com/blog/complete-cross-browser-console-log/
    // Tell IE9 to use its built-in console
    if (Function.prototype.bind && (typeof console === 'object' || typeof console === 'function') && typeof console.log == 'object') {
        try {
            ['log', 'info', 'warn', 'error', 'assert', 'dir', 'clear', 'profile', 'profileEnd']
                .forEach(function(method) {
                    console[method] = this.call(console[method], console);
                }, Function.prototype.bind);
        } catch (ex) {
            treatAsIE8 = true;
        }
    }

    // callback for dojo's loader 
    // note: if you wish to use Durandal with dojo's AMD loader,
    // currently you must fork the dojo source with the following
    // dojo/dojo.js, line 1187, the last line of the finishExec() function: 
    //  (add) signal("moduleLoaded", [module.result, module.mid]);
    // an enhancement request has been submitted to dojo to make this
    // a permanent change. To view the status of this request, visit:
    // http://bugs.dojotoolkit.org/ticket/16727

    if (require.on) {
        require.on("moduleLoaded", function(module, mid) {
            system.setModuleId(module, mid);
        });
    }

    // callback for require.js loader
    if (typeof requirejs !== 'undefined') {
        requirejs.onResourceLoad = function(context, map, depArray) {
            system.setModuleId(context.defined[map.id], map.id);
        };
    }

    var noop = function() { };

    var log = function() {
        try {
            // Modern browsers
            if (typeof console != 'undefined' && typeof console.log == 'function') {
                // Opera 11
                if (window.opera) {
                    var i = 0;
                    while (i < arguments.length) {
                        console.log('Item ' + (i + 1) + ': ' + arguments[i]);
                        i++;
                    }
                }
                // All other modern browsers
                else if ((slice.call(arguments)).length == 1 && typeof slice.call(arguments)[0] == 'string') {
                    console.log((slice.call(arguments)).toString());
                } else {
                    console.log.apply(console, slice.call(arguments));
                }
            }
            // IE8
            else if ((!Function.prototype.bind || treatAsIE8) && typeof console != 'undefined' && typeof console.log == 'object') {
                Function.prototype.call.call(console.log, console, slice.call(arguments));
            }

            // IE7 and lower, and other old browsers
        } catch (ignore) { }
    };

    var logError = function(error, err) {
        var exception;
        
        if(error instanceof Error){
            exception = error;
        } else {
            exception = new Error(error);
        }
        
        exception.innerError = err;
        
        //Report the error as an error, not as a log
        try {
            // Modern browsers (it's only a single item, no need for argument splitting as in log() above)
            if (typeof console != 'undefined' && typeof console.error == 'function') {
                console.error(exception);
            }
            // IE8
            else if ((!Function.prototype.bind || treatAsIE8) && typeof console != 'undefined' && typeof console.error == 'object') {
                Function.prototype.call.call(console.error, console, exception);
            }
            // IE7 and lower, and other old browsers
        } catch (ignore) { }

        throw exception;
    };

    /**
     * @class SystemModule
     * @static
     */
    system = {
        /**
         * Durandal's version.
         * @property {string} version
         */
        version: "2.1.0",
        /**
         * A noop function.
         * @method noop
         */
        noop: noop,
        /**
         * Gets the module id for the specified object.
         * @method getModuleId
         * @param {object} obj The object whose module id you wish to determine.
         * @return {string} The module id.
         */
        getModuleId: function(obj) {
            if (!obj) {
                return null;
            }

            if (typeof obj == 'function' && obj.prototype) {
                return obj.prototype.__moduleId__;
            }

            if (typeof obj == 'string') {
                return null;
            }

            return obj.__moduleId__;
        },
        /**
         * Sets the module id for the specified object.
         * @method setModuleId
         * @param {object} obj The object whose module id you wish to set.
         * @param {string} id The id to set for the specified object.
         */
        setModuleId: function(obj, id) {
            if (!obj) {
                return;
            }

            if (typeof obj == 'function' && obj.prototype) {
                obj.prototype.__moduleId__ = id;
                return;
            }

            if (typeof obj == 'string') {
                return;
            }

            obj.__moduleId__ = id;
        },
        /**
         * Resolves the default object instance for a module. If the module is an object, the module is returned. If the module is a function, that function is called with `new` and it's result is returned.
         * @method resolveObject
         * @param {object} module The module to use to get/create the default object for.
         * @return {object} The default object for the module.
         */
        resolveObject: function(module) {
            if (system.isFunction(module)) {
                return new module();
            } else {
                return module;
            }
        },
        /**
         * Gets/Sets whether or not Durandal is in debug mode.
         * @method debug
         * @param {boolean} [enable] Turns on/off debugging.
         * @return {boolean} Whether or not Durandal is current debugging.
         */
        debug: function(enable) {
            if (arguments.length == 1) {
                isDebugging = enable;
                if (isDebugging) {
                    this.log = log;
                    this.error = logError;
                    this.log('Debug:Enabled');
                } else {
                    this.log('Debug:Disabled');
                    this.log = noop;
                    this.error = noop;
                }
            }

            return isDebugging;
        },
        /**
         * Logs data to the console. Pass any number of parameters to be logged. Log output is not processed if the framework is not running in debug mode.
         * @method log
         * @param {object} info* The objects to log.
         */
        log: noop,
        /**
         * Logs an error.
         * @method error
         * @param {string|Error} obj The error to report.
         */
        error: noop,
        /**
         * Asserts a condition by throwing an error if the condition fails.
         * @method assert
         * @param {boolean} condition The condition to check.
         * @param {string} message The message to report in the error if the condition check fails.
         */
        assert: function (condition, message) {
            if (!condition) {
                system.error(new Error(message || 'Assert:Failed'));
            }
        },
        /**
         * Creates a deferred object which can be used to create a promise. Optionally pass a function action to perform which will be passed an object used in resolving the promise.
         * @method defer
         * @param {function} [action] The action to defer. You will be passed the deferred object as a paramter.
         * @return {Deferred} The deferred object.
         */
        defer: function(action) {
            return $.Deferred(action);
        },
        /**
         * Creates a simple V4 UUID. This should not be used as a PK in your database. It can be used to generate internal, unique ids. For a more robust solution see [node-uuid](https://github.com/broofa/node-uuid).
         * @method guid
         * @return {string} The guid.
         */
        guid: function() {
            var d = new Date().getTime();
            return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                var r = (d + Math.random() * 16) % 16 | 0;
                d = Math.floor(d/16);
                return (c == 'x' ? r : (r & 0x7 | 0x8)).toString(16);
            });
        },
        /**
         * Uses require.js to obtain a module. This function returns a promise which resolves with the module instance. You can pass more than one module id to this function or an array of ids. If more than one or an array is passed, then the promise will resolve with an array of module instances.
         * @method acquire
         * @param {string|string[]} moduleId The id(s) of the modules to load.
         * @return {Promise} A promise for the loaded module(s).
         */
        acquire: function() {
            var modules,
                first = arguments[0],
                arrayRequest = false;

            if(system.isArray(first)){
                modules = first;
                arrayRequest = true;
            }else{
                modules = slice.call(arguments, 0);
            }

            return this.defer(function(dfd) {
                require(modules, function() {
                    var args = arguments;
                    setTimeout(function() {
                        if(args.length > 1 || arrayRequest){
                            dfd.resolve(slice.call(args, 0));
                        }else{
                            dfd.resolve(args[0]);
                        }
                    }, 1);
                }, function(err){
                    dfd.reject(err);
                });
            }).promise();
        },
        /**
         * Extends the first object with the properties of the following objects.
         * @method extend
         * @param {object} obj The target object to extend.
         * @param {object} extension* Uses to extend the target object.
         */
        extend: function(obj) {
            var rest = slice.call(arguments, 1);

            for (var i = 0; i < rest.length; i++) {
                var source = rest[i];

                if (source) {
                    for (var prop in source) {
                        obj[prop] = source[prop];
                    }
                }
            }

            return obj;
        },
        /**
         * Uses a setTimeout to wait the specified milliseconds.
         * @method wait
         * @param {number} milliseconds The number of milliseconds to wait.
         * @return {Promise}
         */
        wait: function(milliseconds) {
            return system.defer(function(dfd) {
                setTimeout(dfd.resolve, milliseconds);
            }).promise();
        }
    };

    /**
     * Gets all the owned keys of the specified object.
     * @method keys
     * @param {object} object The object whose owned keys should be returned.
     * @return {string[]} The keys.
     */
    system.keys = nativeKeys || function(obj) {
        if (obj !== Object(obj)) {
            throw new TypeError('Invalid object');
        }

        var keys = [];

        for (var key in obj) {
            if (hasOwnProperty.call(obj, key)) {
                keys[keys.length] = key;
            }
        }

        return keys;
    };

    /**
     * Determines if the specified object is an html element.
     * @method isElement
     * @param {object} object The object to check.
     * @return {boolean} True if matches the type, false otherwise.
     */
    system.isElement = function(obj) {
        return !!(obj && obj.nodeType === 1);
    };

    /**
     * Determines if the specified object is an array.
     * @method isArray
     * @param {object} object The object to check.
     * @return {boolean} True if matches the type, false otherwise.
     */
    system.isArray = nativeIsArray || function(obj) {
        return toString.call(obj) == '[object Array]';
    };

    /**
     * Determines if the specified object is...an object. ie. Not an array, string, etc.
     * @method isObject
     * @param {object} object The object to check.
     * @return {boolean} True if matches the type, false otherwise.
     */
    system.isObject = function(obj) {
        return obj === Object(obj);
    };

    /**
     * Determines if the specified object is a boolean.
     * @method isBoolean
     * @param {object} object The object to check.
     * @return {boolean} True if matches the type, false otherwise.
     */
    system.isBoolean = function(obj) {
        return typeof(obj) === "boolean";
    };

    /**
     * Determines if the specified object is a promise.
     * @method isPromise
     * @param {object} object The object to check.
     * @return {boolean} True if matches the type, false otherwise.
     */
    system.isPromise = function(obj) {
        return obj && system.isFunction(obj.then);
    };

    /**
     * Determines if the specified object is a function arguments object.
     * @method isArguments
     * @param {object} object The object to check.
     * @return {boolean} True if matches the type, false otherwise.
     */

    /**
     * Determines if the specified object is a function.
     * @method isFunction
     * @param {object} object The object to check.
     * @return {boolean} True if matches the type, false otherwise.
     */

    /**
     * Determines if the specified object is a string.
     * @method isString
     * @param {object} object The object to check.
     * @return {boolean} True if matches the type, false otherwise.
     */

    /**
     * Determines if the specified object is a number.
     * @method isNumber
     * @param {object} object The object to check.
     * @return {boolean} True if matches the type, false otherwise.
     */

    /**
     * Determines if the specified object is a date.
     * @method isDate
     * @param {object} object The object to check.
     * @return {boolean} True if matches the type, false otherwise.
     */

    /**
     * Determines if the specified object is a boolean.
     * @method isBoolean
     * @param {object} object The object to check.
     * @return {boolean} True if matches the type, false otherwise.
     */

    //isArguments, isFunction, isString, isNumber, isDate, isRegExp.
    var isChecks = ['Arguments', 'Function', 'String', 'Number', 'Date', 'RegExp'];

    function makeIsFunction(name) {
        var value = '[object ' + name + ']';
        system['is' + name] = function(obj) {
            return toString.call(obj) == value;
        };
    }

    for (var i = 0; i < isChecks.length; i++) {
        makeIsFunction(isChecks[i]);
    }

    return system;
});

/**
 * Durandal 2.1.0 Copyright (c) 2012 Blue Spire Consulting, Inc. All Rights Reserved.
 * Available via the MIT license.
 * see: http://durandaljs.com or https://github.com/BlueSpire/Durandal for details.
 */
/**
 * The viewEngine module provides information to the viewLocator module which is used to locate the view's source file. The viewEngine also transforms a view id into a view instance.
 * @module viewEngine
 * @requires system
 * @requires jquery
 */
define('durandal/viewEngine',['durandal/system', 'jquery'], function (system, $) {
    var parseMarkup;

    if ($.parseHTML) {
        parseMarkup = function (html) {
            return $.parseHTML(html);
        };
    } else {
        parseMarkup = function (html) {
            return $(html).get();
        };
    }

    /**
     * @class ViewEngineModule
     * @static
     */
    return {
        cache:{},
        /**
         * The file extension that view source files are expected to have.
         * @property {string} viewExtension
         * @default .html
         */
        viewExtension: '.html',
        /**
         * The name of the RequireJS loader plugin used by the viewLocator to obtain the view source. (Use requirejs to map the plugin's full path).
         * @property {string} viewPlugin
         * @default text
         */
        viewPlugin: 'text',
        /**
         * Parameters passed to the RequireJS loader plugin used by the viewLocator to obtain the view source.
         * @property {string} viewPluginParameters
         * @default The empty string by default.
         */
        viewPluginParameters: '',
        /**
         * Determines if the url is a url for a view, according to the view engine.
         * @method isViewUrl
         * @param {string} url The potential view url.
         * @return {boolean} True if the url is a view url, false otherwise.
         */
        isViewUrl: function (url) {
            return url.indexOf(this.viewExtension, url.length - this.viewExtension.length) !== -1;
        },
        /**
         * Converts a view url into a view id.
         * @method convertViewUrlToViewId
         * @param {string} url The url to convert.
         * @return {string} The view id.
         */
        convertViewUrlToViewId: function (url) {
            return url.substring(0, url.length - this.viewExtension.length);
        },
        /**
         * Converts a view id into a full RequireJS path.
         * @method convertViewIdToRequirePath
         * @param {string} viewId The view id to convert.
         * @return {string} The require path.
         */
        convertViewIdToRequirePath: function (viewId) {
            var plugin = this.viewPlugin ? this.viewPlugin + '!' : '';
            return plugin + viewId + this.viewExtension + this.viewPluginParameters;
        },
        /**
         * Parses the view engine recognized markup and returns DOM elements.
         * @method parseMarkup
         * @param {string} markup The markup to parse.
         * @return {DOMElement[]} The elements.
         */
        parseMarkup: parseMarkup,
        /**
         * Calls `parseMarkup` and then pipes the results through `ensureSingleElement`.
         * @method processMarkup
         * @param {string} markup The markup to process.
         * @return {DOMElement} The view.
         */
        processMarkup: function (markup) {
            var allElements = this.parseMarkup(markup);
            return this.ensureSingleElement(allElements);
        },
        /**
         * Converts an array of elements into a single element. White space and comments are removed. If a single element does not remain, then the elements are wrapped.
         * @method ensureSingleElement
         * @param {DOMElement[]} allElements The elements.
         * @return {DOMElement} A single element.
         */
        ensureSingleElement:function(allElements){
            if (allElements.length == 1) {
                return allElements[0];
            }

            var withoutCommentsOrEmptyText = [];

            for (var i = 0; i < allElements.length; i++) {
                var current = allElements[i];
                if (current.nodeType != 8) {
                    if (current.nodeType == 3) {
                        var result = /\S/.test(current.nodeValue);
                        if (!result) {
                            continue;
                        }
                    }

                    withoutCommentsOrEmptyText.push(current);
                }
            }

            if (withoutCommentsOrEmptyText.length > 1) {
                return $(withoutCommentsOrEmptyText).wrapAll('<div class="durandal-wrapper"></div>').parent().get(0);
            }

            return withoutCommentsOrEmptyText[0];
        },
        /**
         * Gets the view associated with the id from the cache of parsed views.
         * @method tryGetViewFromCache
         * @param {string} id The view id to lookup in the cache.
         * @return {DOMElement|null} The cached view or null if it's not in the cache.
         */
        tryGetViewFromCache:function(id) {
            return this.cache[id];
        },
        /**
         * Puts the view associated with the id into the cache of parsed views.
         * @method putViewInCache
         * @param {string} id The view id whose view should be cached.
         * @param {DOMElement} view The view to cache.
         */
        putViewInCache: function (id, view) {
            this.cache[id] = view;
        },
        /**
         * Creates the view associated with the view id.
         * @method createView
         * @param {string} viewId The view id whose view should be created.
         * @return {Promise} A promise of the view.
         */
        createView: function(viewId) {
            var that = this;
            var requirePath = this.convertViewIdToRequirePath(viewId);
            var existing = this.tryGetViewFromCache(requirePath);

            if (existing) {
                return system.defer(function(dfd) {
                    dfd.resolve(existing.cloneNode(true));
                }).promise();
            }

            return system.defer(function(dfd) {
                system.acquire(requirePath).then(function(markup) {
                    var element = that.processMarkup(markup);
                    element.setAttribute('data-view', viewId);
                    that.putViewInCache(requirePath, element);
                    dfd.resolve(element.cloneNode(true));
                }).fail(function(err) {
                    that.createFallbackView(viewId, requirePath, err).then(function(element) {
                        element.setAttribute('data-view', viewId);
                        that.cache[requirePath] = element;
                        dfd.resolve(element.cloneNode(true));
                    });
                });
            }).promise();
        },
        /**
         * Called when a view cannot be found to provide the opportunity to locate or generate a fallback view. Mainly used to ease development.
         * @method createFallbackView
         * @param {string} viewId The view id whose view should be created.
         * @param {string} requirePath The require path that was attempted.
         * @param {Error} requirePath The error that was returned from the attempt to locate the default view.
         * @return {Promise} A promise for the fallback view.
         */
        createFallbackView: function (viewId, requirePath, err) {
            var that = this,
                message = 'View Not Found. Searched for "' + viewId + '" via path "' + requirePath + '".';

            return system.defer(function(dfd) {
                dfd.resolve(that.processMarkup('<div class="durandal-view-404">' + message + '</div>'));
            }).promise();
        }
    };
});

/**
 * Durandal 2.1.0 Copyright (c) 2012 Blue Spire Consulting, Inc. All Rights Reserved.
 * Available via the MIT license.
 * see: http://durandaljs.com or https://github.com/BlueSpire/Durandal for details.
 */
/**
 * The viewLocator module collaborates with the viewEngine module to provide views (literally dom sub-trees) to other parts of the framework as needed. The primary consumer of the viewLocator is the composition module.
 * @module viewLocator
 * @requires system
 * @requires viewEngine
 */
define('durandal/viewLocator',['durandal/system', 'durandal/viewEngine'], function (system, viewEngine) {
    function findInElements(nodes, url) {
        for (var i = 0; i < nodes.length; i++) {
            var current = nodes[i];
            var existingUrl = current.getAttribute('data-view');
            if (existingUrl == url) {
                return current;
            }
        }
    }
    
    function escape(str) {
        return (str + '').replace(/([\\\.\+\*\?\[\^\]\$\(\)\{\}\=\!\<\>\|\:])/g, "\\$1");
    }

    /**
     * @class ViewLocatorModule
     * @static
     */
    return {
        /**
         * Allows you to set up a convention for mapping module folders to view folders. It is a convenience method that customizes `convertModuleIdToViewId` and `translateViewIdToArea` under the covers.
         * @method useConvention
         * @param {string} [modulesPath] A string to match in the path and replace with the viewsPath. If not specified, the match is 'viewmodels'.
         * @param {string} [viewsPath] The replacement for the modulesPath. If not specified, the replacement is 'views'.
         * @param {string} [areasPath] Partial views are mapped to the "views" folder if not specified. Use this parameter to change their location.
         */
        useConvention: function(modulesPath, viewsPath, areasPath) {
            modulesPath = modulesPath || 'viewmodels';
            viewsPath = viewsPath || 'views';
            areasPath = areasPath || viewsPath;

            var reg = new RegExp(escape(modulesPath), 'gi');

            this.convertModuleIdToViewId = function (moduleId) {
                return moduleId.replace(reg, viewsPath);
            };

            this.translateViewIdToArea = function (viewId, area) {
                if (!area || area == 'partial') {
                    return areasPath + '/' + viewId;
                }
                
                return areasPath + '/' + area + '/' + viewId;
            };
        },
        /**
         * Maps an object instance to a view instance.
         * @method locateViewForObject
         * @param {object} obj The object to locate the view for.
         * @param {string} [area] The area to translate the view to.
         * @param {DOMElement[]} [elementsToSearch] An existing set of elements to search first.
         * @return {Promise} A promise of the view.
         */
        locateViewForObject: function(obj, area, elementsToSearch) {
            var view;

            if (obj.getView) {
                view = obj.getView();
                if (view) {
                    return this.locateView(view, area, elementsToSearch);
                }
            }

            if (obj.viewUrl) {
                return this.locateView(obj.viewUrl, area, elementsToSearch);
            }

            var id = system.getModuleId(obj);
            if (id) {
                return this.locateView(this.convertModuleIdToViewId(id), area, elementsToSearch);
            }

            return this.locateView(this.determineFallbackViewId(obj), area, elementsToSearch);
        },
        /**
         * Converts a module id into a view id. By default the ids are the same.
         * @method convertModuleIdToViewId
         * @param {string} moduleId The module id.
         * @return {string} The view id.
         */
        convertModuleIdToViewId: function(moduleId) {
            return moduleId;
        },
        /**
         * If no view id can be determined, this function is called to genreate one. By default it attempts to determine the object's type and use that.
         * @method determineFallbackViewId
         * @param {object} obj The object to determine the fallback id for.
         * @return {string} The view id.
         */
        determineFallbackViewId: function (obj) {
            var funcNameRegex = /function (.{1,})\(/;
            var results = (funcNameRegex).exec((obj).constructor.toString());
            var typeName = (results && results.length > 1) ? results[1] : "";
            typeName = typeName.trim();
            return 'views/' + typeName;
        },
        /**
         * Takes a view id and translates it into a particular area. By default, no translation occurs.
         * @method translateViewIdToArea
         * @param {string} viewId The view id.
         * @param {string} area The area to translate the view to.
         * @return {string} The translated view id.
         */
        translateViewIdToArea: function (viewId, area) {
            return viewId;
        },
        /**
         * Locates the specified view.
         * @method locateView
         * @param {string|DOMElement} viewOrUrlOrId A view, view url or view id to locate.
         * @param {string} [area] The area to translate the view to.
         * @param {DOMElement[]} [elementsToSearch] An existing set of elements to search first.
         * @return {Promise} A promise of the view.
         */
        locateView: function(viewOrUrlOrId, area, elementsToSearch) {
            if (typeof viewOrUrlOrId === 'string') {
                var viewId;

                if (viewEngine.isViewUrl(viewOrUrlOrId)) {
                    viewId = viewEngine.convertViewUrlToViewId(viewOrUrlOrId);
                } else {
                    viewId = viewOrUrlOrId;
                }

                if (area) {
                    viewId = this.translateViewIdToArea(viewId, area);
                }

                if (elementsToSearch) {
                    var existing = findInElements(elementsToSearch, viewId);
                    if (existing) {
                        return system.defer(function(dfd) {
                            dfd.resolve(existing);
                        }).promise();
                    }
                }

                return viewEngine.createView(viewId);
            }

            return system.defer(function(dfd) {
                dfd.resolve(viewOrUrlOrId);
            }).promise();
        }
    };
});

/**
 * Durandal 2.1.0 Copyright (c) 2012 Blue Spire Consulting, Inc. All Rights Reserved.
 * Available via the MIT license.
 * see: http://durandaljs.com or https://github.com/BlueSpire/Durandal for details.
 */
/**
 * The binder joins an object instance and a DOM element tree by applying databinding and/or invoking binding lifecycle callbacks (binding and bindingComplete).
 * @module binder
 * @requires system
 * @requires knockout
 */
define('durandal/binder',['durandal/system', 'knockout'], function (system, ko) {
    var binder,
        insufficientInfoMessage = 'Insufficient Information to Bind',
        unexpectedViewMessage = 'Unexpected View Type',
        bindingInstructionKey = 'durandal-binding-instruction',
        koBindingContextKey = '__ko_bindingContext__';

    function normalizeBindingInstruction(result){
        if(result === undefined){
            return { applyBindings: true };
        }

        if(system.isBoolean(result)){
            return { applyBindings:result };
        }

        if(result.applyBindings === undefined){
            result.applyBindings = true;
        }

        return result;
    }

    function doBind(obj, view, bindingTarget, data){
        if (!view || !bindingTarget) {
            if (binder.throwOnErrors) {
                system.error(insufficientInfoMessage);
            } else {
                system.log(insufficientInfoMessage, view, data);
            }
            return;
        }

        if (!view.getAttribute) {
            if (binder.throwOnErrors) {
                system.error(unexpectedViewMessage);
            } else {
                system.log(unexpectedViewMessage, view, data);
            }
            return;
        }

        var viewName = view.getAttribute('data-view');

        try {
            var instruction;

            if (obj && obj.binding) {
                instruction = obj.binding(view);
            }

            instruction = normalizeBindingInstruction(instruction);
            binder.binding(data, view, instruction);

            if(instruction.applyBindings){
                system.log('Binding', viewName, data);
                ko.applyBindings(bindingTarget, view);
            }else if(obj){
                ko.utils.domData.set(view, koBindingContextKey, { $data:obj });
            }

            binder.bindingComplete(data, view, instruction);

            if (obj && obj.bindingComplete) {
                obj.bindingComplete(view);
            }

            ko.utils.domData.set(view, bindingInstructionKey, instruction);
            return instruction;
        } catch (e) {
            e.message = e.message + ';\nView: ' + viewName + ";\nModuleId: " + system.getModuleId(data);
            if (binder.throwOnErrors) {
                system.error(e);
            } else {
                system.log(e.message);
            }
        }
    }

    /**
     * @class BinderModule
     * @static
     */
    return binder = {
        /**
         * Called before every binding operation. Does nothing by default.
         * @method binding
         * @param {object} data The data that is about to be bound.
         * @param {DOMElement} view The view that is about to be bound.
         * @param {object} instruction The object that carries the binding instructions.
         */
        binding: system.noop,
        /**
         * Called after every binding operation. Does nothing by default.
         * @method bindingComplete
         * @param {object} data The data that has just been bound.
         * @param {DOMElement} view The view that has just been bound.
         * @param {object} instruction The object that carries the binding instructions.
         */
        bindingComplete: system.noop,
        /**
         * Indicates whether or not the binding system should throw errors or not.
         * @property {boolean} throwOnErrors
         * @default false The binding system will not throw errors by default. Instead it will log them.
         */
        throwOnErrors: false,
        /**
         * Gets the binding instruction that was associated with a view when it was bound.
         * @method getBindingInstruction
         * @param {DOMElement} view The view that was previously bound.
         * @return {object} The object that carries the binding instructions.
         */
        getBindingInstruction:function(view){
            return ko.utils.domData.get(view, bindingInstructionKey);
        },
        /**
         * Binds the view, preserving the existing binding context. Optionally, a new context can be created, parented to the previous context.
         * @method bindContext
         * @param {KnockoutBindingContext} bindingContext The current binding context.
         * @param {DOMElement} view The view to bind.
         * @param {object} [obj] The data to bind to, causing the creation of a child binding context if present.
         * @param {string} [dataAlias] An alias for $data if present.
         */
        bindContext: function(bindingContext, view, obj, dataAlias) {
            if (obj && bindingContext) {
                bindingContext = bindingContext.createChildContext(obj, typeof(dataAlias) === 'string' ? dataAlias : null);
            }

            return doBind(obj, view, bindingContext, obj || (bindingContext ? bindingContext.$data : null));
        },
        /**
         * Binds the view, preserving the existing binding context. Optionally, a new context can be created, parented to the previous context.
         * @method bind
         * @param {object} obj The data to bind to.
         * @param {DOMElement} view The view to bind.
         */
        bind: function(obj, view) {
            return doBind(obj, view, obj, obj);
        }
    };
});

/**
 * Durandal 2.1.0 Copyright (c) 2012 Blue Spire Consulting, Inc. All Rights Reserved.
 * Available via the MIT license.
 * see: http://durandaljs.com or https://github.com/BlueSpire/Durandal for details.
 */
/**
 * The activator module encapsulates all logic related to screen/component activation.
 * An activator is essentially an asynchronous state machine that understands a particular state transition protocol.
 * The protocol ensures that the following series of events always occur: `canDeactivate` (previous state), `canActivate` (new state), `deactivate` (previous state), `activate` (new state).
 * Each of the _can_ callbacks may return a boolean, affirmative value or promise for one of those. If either of the _can_ functions yields a false result, then activation halts.
 * @module activator
 * @requires system
 * @requires knockout
 */
define('durandal/activator',['durandal/system', 'knockout'], function (system, ko) {
    var activator;
    var defaultOptions = {
        canDeactivate:true
    };

    function ensureSettings(settings) {
        if (settings == undefined) {
            settings = {};
        }

        if (!system.isBoolean(settings.closeOnDeactivate)) {
            settings.closeOnDeactivate = activator.defaults.closeOnDeactivate;
        }

        if (!settings.beforeActivate) {
            settings.beforeActivate = activator.defaults.beforeActivate;
        }

        if (!settings.afterDeactivate) {
            settings.afterDeactivate = activator.defaults.afterDeactivate;
        }

        if(!settings.affirmations){
            settings.affirmations = activator.defaults.affirmations;
        }

        if (!settings.interpretResponse) {
            settings.interpretResponse = activator.defaults.interpretResponse;
        }

        if (!settings.areSameItem) {
            settings.areSameItem = activator.defaults.areSameItem;
        }

        if (!settings.findChildActivator) {
            settings.findChildActivator = activator.defaults.findChildActivator;
        }

        return settings;
    }

    function invoke(target, method, data) {
        if (system.isArray(data)) {
            return target[method].apply(target, data);
        }

        return target[method](data);
    }

    function deactivate(item, close, settings, dfd, setter) {
        if (item && item.deactivate) {
            system.log('Deactivating', item);

            var result;
            try {
                result = item.deactivate(close);
            } catch(error) {
                system.log('ERROR: ' + error.message, error);
                dfd.resolve(false);
                return;
            }

            if (result && result.then) {
                result.then(function() {
                    settings.afterDeactivate(item, close, setter);
                    dfd.resolve(true);
                }, function(reason) {
                    system.log(reason);
                    dfd.resolve(false);
                });
            } else {
                settings.afterDeactivate(item, close, setter);
                dfd.resolve(true);
            }
        } else {
            if (item) {
                settings.afterDeactivate(item, close, setter);
            }

            dfd.resolve(true);
        }
    }

    function activate(newItem, activeItem, callback, activationData) {
        var result;

        if(newItem && newItem.activate) {
            system.log('Activating', newItem);

            try {
                result = invoke(newItem, 'activate', activationData);
            } catch(error) {
                system.log('ERROR: ' + error.message, error);
                callback(false);
                return;
            }
        }

        if(result && result.then) {
            result.then(function() {
                activeItem(newItem);
                callback(true);
            }, function(reason) {
                system.log('ERROR: ' + reason.message, reason);
                callback(false);
            });
        } else {
            activeItem(newItem);
            callback(true);
        }
    }

    function canDeactivateItem(item, close, settings, options) {
        options = system.extend({}, defaultOptions, options);
        settings.lifecycleData = null;

        return system.defer(function (dfd) {
            function continueCanDeactivate() {
                if (item && item.canDeactivate && options.canDeactivate) {
                    var resultOrPromise;
                    try {
                        resultOrPromise = item.canDeactivate(close);
                    } catch (error) {
                        system.log('ERROR: ' + error.message, error);
                        dfd.resolve(false);
                        return;
                    }

                    if (resultOrPromise.then) {
                        resultOrPromise.then(function (result) {
                            settings.lifecycleData = result;
                            dfd.resolve(settings.interpretResponse(result));
                        }, function (reason) {
                            system.log('ERROR: ' + reason.message, reason);
                            dfd.resolve(false);
                        });
                    } else {
                        settings.lifecycleData = resultOrPromise;
                        dfd.resolve(settings.interpretResponse(resultOrPromise));
                    }
                } else {
                    dfd.resolve(true);
                }
            }

            var childActivator = settings.findChildActivator(item);
            if (childActivator) {
                childActivator.canDeactivate().then(function(result) {
                    if (result) {
                        continueCanDeactivate();
                    } else {
                        dfd.resolve(false);
                    }
                });
            } else {
                continueCanDeactivate();
            }
        }).promise();
    };

    function canActivateItem(newItem, activeItem, settings, activeData, newActivationData) {
        settings.lifecycleData = null;

        return system.defer(function (dfd) {
            if (settings.areSameItem(activeItem(), newItem, activeData, newActivationData)) {
                dfd.resolve(true);
                return;
            }

            if (newItem && newItem.canActivate) {
                var resultOrPromise;
                try {
                    resultOrPromise = invoke(newItem, 'canActivate', newActivationData);
                } catch (error) {
                    system.log('ERROR: ' + error.message, error);
                    dfd.resolve(false);
                    return;
                }

                if (resultOrPromise.then) {
                    resultOrPromise.then(function(result) {
                        settings.lifecycleData = result;
                        dfd.resolve(settings.interpretResponse(result));
                    }, function(reason) {
                        system.log('ERROR: ' + reason.message, reason);
                        dfd.resolve(false);
                    });
                } else {
                    settings.lifecycleData = resultOrPromise;
                    dfd.resolve(settings.interpretResponse(resultOrPromise));
                }
            } else {
                dfd.resolve(true);
            }
        }).promise();
    };

    /**
     * An activator is a read/write computed observable that enforces the activation lifecycle whenever changing values.
     * @class Activator
     */
    function createActivator(initialActiveItem, settings) {
        var activeItem = ko.observable(null);
        var activeData;

        settings = ensureSettings(settings);

        var computed = ko.computed({
            read: function () {
                return activeItem();
            },
            write: function (newValue) {
                computed.viaSetter = true;
                computed.activateItem(newValue);
            }
        });

        computed.__activator__ = true;

        /**
         * The settings for this activator.
         * @property {ActivatorSettings} settings
         */
        computed.settings = settings;
        settings.activator = computed;

        /**
         * An observable which indicates whether or not the activator is currently in the process of activating an instance.
         * @method isActivating
         * @return {boolean}
         */
        computed.isActivating = ko.observable(false);

        computed.forceActiveItem = function (item) {
            activeItem(item);
        };

        /**
         * Determines whether or not the specified item can be deactivated.
         * @method canDeactivateItem
         * @param {object} item The item to check.
         * @param {boolean} close Whether or not to check if close is possible.
         * @param {object} options Options for controlling the activation process.
         * @return {promise}
         */
        computed.canDeactivateItem = function (item, close, options) {
            return canDeactivateItem(item, close, settings, options);
        };

        /**
         * Deactivates the specified item.
         * @method deactivateItem
         * @param {object} item The item to deactivate.
         * @param {boolean} close Whether or not to close the item.
         * @return {promise}
         */
        computed.deactivateItem = function (item, close) {
            return system.defer(function(dfd) {
                computed.canDeactivateItem(item, close).then(function(canDeactivate) {
                    if (canDeactivate) {
                        deactivate(item, close, settings, dfd, activeItem);
                    } else {
                        computed.notifySubscribers();
                        dfd.resolve(false);
                    }
                });
            }).promise();
        };

        /**
         * Determines whether or not the specified item can be activated.
         * @method canActivateItem
         * @param {object} item The item to check.
         * @param {object} activationData Data associated with the activation.
         * @return {promise}
         */
        computed.canActivateItem = function (newItem, activationData) {
            return canActivateItem(newItem, activeItem, settings, activeData, activationData);
        };

        /**
         * Activates the specified item.
         * @method activateItem
         * @param {object} newItem The item to activate.
         * @param {object} newActivationData Data associated with the activation.
         * @param {object} options Options for controlling the activation process.
         * @return {promise}
         */
        computed.activateItem = function (newItem, newActivationData, options) {
            var viaSetter = computed.viaSetter;
            computed.viaSetter = false;

            return system.defer(function (dfd) {
                if (computed.isActivating()) {
                    dfd.resolve(false);
                    return;
                }

                computed.isActivating(true);

                var currentItem = activeItem();
                if (settings.areSameItem(currentItem, newItem, activeData, newActivationData)) {
                    computed.isActivating(false);
                    dfd.resolve(true);
                    return;
                }

                computed.canDeactivateItem(currentItem, settings.closeOnDeactivate, options).then(function (canDeactivate) {
                    if (canDeactivate) {
                        computed.canActivateItem(newItem, newActivationData).then(function (canActivate) {
                            if (canActivate) {
                                system.defer(function (dfd2) {
                                    deactivate(currentItem, settings.closeOnDeactivate, settings, dfd2);
                                }).promise().then(function () {
                                        newItem = settings.beforeActivate(newItem, newActivationData);
                                        activate(newItem, activeItem, function (result) {
                                            activeData = newActivationData;
                                            computed.isActivating(false);
                                            dfd.resolve(result);
                                        }, newActivationData);
                                    });
                            } else {
                                if (viaSetter) {
                                    computed.notifySubscribers();
                                }

                                computed.isActivating(false);
                                dfd.resolve(false);
                            }
                        });
                    } else {
                        if (viaSetter) {
                            computed.notifySubscribers();
                        }

                        computed.isActivating(false);
                        dfd.resolve(false);
                    }
                });
            }).promise();
        };

        /**
         * Determines whether or not the activator, in its current state, can be activated.
         * @method canActivate
         * @return {promise}
         */
        computed.canActivate = function () {
            var toCheck;

            if (initialActiveItem) {
                toCheck = initialActiveItem;
                initialActiveItem = false;
            } else {
                toCheck = computed();
            }

            return computed.canActivateItem(toCheck);
        };

        /**
         * Activates the activator, in its current state.
         * @method activate
         * @return {promise}
         */
        computed.activate = function () {
            var toActivate;

            if (initialActiveItem) {
                toActivate = initialActiveItem;
                initialActiveItem = false;
            } else {
                toActivate = computed();
            }

            return computed.activateItem(toActivate);
        };

        /**
         * Determines whether or not the activator, in its current state, can be deactivated.
         * @method canDeactivate
         * @return {promise}
         */
        computed.canDeactivate = function (close) {
            return computed.canDeactivateItem(computed(), close);
        };

        /**
         * Deactivates the activator, in its current state.
         * @method deactivate
         * @return {promise}
         */
        computed.deactivate = function (close) {
            return computed.deactivateItem(computed(), close);
        };

        computed.includeIn = function (includeIn) {
            includeIn.canActivate = function () {
                return computed.canActivate();
            };

            includeIn.activate = function () {
                return computed.activate();
            };

            includeIn.canDeactivate = function (close) {
                return computed.canDeactivate(close);
            };

            includeIn.deactivate = function (close) {
                return computed.deactivate(close);
            };
        };

        if (settings.includeIn) {
            computed.includeIn(settings.includeIn);
        } else if (initialActiveItem) {
            computed.activate();
        }

        computed.forItems = function (items) {
            settings.closeOnDeactivate = false;

            settings.determineNextItemToActivate = function (list, lastIndex) {
                var toRemoveAt = lastIndex - 1;

                if (toRemoveAt == -1 && list.length > 1) {
                    return list[1];
                }

                if (toRemoveAt > -1 && toRemoveAt < list.length - 1) {
                    return list[toRemoveAt];
                }

                return null;
            };

            settings.beforeActivate = function (newItem) {
                var currentItem = computed();

                if (!newItem) {
                    newItem = settings.determineNextItemToActivate(items, currentItem ? items.indexOf(currentItem) : 0);
                } else {
                    var index = items.indexOf(newItem);

                    if (index == -1) {
                        items.push(newItem);
                    } else {
                        newItem = items()[index];
                    }
                }

                return newItem;
            };

            settings.afterDeactivate = function (oldItem, close) {
                if (close) {
                    items.remove(oldItem);
                }
            };

            var originalCanDeactivate = computed.canDeactivate;
            computed.canDeactivate = function (close) {
                if (close) {
                    return system.defer(function (dfd) {
                        var list = items();
                        var results = [];

                        function finish() {
                            for (var j = 0; j < results.length; j++) {
                                if (!results[j]) {
                                    dfd.resolve(false);
                                    return;
                                }
                            }

                            dfd.resolve(true);
                        }

                        for (var i = 0; i < list.length; i++) {
                            computed.canDeactivateItem(list[i], close).then(function (result) {
                                results.push(result);
                                if (results.length == list.length) {
                                    finish();
                                }
                            });
                        }
                    }).promise();
                } else {
                    return originalCanDeactivate();
                }
            };

            var originalDeactivate = computed.deactivate;
            computed.deactivate = function (close) {
                if (close) {
                    return system.defer(function (dfd) {
                        var list = items();
                        var results = 0;
                        var listLength = list.length;

                        function doDeactivate(item) {
                            setTimeout(function () {
                                computed.deactivateItem(item, close).then(function () {
                                    results++;
                                    items.remove(item);
                                    if (results == listLength) {
                                        dfd.resolve();
                                    }
                                });
                            }, 1);
                        }

                        for (var i = 0; i < listLength; i++) {
                            doDeactivate(list[i]);
                        }
                    }).promise();
                } else {
                    return originalDeactivate();
                }
            };

            return computed;
        };

        return computed;
    }

    /**
     * @class ActivatorSettings
     * @static
     */
    var activatorSettings = {
        /**
         * The default value passed to an object's deactivate function as its close parameter.
         * @property {boolean} closeOnDeactivate
         * @default true
         */
        closeOnDeactivate: true,
        /**
         * Lower-cased words which represent a truthy value.
         * @property {string[]} affirmations
         * @default ['yes', 'ok', 'true']
         */
        affirmations: ['yes', 'ok', 'true'],
        /**
         * Interprets the response of a `canActivate` or `canDeactivate` call using the known affirmative values in the `affirmations` array.
         * @method interpretResponse
         * @param {object} value
         * @return {boolean}
         */
        interpretResponse: function(value) {
            if(system.isObject(value)) {
                value = value.can || false;
            }

            if(system.isString(value)) {
                return ko.utils.arrayIndexOf(this.affirmations, value.toLowerCase()) !== -1;
            }

            return value;
        },
        /**
         * Determines whether or not the current item and the new item are the same.
         * @method areSameItem
         * @param {object} currentItem
         * @param {object} newItem
         * @param {object} currentActivationData
         * @param {object} newActivationData
         * @return {boolean}
         */
        areSameItem: function(currentItem, newItem, currentActivationData, newActivationData) {
            return currentItem == newItem;
        },
        /**
         * Called immediately before the new item is activated.
         * @method beforeActivate
         * @param {object} newItem
         */
        beforeActivate: function(newItem) {
            return newItem;
        },
        /**
         * Called immediately after the old item is deactivated.
         * @method afterDeactivate
         * @param {object} oldItem The previous item.
         * @param {boolean} close Whether or not the previous item was closed.
         * @param {function} setter The activate item setter function.
         */
        afterDeactivate: function(oldItem, close, setter) {
            if(close && setter) {
                setter(null);
            }
        },
        findChildActivator: function(item){
            return null;
        }
    };

    /**
     * @class ActivatorModule
     * @static
     */
    activator = {
        /**
         * The default settings used by activators.
         * @property {ActivatorSettings} defaults
         */
        defaults: activatorSettings,
        /**
         * Creates a new activator.
         * @method create
         * @param {object} [initialActiveItem] The item which should be immediately activated upon creation of the ativator.
         * @param {ActivatorSettings} [settings] Per activator overrides of the default activator settings.
         * @return {Activator} The created activator.
         */
        create: createActivator,
        /**
         * Determines whether or not the provided object is an activator or not.
         * @method isActivator
         * @param {object} object Any object you wish to verify as an activator or not.
         * @return {boolean} True if the object is an activator; false otherwise.
         */
        isActivator:function(object){
            return object && object.__activator__;
        }
    };

    return activator;
});

/**
 * Durandal 2.1.0 Copyright (c) 2012 Blue Spire Consulting, Inc. All Rights Reserved.
 * Available via the MIT license.
 * see: http://durandaljs.com or https://github.com/BlueSpire/Durandal for details.
 */
/**
 * The composition module encapsulates all functionality related to visual composition.
 * @module composition
 * @requires system
 * @requires viewLocator
 * @requires binder
 * @requires viewEngine
 * @requires activator
 * @requires jquery
 * @requires knockout
 */
define('durandal/composition',['durandal/system', 'durandal/viewLocator', 'durandal/binder', 'durandal/viewEngine', 'durandal/activator', 'jquery', 'knockout'], function (system, viewLocator, binder, viewEngine, activator, $, ko) {
    var dummyModel = {},
        activeViewAttributeName = 'data-active-view',
        composition,
        compositionCompleteCallbacks = [],
        compositionCount = 0,
        compositionDataKey = 'durandal-composition-data',
        partAttributeName = 'data-part',
        bindableSettings = ['model', 'view', 'transition', 'area', 'strategy', 'activationData', 'onError'],
        visibilityKey = "durandal-visibility-data",
        composeBindings = ['compose:'];
    
    function onError(context, error, element) {
        try {
            if (context.onError) {
                try {
                    context.onError(error, element);
                } catch (e) {
                    system.error(e);
                }
            } else {
                system.error(error);
            }
        } finally {
            endComposition(context, element, true);
        }
    }

    function getHostState(parent) {
        var elements = [];
        var state = {
            childElements: elements,
            activeView: null
        };

        var child = ko.virtualElements.firstChild(parent);

        while (child) {
            if (child.nodeType == 1) {
                elements.push(child);
                if (child.getAttribute(activeViewAttributeName)) {
                    state.activeView = child;
                }
            }

            child = ko.virtualElements.nextSibling(child);
        }

        if(!state.activeView){
            state.activeView = elements[0];
        }

        return state;
    }

    function endComposition(context, element, error) {
        compositionCount--;

        if(compositionCount === 0) {
            var callBacks = compositionCompleteCallbacks;
            compositionCompleteCallbacks = [];
            
            if (!error) {
                setTimeout(function () {
                    var i = callBacks.length;

                    while (i--) {
                        try {
                            callBacks[i]();
                        } catch (e) {
                            onError(context, e, element);
                        }
                    }
                }, 1);
            }
        }

        cleanUp(context);
    }

    function cleanUp(context){
        delete context.activeView;
        delete context.viewElements;
    }

    function tryActivate(context, successCallback, skipActivation, element) {
        if(skipActivation){
            successCallback();
        } else if (context.activate && context.model && context.model.activate) {
            var result;

            try{
                if(system.isArray(context.activationData)) {
                    result = context.model.activate.apply(context.model, context.activationData);
                } else {
                    result = context.model.activate(context.activationData);
                }

                if(result && result.then) {
                    result.then(successCallback, function(reason) {
                        onError(context, reason, element);
                        successCallback();
                    });
                } else if(result || result === undefined) {
                    successCallback();
                } else {
                    endComposition(context, element);
                }
            }
            catch(e){
                onError(context, e, element);
            }
        } else {
            successCallback();
        }
    }

    function triggerAttach(context, element) {
        var context = this;

        if (context.activeView) {
            context.activeView.removeAttribute(activeViewAttributeName);
        }

        if (context.child) {
            try{
                if (context.model && context.model.attached) {
                    if (context.composingNewView || context.alwaysTriggerAttach) {
                        context.model.attached(context.child, context.parent, context);
                    }
                }

                if (context.attached) {
                    context.attached(context.child, context.parent, context);
                }

                context.child.setAttribute(activeViewAttributeName, true);

                if (context.composingNewView && context.model && context.model.detached) {
                    ko.utils.domNodeDisposal.addDisposeCallback(context.child, function () {
                        try{
                            context.model.detached(context.child, context.parent, context);
                        }catch(e2){
                            onError(context, e2, element);
                        }
                    });
                }
            }catch(e){
                onError(context, e, element);
            }
        }

        context.triggerAttach = system.noop;
    }

    function shouldTransition(context) {
        if (system.isString(context.transition)) {
            if (context.activeView) {
                if (context.activeView == context.child) {
                    return false;
                }

                if (!context.child) {
                    return true;
                }

                if (context.skipTransitionOnSameViewId) {
                    var currentViewId = context.activeView.getAttribute('data-view');
                    var newViewId = context.child.getAttribute('data-view');
                    return currentViewId != newViewId;
                }
            }

            return true;
        }

        return false;
    }

    function cloneNodes(nodesArray) {
        for (var i = 0, j = nodesArray.length, newNodesArray = []; i < j; i++) {
            var clonedNode = nodesArray[i].cloneNode(true);
            newNodesArray.push(clonedNode);
        }
        return newNodesArray;
    }

    function replaceParts(context){
        var parts = cloneNodes(context.parts);
        var replacementParts = composition.getParts(parts);
        var standardParts = composition.getParts(context.child);

        for (var partId in replacementParts) {
            var toReplace = standardParts[partId];
            if (!toReplace) {
                toReplace = $('[data-part="' + partId + '"]', context.child).get(0);
                if (!toReplace) {
                    system.log('Could not find part to override: ' + partId);
                    continue;
                }
            }

            toReplace.parentNode.replaceChild(replacementParts[partId], toReplace);
        }
    }

    function removePreviousView(context){
        var children = ko.virtualElements.childNodes(context.parent), i, len;

        if(!system.isArray(children)){
            var arrayChildren = [];
            for(i = 0, len = children.length; i < len; i++){
                arrayChildren[i] = children[i];
            }
            children = arrayChildren;
        }

        for(i = 1,len = children.length; i < len; i++){
            ko.removeNode(children[i]);
        }
    }

    function hide(view) {
        ko.utils.domData.set(view, visibilityKey, view.style.display);
        view.style.display = 'none';
    }

    function show(view) {
        var displayStyle = ko.utils.domData.get(view, visibilityKey);
        view.style.display = displayStyle === 'none' ? 'block' : displayStyle;
    }

    function hasComposition(element){
        var dataBind = element.getAttribute('data-bind');
        if(!dataBind){
            return false;
        }

        for(var i = 0, length = composeBindings.length; i < length; i++){
            if(dataBind.indexOf(composeBindings[i]) > -1){
                return true;
            }
        }

        return false;
    }

    /**
     * @class CompositionTransaction
     * @static
     */
    var compositionTransaction = {
        /**
         * Registers a callback which will be invoked when the current composition transaction has completed. The transaction includes all parent and children compositions.
         * @method complete
         * @param {function} callback The callback to be invoked when composition is complete.
         */
        complete: function (callback) {
            compositionCompleteCallbacks.push(callback);
        }
    };

    /**
     * @class CompositionModule
     * @static
     */
    composition = {
        /**
         * An array of all the binding handler names (includeing :) that trigger a composition.
         * @property {string} composeBindings
         * @default ['compose:']
         */
        composeBindings:composeBindings,
        /**
         * Converts a transition name to its moduleId.
         * @method convertTransitionToModuleId
         * @param {string} name The name of the transtion.
         * @return {string} The moduleId.
         */
        convertTransitionToModuleId: function (name) {
            return 'transitions/' + name;
        },
        /**
         * The name of the transition to use in all compositions.
         * @property {string} defaultTransitionName
         * @default null
         */
        defaultTransitionName: null,
        /**
         * Represents the currently executing composition transaction.
         * @property {CompositionTransaction} current
         */
        current: compositionTransaction,
        /**
         * Registers a binding handler that will be invoked when the current composition transaction is complete.
         * @method addBindingHandler
         * @param {string} name The name of the binding handler.
         * @param {object} [config] The binding handler instance. If none is provided, the name will be used to look up an existing handler which will then be converted to a composition handler.
         * @param {function} [initOptionsFactory] If the registered binding needs to return options from its init call back to knockout, this function will server as a factory for those options. It will receive the same parameters that the init function does.
         */
        addBindingHandler:function(name, config, initOptionsFactory){
            var key,
                dataKey = 'composition-handler-' + name,
                handler;

            config = config || ko.bindingHandlers[name];
            initOptionsFactory = initOptionsFactory || function(){ return undefined;  };

            handler = ko.bindingHandlers[name] = {
                init: function(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
                    if(compositionCount > 0){
                        var data = {
                            trigger:ko.observable(null)
                        };

                        composition.current.complete(function(){
                            if(config.init){
                                config.init(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext);
                            }

                            if(config.update){
                                ko.utils.domData.set(element, dataKey, config);
                                data.trigger('trigger');
                            }
                        });

                        ko.utils.domData.set(element, dataKey, data);
                    }else{
                        ko.utils.domData.set(element, dataKey, config);

                        if(config.init){
                            config.init(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext);
                        }
                    }

                    return initOptionsFactory(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext);
                },
                update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
                    var data = ko.utils.domData.get(element, dataKey);

                    if(data.update){
                        return data.update(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext);
                    }

                    if(data.trigger){
                        data.trigger();
                    }
                }
            };

            for (key in config) {
                if (key !== "init" && key !== "update") {
                    handler[key] = config[key];
                }
            }
        },
        /**
         * Gets an object keyed with all the elements that are replacable parts, found within the supplied elements. The key will be the part name and the value will be the element itself.
         * @method getParts
         * @param {DOMElement\DOMElement[]} elements The element(s) to search for parts.
         * @return {object} An object keyed by part.
         */
        getParts: function(elements, parts) {
            parts = parts || {};

            if (!elements) {
                return parts;
            }

            if (elements.length === undefined) {
                elements = [elements];
            }

            for (var i = 0, length = elements.length; i < length; i++) {
                var element = elements[i],
                    id;

                if (element.getAttribute) {
                    id = element.getAttribute(partAttributeName);
                    if (id) {
                        parts[id] = element;
                    }

                    if (element.hasChildNodes() && !hasComposition(element)) {
                        composition.getParts(element.childNodes, parts);
                    }
                }
            }

            return parts;
        },
        cloneNodes:cloneNodes,
        finalize: function (context, element) {
            if(context.transition === undefined) {
                context.transition = this.defaultTransitionName;
            }

            if(!context.child && !context.activeView){
                if (!context.cacheViews) {
                    ko.virtualElements.emptyNode(context.parent);
                }

                context.triggerAttach(context, element);
                endComposition(context, element);
            } else if (shouldTransition(context)) {
                var transitionModuleId = this.convertTransitionToModuleId(context.transition);

                system.acquire(transitionModuleId).then(function (transition) {
                    context.transition = transition;

                    transition(context).then(function () {
                        if (!context.cacheViews) {
                            if(!context.child){
                                ko.virtualElements.emptyNode(context.parent);
                            }else{
                                removePreviousView(context);
                            }
                        }else if(context.activeView){
                            var instruction = binder.getBindingInstruction(context.activeView);
                            if(instruction && instruction.cacheViews != undefined && !instruction.cacheViews){
                                ko.removeNode(context.activeView);
                            }else{
                                hide(context.activeView);
                            }
                        }

                        if (context.child) {
                            show(context.child);
                        }

                        context.triggerAttach(context, element);
                        endComposition(context, element);
                    });
                }).fail(function(err){
                    onError(context, 'Failed to load transition (' + transitionModuleId + '). Details: ' + err.message, element);
                });
            } else {
                if (context.child != context.activeView) {
                    if (context.cacheViews && context.activeView) {
                        var instruction = binder.getBindingInstruction(context.activeView);
                        if(!instruction || (instruction.cacheViews != undefined && !instruction.cacheViews)){
                            ko.removeNode(context.activeView);
                        }else{
                            hide(context.activeView);
                        }
                    }

                    if (!context.child) {
                        if (!context.cacheViews) {
                            ko.virtualElements.emptyNode(context.parent);
                        }
                    } else {
                        if (!context.cacheViews) {
                            removePreviousView(context);
                        }

                        show(context.child);
                    }
                }

                context.triggerAttach(context, element);
                endComposition(context, element);
            }
        },
        bindAndShow: function (child, element, context, skipActivation) {
            context.child = child;
            context.parent.__composition_context = context;

            if (context.cacheViews) {
                context.composingNewView = (ko.utils.arrayIndexOf(context.viewElements, child) == -1);
            } else {
                context.composingNewView = true;
            }

            tryActivate(context, function () {
                if (context.parent.__composition_context == context) {
                    delete context.parent.__composition_context;

                    if (context.binding) {
                        context.binding(context.child, context.parent, context);
                    }

                    if (context.preserveContext && context.bindingContext) {
                        if (context.composingNewView) {
                            if(context.parts){
                                replaceParts(context);
                            }

                            hide(child);
                            ko.virtualElements.prepend(context.parent, child);

                        binder.bindContext(context.bindingContext, child, context.model, context.as);
                        }
                    } else if (child) {
                        var modelToBind = context.model || dummyModel;
                        var currentModel = ko.dataFor(child);

                        if (currentModel != modelToBind) {
                            if (!context.composingNewView) {
                                ko.removeNode(child);
                                viewEngine.createView(child.getAttribute('data-view')).then(function(recreatedView) {
                                    composition.bindAndShow(recreatedView, element, context, true);
                                });
                                return;
                            }

                            if(context.parts){
                                replaceParts(context);
                            }

                            hide(child);
                            ko.virtualElements.prepend(context.parent, child);

                            binder.bind(modelToBind, child);
                        }
                    }

                    composition.finalize(context, element);
                } else {
                    endComposition(context, element);
                }
            }, skipActivation, element);
        },
        /**
         * Eecutes the default view location strategy.
         * @method defaultStrategy
         * @param {object} context The composition context containing the model and possibly existing viewElements.
         * @return {promise} A promise for the view.
         */
        defaultStrategy: function (context) {
            return viewLocator.locateViewForObject(context.model, context.area, context.viewElements);
        },
        getSettings: function (valueAccessor, element) {
            var value = valueAccessor(),
                settings = ko.utils.unwrapObservable(value) || {},
                activatorPresent = activator.isActivator(value),
                moduleId;

            if (system.isString(settings)) {
                if (viewEngine.isViewUrl(settings)) {
                    settings = {
                        view: settings
                    };
                } else {
                    settings = {
                        model: settings,
                        activate: !activatorPresent
                    };
                }

                return settings;
            }

            moduleId = system.getModuleId(settings);
            if (moduleId) {
                settings = {
                    model: settings,
                    activate: !activatorPresent
                };

                return settings;
            }

            if(!activatorPresent && settings.model) {
                activatorPresent = activator.isActivator(settings.model);
            }

            for (var attrName in settings) {
                if (ko.utils.arrayIndexOf(bindableSettings, attrName) != -1) {
                    settings[attrName] = ko.utils.unwrapObservable(settings[attrName]);
                } else {
                    settings[attrName] = settings[attrName];
                }
            }

            if (activatorPresent) {
                settings.activate = false;
            } else if (settings.activate === undefined) {
                settings.activate = true;
            }

            return settings;
        },
        executeStrategy: function (context, element) {
            context.strategy(context).then(function (child) {
                composition.bindAndShow(child, element, context);
            });
        },
        inject: function (context, element) {
            if (!context.model) {
                this.bindAndShow(null, element, context);
                return;
            }

            if (context.view) {
                viewLocator.locateView(context.view, context.area, context.viewElements).then(function (child) {
                    composition.bindAndShow(child, element, context);
                });
                return;
            }

            if (!context.strategy) {
                context.strategy = this.defaultStrategy;
            }

            if (system.isString(context.strategy)) {
                system.acquire(context.strategy).then(function (strategy) {
                    context.strategy = strategy;
                    composition.executeStrategy(context, element);
                }).fail(function (err) {
                    onError(context, 'Failed to load view strategy (' + context.strategy + '). Details: ' + err.message, element);
                });
            } else {
                this.executeStrategy(context, element);
            }
        },
        /**
         * Initiates a composition.
         * @method compose
         * @param {DOMElement} element The DOMElement or knockout virtual element that serves as the parent for the composition.
         * @param {object} settings The composition settings.
         * @param {object} [bindingContext] The current binding context.
         */
        compose: function (element, settings, bindingContext, fromBinding) {
            compositionCount++;

            if(!fromBinding){
                settings = composition.getSettings(function() { return settings; }, element);
            }

            if (settings.compositionComplete) {
                compositionCompleteCallbacks.push(function () {
                    settings.compositionComplete(settings.child, settings.parent, settings);
                });
            }

            compositionCompleteCallbacks.push(function () {
                if(settings.composingNewView && settings.model && settings.model.compositionComplete){
                    settings.model.compositionComplete(settings.child, settings.parent, settings);
                }
            });

            var hostState = getHostState(element);

            settings.activeView = hostState.activeView;
            settings.parent = element;
            settings.triggerAttach = triggerAttach;
            settings.bindingContext = bindingContext;

            if (settings.cacheViews && !settings.viewElements) {
                settings.viewElements = hostState.childElements;
            }

            if (!settings.model) {
                if (!settings.view) {
                    this.bindAndShow(null, element, settings);
                } else {
                    settings.area = settings.area || 'partial';
                    settings.preserveContext = true;

                    viewLocator.locateView(settings.view, settings.area, settings.viewElements).then(function (child) {
                        composition.bindAndShow(child, element, settings);
                    });
                }
            } else if (system.isString(settings.model)) {
                system.acquire(settings.model).then(function (module) {
                    settings.model = system.resolveObject(module);
                    composition.inject(settings, element);
                }).fail(function (err) {
                    onError(settings, 'Failed to load composed module (' + settings.model + '). Details: ' + err.message, element);
                });
            } else {
                composition.inject(settings, element);
            }
        }
    };

    ko.bindingHandlers.compose = {
        init: function() {
            return { controlsDescendantBindings: true };
        },
        update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            var settings = composition.getSettings(valueAccessor, element);
            if(settings.mode){
                var data = ko.utils.domData.get(element, compositionDataKey);
                if(!data){
                    var childNodes = ko.virtualElements.childNodes(element);
                    data = {};

                    if(settings.mode === 'inline'){
                        data.view = viewEngine.ensureSingleElement(childNodes);
                    }else if(settings.mode === 'templated'){
                        data.parts = cloneNodes(childNodes);
                    }

                    ko.virtualElements.emptyNode(element);
                    ko.utils.domData.set(element, compositionDataKey, data);
                }

                if(settings.mode === 'inline'){
                    settings.view = data.view.cloneNode(true);
                }else if(settings.mode === 'templated'){
                    settings.parts = data.parts;
                }

                settings.preserveContext = true;
            }

            composition.compose(element, settings, bindingContext, true);
        }
    };

    ko.virtualElements.allowedBindings.compose = true;

    return composition;
});

/**
 * Durandal 2.1.0 Copyright (c) 2012 Blue Spire Consulting, Inc. All Rights Reserved.
 * Available via the MIT license.
 * see: http://durandaljs.com or https://github.com/BlueSpire/Durandal for details.
 */
/**
 * Durandal events originate from backbone.js but also combine some ideas from signals.js as well as some additional improvements.
 * Events can be installed into any object and are installed into the `app` module by default for convenient app-wide eventing.
 * @module events
 * @requires system
 */
define('durandal/events',['durandal/system'], function (system) {
    var eventSplitter = /\s+/;
    var Events = function() { };

    /**
     * Represents an event subscription.
     * @class Subscription
     */
    var Subscription = function(owner, events) {
        this.owner = owner;
        this.events = events;
    };

    /**
     * Attaches a callback to the event subscription.
     * @method then
     * @param {function} callback The callback function to invoke when the event is triggered.
     * @param {object} [context] An object to use as `this` when invoking the `callback`.
     * @chainable
     */
    Subscription.prototype.then = function (callback, context) {
        this.callback = callback || this.callback;
        this.context = context || this.context;
        
        if (!this.callback) {
            return this;
        }

        this.owner.on(this.events, this.callback, this.context);
        return this;
    };

    /**
     * Attaches a callback to the event subscription.
     * @method on
     * @param {function} [callback] The callback function to invoke when the event is triggered. If `callback` is not provided, the previous callback will be re-activated.
     * @param {object} [context] An object to use as `this` when invoking the `callback`.
     * @chainable
     */
    Subscription.prototype.on = Subscription.prototype.then;

    /**
     * Cancels the subscription.
     * @method off
     * @chainable
     */
    Subscription.prototype.off = function () {
        this.owner.off(this.events, this.callback, this.context);
        return this;
    };

    /**
     * Creates an object with eventing capabilities.
     * @class Events
     */

    /**
     * Creates a subscription or registers a callback for the specified event.
     * @method on
     * @param {string} events One or more events, separated by white space.
     * @param {function} [callback] The callback function to invoke when the event is triggered. If `callback` is not provided, a subscription instance is returned.
     * @param {object} [context] An object to use as `this` when invoking the `callback`.
     * @return {Subscription|Events} A subscription is returned if no callback is supplied, otherwise the events object is returned for chaining.
     */
    Events.prototype.on = function(events, callback, context) {
        var calls, event, list;

        if (!callback) {
            return new Subscription(this, events);
        } else {
            calls = this.callbacks || (this.callbacks = {});
            events = events.split(eventSplitter);

            while (event = events.shift()) {
                list = calls[event] || (calls[event] = []);
                list.push(callback, context);
            }

            return this;
        }
    };

    /**
     * Removes the callbacks for the specified events.
     * @method off
     * @param {string} [events] One or more events, separated by white space to turn off. If no events are specified, then the callbacks will be removed.
     * @param {function} [callback] The callback function to remove. If `callback` is not provided, all callbacks for the specified events will be removed.
     * @param {object} [context] The object that was used as `this`. Callbacks with this context will be removed.
     * @chainable
     */
    Events.prototype.off = function(events, callback, context) {
        var event, calls, list, i;

        // No events
        if (!(calls = this.callbacks)) {
            return this;
        }

        //removing all
        if (!(events || callback || context)) {
            delete this.callbacks;
            return this;
        }

        events = events ? events.split(eventSplitter) : system.keys(calls);

        // Loop through the callback list, splicing where appropriate.
        while (event = events.shift()) {
            if (!(list = calls[event]) || !(callback || context)) {
                delete calls[event];
                continue;
            }

            for (i = list.length - 2; i >= 0; i -= 2) {
                if (!(callback && list[i] !== callback || context && list[i + 1] !== context)) {
                    list.splice(i, 2);
                }
            }
        }

        return this;
    };

    /**
     * Triggers the specified events.
     * @method trigger
     * @param {string} [events] One or more events, separated by white space to trigger.
     * @chainable
     */
    Events.prototype.trigger = function(events) {
        var event, calls, list, i, length, args, all, rest;
        if (!(calls = this.callbacks)) {
            return this;
        }

        rest = [];
        events = events.split(eventSplitter);
        for (i = 1, length = arguments.length; i < length; i++) {
            rest[i - 1] = arguments[i];
        }

        // For each event, walk through the list of callbacks twice, first to
        // trigger the event, then to trigger any `"all"` callbacks.
        while (event = events.shift()) {
            // Copy callback lists to prevent modification.
            if (all = calls.all) {
                all = all.slice();
            }

            if (list = calls[event]) {
                list = list.slice();
            }

            // Execute event callbacks.
            if (list) {
                for (i = 0, length = list.length; i < length; i += 2) {
                    list[i].apply(list[i + 1] || this, rest);
                }
            }

            // Execute "all" callbacks.
            if (all) {
                args = [event].concat(rest);
                for (i = 0, length = all.length; i < length; i += 2) {
                    all[i].apply(all[i + 1] || this, args);
                }
            }
        }

        return this;
    };

    /**
     * Creates a function that will trigger the specified events when called. Simplifies proxying jQuery (or other) events through to the events object.
     * @method proxy
     * @param {string} events One or more events, separated by white space to trigger by invoking the returned function.
     * @return {function} Calling the function will invoke the previously specified events on the events object.
     */
    Events.prototype.proxy = function(events) {
        var that = this;
        return (function(arg) {
            that.trigger(events, arg);
        });
    };

    /**
     * Creates an object with eventing capabilities.
     * @class EventsModule
     * @static
     */

    /**
     * Adds eventing capabilities to the specified object.
     * @method includeIn
     * @param {object} targetObject The object to add eventing capabilities to.
     */
    Events.includeIn = function(targetObject) {
        targetObject.on = Events.prototype.on;
        targetObject.off = Events.prototype.off;
        targetObject.trigger = Events.prototype.trigger;
        targetObject.proxy = Events.prototype.proxy;
    };

    return Events;
});

/**
 * Durandal 2.1.0 Copyright (c) 2012 Blue Spire Consulting, Inc. All Rights Reserved.
 * Available via the MIT license.
 * see: http://durandaljs.com or https://github.com/BlueSpire/Durandal for details.
 */
/**
 * The app module controls app startup, plugin loading/configuration and root visual display.
 * @module app
 * @requires system
 * @requires viewEngine
 * @requires composition
 * @requires events
 * @requires jquery
 */
define('durandal/app',['durandal/system', 'durandal/viewEngine', 'durandal/composition', 'durandal/events', 'jquery'], function(system, viewEngine, composition, Events, $) {
    var app,
        allPluginIds = [],
        allPluginConfigs = [];

    function loadPlugins(){
        return system.defer(function(dfd){
            if(allPluginIds.length == 0){
                dfd.resolve();
                return;
            }

            system.acquire(allPluginIds).then(function(loaded){
                for(var i = 0; i < loaded.length; i++){
                    var currentModule = loaded[i];

                    if(currentModule.install){
                        var config = allPluginConfigs[i];
                        if(!system.isObject(config)){
                            config = {};
                        }

                        currentModule.install(config);
                        system.log('Plugin:Installed ' + allPluginIds[i]);
                    }else{
                        system.log('Plugin:Loaded ' + allPluginIds[i]);
                    }
                }

                dfd.resolve();
            }).fail(function(err){
                system.error('Failed to load plugin(s). Details: ' + err.message);
            });
        }).promise();
    }

    /**
     * @class AppModule
     * @static
     * @uses Events
     */
    app = {
        /**
         * The title of your application.
         * @property {string} title
         */
        title: 'Application',
        /**
         * Configures one or more plugins to be loaded and installed into the application.
         * @method configurePlugins
         * @param {object} config Keys are plugin names. Values can be truthy, to simply install the plugin, or a configuration object to pass to the plugin.
         * @param {string} [baseUrl] The base url to load the plugins from.
         */
        configurePlugins:function(config, baseUrl){
            var pluginIds = system.keys(config);
            baseUrl = baseUrl || 'plugins/';

            if(baseUrl.indexOf('/', baseUrl.length - 1) === -1){
                baseUrl += '/';
            }

            for(var i = 0; i < pluginIds.length; i++){
                var key = pluginIds[i];
                allPluginIds.push(baseUrl + key);
                allPluginConfigs.push(config[key]);
            }
        },
        /**
         * Starts the application.
         * @method start
         * @return {promise}
         */
        start: function() {
            system.log('Application:Starting');

            if (this.title) {
                document.title = this.title;
            }

            return system.defer(function (dfd) {
                $(function() {
                    loadPlugins().then(function(){
                        dfd.resolve();
                        system.log('Application:Started');
                    });
                });
            }).promise();
        },
        /**
         * Sets the root module/view for the application.
         * @method setRoot
         * @param {string} root The root view or module.
         * @param {string} [transition] The transition to use from the previous root (or splash screen) into the new root.
         * @param {string} [applicationHost] The application host element or id. By default the id 'applicationHost' will be used.
         */
        setRoot: function(root, transition, applicationHost) {
            var hostElement, settings = { activate:true, transition: transition };

            if (!applicationHost || system.isString(applicationHost)) {
                hostElement = document.getElementById(applicationHost || 'applicationHost');
            } else {
                hostElement = applicationHost;
            }

            if (system.isString(root)) {
                if (viewEngine.isViewUrl(root)) {
                    settings.view = root;
                } else {
                    settings.model = root;
                }
            } else {
                settings.model = root;
            }

            function finishComposition() {
                if(settings.model) {
                    if (settings.model.canActivate) {
                        try {
                            var result = settings.model.canActivate();
                            if (result && result.then) {
                                result.then(function (actualResult) {
                                    if (actualResult) {
                                        composition.compose(hostElement, settings);
                                    }
                                }).fail(function (err) {
                                    system.error(err);
                                });
                            } else if (result) {
                                composition.compose(hostElement, settings);
                            }
                        } catch (er) {
                            system.error(er);
                        }
                    } else {
                        composition.compose(hostElement, settings);
                    }
                } else {
                    composition.compose(hostElement, settings);
                }
            }

            if(system.isString(settings.model)) {
                system.acquire(settings.model).then(function(module) {
                    settings.model = system.resolveObject(module);
                    finishComposition();
                }).fail(function(err) {
                    system.error('Failed to load root module (' + settings.model + '). Details: ' + err.message);
                });
            } else {
                finishComposition();
            }
        }
    };

    Events.includeIn(app);

    return app;
});

require.config({
    paths: {
        "text": "/Scripts/text",
        'durandal': "/Scripts/durandal",
        'plugins': "/Scripts/durandal/plugins",
        'transitions': "/Scripts/durandal/transitions",
        "jquery.contextmenu": "/scripts/jquery.contextMenu",
        "jquery.ui.position": "/scripts/jquery.ui.position",
        'bootstrap': "../Scripts/bootstrap",
    },
    shim: {
        'bootstrap': {
            deps: ["jquery"],
            exports: "jQuery"
        }
    }
});
define("jquery", [],function () { return jQuery; });
define("knockout", ko);

define('main',["durandal/app", "durandal/viewLocator", "durandal/system"],
    function (app, viewLocator, system, config) {
        system.debug(true);
        app.title = config.applicationFullName ;
        
        //specify which plugins to install and their configuration
        app.configurePlugins({
            router: true,
            dialog: true,
            widget: {
                kinds: ["expander"]
            }
        });
        
        app.start().then(function () {
            viewLocator.useConvention();
            app.setRoot("viewmodels/shell", "entrance");
        });
    });

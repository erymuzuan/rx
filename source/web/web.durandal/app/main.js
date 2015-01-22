require.config({
    paths: {
        "text": "/Scripts/text",
        'durandal': '/Scripts/durandal',
        'plugins': '/Scripts/durandal/plugins',
        'transitions': '/Scripts/durandal/transitions',
        "jquery.contextmenu": "/scripts/jquery.contextMenu",
        "jquery.ui.position": "/scripts/jquery.ui.position",
        'bootstrap': '../Scripts/bootstrap',
    },
    shim: {
        'bootstrap': {
            deps: ['jquery'],
            exports: 'jQuery'
        }
    }
});
define('jquery', function () { return jQuery; });
define('knockout', ko);

define(['durandal/app', 'durandal/viewLocator', 'durandal/system',  objectbuilders.config],
    function (app, viewLocator, system, config) {
        system.debug(true);
        app.title = config.applicationFullName ;
        
        //specify which plugins to install and their configuration
        app.configurePlugins({
            router: true,
            dialog: true,
            widget: {
                kinds: ['expander']
            }
        });
        
        app.start().then(function () {
            viewLocator.useConvention();
            app.setRoot('viewmodels/shell', 'entrance');
        });
    });
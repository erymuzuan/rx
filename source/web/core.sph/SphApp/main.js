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

define(['durandal/app', 'durandal/viewLocator', 'durandal/system', 'services/logger'],
    function (app, viewLocator, system) {
        system.debug(true);
        app.title = "SPH";
        
        
        app.start().then(function () {
            viewLocator.useConvention();
            app.setRoot('viewmodels/shell', 'entrance');
        });
    });
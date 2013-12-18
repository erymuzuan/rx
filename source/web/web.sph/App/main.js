require.config({
    paths: {
        "text": "durandal/amd/text",
        "jquery.contextmenu": "/scripts/jquery.contextMenu",
        "jquery.ui.position": "/scripts/jquery.ui.position"
    }
});

define(['durandal/app', 'durandal/viewLocator', 'durandal/system', 'durandal/plugins/router', 'services/logger'],
    function (app, viewLocator, system, router, logger) {

        // Enable debug message to show in the console 
        system.debug(true);
        app.title = "SPH";
        app.start().then(function () {
            toastr.options.positionClass = 'toast-bottom-right';
            toastr.options.backgroundpositionClass = 'toast-bottom-right';

            router.handleInvalidRoute = function (route, params) {
                logger.logError('No Route Found', route, 'main', true);
                app.showMessage("You may not have the permission to access " + route + "<br/>Do you want to login?","Login", ["Yes", "No"])
                    .done(function (dr) {
                        if (dr === "Yes") {
                            window.location = "/Account/Login?returnUrl=" + route;
                        } else {
                            window.location = "/";
                        }
                    });
            };

            // When finding a viewmodel module, replace the viewmodel string 
            // with view to find it partner view.
            router.useConvention();
            viewLocator.useConvention();

            // Adapt to touch devices
            app.adaptToDevice();
            //Show the app by setting the root view model for our application.
            app.setRoot('viewmodels/shell', 'entrance');
        });
    });
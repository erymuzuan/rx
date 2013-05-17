define(['services/logger'], function (logger) {
    var vm = {
        activate: activate,
        title: 'Papan Tugas'
    };

    return vm;

    //#region Internal Methods
    function activate() {
        logger.log('Dashboard View Activated', null, 'details', true);
        return true;
    }
    //#endregion
});
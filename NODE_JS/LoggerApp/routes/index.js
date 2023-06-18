module.exports = function(application) {
    application.get('/', function(req, res) {
        application.api.controllers.home.index(application, req, res);
    });
}

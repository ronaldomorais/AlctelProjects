const express = require('express');
const consign = require('consign');

let app = express();
app.set('view engine', 'ejs');
app.set('views', './api/views');

consign()
    .include('config')
    .then('api/models')
    .then('api/controllers')
    .then('routes')
    // .then('utils')
    .then('helpers')
    .then('api/subscribers')
    .into(app);

// const server_config = app.config.appsettings.getSettings();
// console.log(`Diretório dos logs em ${server_config.log_location}`);
// app.helpers.functions.checkLogsDir(app.utils.log_file_settings.getFullpath(app)),

module.exports = app;
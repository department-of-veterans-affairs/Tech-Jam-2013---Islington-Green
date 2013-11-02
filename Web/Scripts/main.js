requirejs.config({
    baseUrl: '/Scripts/Map/',
    paths: {
        'jquery': '../lib/jquery/jquery.min',
        'underscore': '../lib/underscore/underscore-min',
        'amplify': '../lib/amplify/lib/amplify.min',
        'openlayers': '../lib/openlayers/OpenLayers.min',
        'bootstrap': '../lib/bootstrap/dist/js/bootstrap.min',
        'bootstrap-switch': '../lib/bootstrap-switch/bootstrap-switch.min',
        'fastclick': '../lib/fastclick/fastclick',
        'App': 'App',
        'Data': 'Data',
        'Map': 'Map',
        'Form': 'Form',
        'Ajax': 'Ajax',
    },
    shim: {
        'openlayers': {
            exports: 'OpenLayers'
        },
        'underscore': {
            exports: '_'
        },
        'amplify': {
            deps: ['jquery'],
            exports: 'amplify'
        },
        'bootstrap': {
            deps: ['jquery']
        },
        'bootstrap-switch': {
            deps: ['jquery','bootstrap']
        },
        'Form': {
            deps: ['jquery','underscore','bootstrap','bootstrap-switch']
        },
        'Ajax': {
            deps: ['underscore', 'jquery', 'amplify']
        }
    }
});

require(['Data', 'Map', 'Form', 'Ajax', 'App'], function (Data, Map, Form, Ajax, App) {
    App.init(Data, Map, Form, Ajax);
});

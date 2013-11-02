requirejs.config({
    baseUrl: '/Scripts/Data/',
    paths: {
        'jquery': '../lib/jquery/jquery.min',
        'underscore': '../lib/underscore/underscore-min',
        'amplify': '../lib/amplify/lib/amplify.min',
        'knockout': '../knockout-3.0.0',
        'knockout-mapping': '../knockout.mapping-latest',
        'bootstrap': '../lib/bootstrap/dist/js/bootstrap.min',
        'bootstrap-switch': '../lib/bootstrap-switch/bootstrap-switch.min',
        'fastclick': '../lib/fastclick/fastclick',
        'App': 'App',
        'Data': 'Data',
        'Form': 'Form',
        'Ajax': 'Ajax',
    },
    shim: {
        'underscore': {
            exports: '_'
        },
        'amplify': {
            deps: ['jquery'],
            exports: 'amplify'
        },
        'knockout': {
            exports: 'ko'
        },
        'knockout-mapping': {
            deps: ['knockout'],
            exports: 'ko'
        },
        'bootstrap': {
            deps: ['jquery']
        },
        'bootstrap-switch': {
            deps: ['jquery', 'bootstrap']
        },
        'Form': {
            deps: ['jquery', 'underscore', 'bootstrap', 'bootstrap-switch']
        },
        'Ajax': {
            deps: ['underscore', 'jquery', 'amplify']
        }
    }
});

require(['Data', 'Form', 'Ajax', 'App'], function (Data, Form, Ajax, App) {
    App.init(Data, Form, Ajax);
});

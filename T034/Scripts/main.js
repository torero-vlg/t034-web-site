require.config({
    baseUrl: '/Scripts',
    paths: {
        main: 'main',
        jquery: 'lib/jquery-1.9.0',
        jqueryui: 'lib/jquery-ui-1.9.0',
        jqueryValidate: 'jquery.validate',//заглавные буквы не работают
        jqueryValidateUnobtrusive: 'lib/jquery.validate.unobtrusive',//заглавные буквы не работают
        bootstrap: 'lib/bootstrap',
        datatables: 'lib/DataTables-1.10.2/media/js/jquery.dataTables',
        initdatatables: 'app/t034/t034.dataTables',
        ckeditor: 'lib/ckeditor/ckeditor',
        ckeditoradapter: 'lib/ckeditor/adapters/jquery'
    },
    shim: {
        jqueryui: {
            deps: ['jquery']
        },
        bootstrap: {
            deps: ['jquery']
        },
        datatables: {
            deps: ['jquery']
        },
        initdatatables: {
            deps: ['datatables']
        },
        jqueryValidate: {
            deps: ['jquery']
        },
        jqueryValidateUnobtrusive: {
            deps: ['jquery', 'jqueryValidate']
        },
        ckeditoradapter: {
            deps: ['ckeditor']
        }
    }
});

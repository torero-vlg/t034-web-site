require.config({
    baseUrl: '/Scripts',
    paths: {
        main: 'main',
        jquery: 'lib/jquery-1.9.0',
        jqueryui: 'lib/jquery-ui-1.9.0',
        jqueryValidate: 'jquery.validate',
        jqueryValidateUnobtrusive: 'lib/jquery.validate.unobtrusive',
        bootstrap: 'lib/bootstrap',
        dataTables: 'lib/DataTables-1.10.2/media/js/jquery.dataTables',
        ckeditor: 'lib/ckeditor'
    },
    shim: {
        jqueryui: {
            deps: ['jquery']
        },
        bootstrap: {
            deps: ['jquery']
        },
        dataTables: {
            deps: ['jquery', 'jqueryui']
        },
        jqueryValidate: {
            deps: ['jquery']
        },
        jqueryValidateUnobtrusive: {
            deps: ['jquery', 'jqueryValidate']
        }
    }
});

require.config({
    baseUrl: '/Scripts',
    paths: {
        main: 'main',
        jquery: 'lib/jquery-1.9.0',
        jqueryui: 'lib/jquery-ui-1.9.0',
        jqueryValidateUnobtrusive: 'lib/jquery.validate.unobtrusive',//заглавные буквы не работают
        bootstrap: 'lib/bootstrap',
        datatables: 'lib/DataTables-1.10.2/media/js/jquery.dataTables',
        initdatatables: 'app/t034/t034.dataTables',
        ckeditor: 'lib/ckeditor/ckeditor',
        ckeditoradapter: 'lib/ckeditor/adapters/jquery',

        'jquery.fileupload': '/Scripts/lib/jQuery.FileUpload/jquery.fileupload',
        'jquery.fileupload-ui': 'lib/jQuery.FileUpload/jquery.fileupload-ui',
        'jquery.ui.widget': ['/Scripts/lib/jQuery.FileUpload/vendor/jquery.ui.widget'],
        'jquery.fileupload-image': '/Scripts/lib/jQuery.FileUpload/jquery.fileupload-image',    
        'jquery.fileupload-validate': '/Scripts/lib/jQuery.FileUpload/jquery.fileupload-validate',
        'jquery.fileupload-video': '/Scripts/lib/jQuery.FileUpload/jquery.fileupload-video',
        'jquery.fileupload-audio': '/Scripts/lib/jQuery.FileUpload/jquery.fileupload-audio',
        'jquery.fileupload-process': '/Scripts/lib/jQuery.FileUpload/jquery.fileupload-process',
        'load-image': '/Scripts/lib/jQuery.FileUpload/load-image/load-image',
        'load-image-meta': '/Scripts/lib/jQuery.FileUpload/load-image/load-image-meta',
        'load-image-exif': ['/Scripts/lib/jQuery.FileUpload/load-image/load-image-meta', '/Scripts/lib/jQuery.FileUpload/load-image/load-image-exif'],
        'canvas-to-blob': '/Scripts/lib/jQuery.FileUpload/canvas-to-blob/canvas-to-blob',
        'tmpl': '/Scripts/lib/jQuery.FileUpload/tmpl',
        
        'layout': 'app/layout'
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
        'lib/jquery.validate': {
            deps: ['jquery']
        },
        'lib/jquery.validate.unobtrusive': {
            deps: ['jquery', 'lib/jquery.validate']
        },
        ckeditoradapter: {
            deps: ['ckeditor']
        },
        'jquery.fileupload-ui': {
            deps: ['load-image']
        }
    }
});

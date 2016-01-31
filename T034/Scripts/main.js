require.config({
    baseUrl: "/Scripts/app",
    paths: {
        jquery: "../lib/jquery-1.9.0",
        jqueryValidate: "../lib/jquery.validate",
        jqueryValidateUnobtrusive: "../lib/jquery.validate.unobtrusive",
        bootstrap: "../lib/bootstrap"
    },
    shim: {
        jqueryValidate: ["jquery"],
        jqueryValidateUnobtrusive: ["jquery", "jqueryValidate"]
    }
});

require();
// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$('.item').click(function () {
    let loc = $(this).find("a").attr("href");
    if (loc) {
        window.location = $(this).find("a").attr("href");
    }    
});
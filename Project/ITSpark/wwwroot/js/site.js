// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Add this to your site.js file
function applyRtlIfNeeded() {
    var htmlElement = document.documentElement;
    var lang = htmlElement.getAttribute("lang");

    if (lang === "ar") {
        htmlElement.classList.add("rtl");
    } else {
        htmlElement.classList.remove("rtl");
    }
}

// Call the function on page load
applyRtlIfNeeded();

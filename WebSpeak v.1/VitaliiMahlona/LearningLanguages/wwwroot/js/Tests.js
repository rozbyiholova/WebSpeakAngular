var select = document.querySelectorAll('#actionLink a');
for (var i = 0; i < select.length; i++) {
    select[i].className += "btn btn-outline-success";
    select[select.length - 1].setAttribute("role", "button");
    select[select.length - 1].setAttribute("aria-pressed", "true");
}
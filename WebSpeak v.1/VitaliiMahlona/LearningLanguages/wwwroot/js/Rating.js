$("#totalRatingLang .languages").click(function (event) {
    event.stopPropagation();
    event.stopImmediatePropagation();

    var users = $('.users');
    var attrId = users.filter(`#${$(this).attr('id')}`);

    attrId.slideToggle(300);

    var siblings = $(this).siblings("#totalRatingLang .languages");

    checkSiblingChildren(siblings);

    $(this).toggleClass("active");
});

function checkSiblingChildren(siblings) {
    siblings.removeClass('active');

    while (siblings.children().length > 0 && !siblings.children().hasClass("table")) {
        siblings.children().hide(300);
        siblings.children().removeClass('active');
        siblings = siblings.children();
    }
}
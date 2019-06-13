$(".languages").click(function (event)
{
    event.stopPropagation();
    event.stopImmediatePropagation();

    var categories = $('.categories');
    var attrId = categories.filter(`#${$(this).attr('id')}`);

    attrId.slideToggle(300);

    var siblings = $(this).siblings(".languages");

    checkSiblingChildren(siblings);

    $(this).toggleClass("active");
});

$(".categories").click(function (event)
{
    event.stopPropagation();
    event.stopImmediatePropagation();

    var categories = $(`[cat="${$(this).attr('cat')}"]`);
    var subCategories = categories.filter('.subCategories');
    var attrId = subCategories.filter(`#${$(this).attr('id')}`);

    attrId.slideToggle(300);

    var siblings = $(this).siblings(".categories");

    checkSiblingChildren(siblings);

    $(this).toggleClass("active");
});

$(".subCategories").click(function (event)
{
    event.stopPropagation();
    event.stopImmediatePropagation();

    var subCategories = $(`[subCat="${$(this).attr('subCat')}"]`);
    var categories = subCategories.filter(`[cat="${$(this).attr('cat')}"]`);
    var tests = categories.filter('.tests');
    var attrId = tests.filter(`#${$(this).attr('id')}`);

    attrId.slideToggle(300);

    var siblings = $(this).siblings(".subCategories");

    checkSiblingChildren(siblings);

    $(this).toggleClass("active");
});

$(".tests").click(function (event)
{
    event.stopPropagation();
    event.stopImmediatePropagation();

    var tests = $(`[test="${$(this).attr('test')}"]`);
    var subCategories = tests.filter(`[subCat="${$(this).attr('subCat')}"]`);
    var categories = subCategories.filter(`[cat="${$(this).attr('cat')}"]`);
    var testResults = categories.filter('.testResults');
    var attrId = testResults.filter(`#${$(this).attr('id')}`);

    attrId.slideToggle(300);

    var siblings = $(this).siblings(".tests");

    checkSiblingChildren(siblings);

    $(this).toggleClass("active");
});

function checkSiblingChildren(siblings)
{
    siblings.removeClass('active');

    while (siblings.children().length > 0 && !siblings.children().hasClass("table"))
    {
        siblings.children().hide(300);
        siblings.children().removeClass('active');
        siblings = siblings.children();
    }
}
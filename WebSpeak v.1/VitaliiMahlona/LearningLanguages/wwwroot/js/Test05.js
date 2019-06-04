var correctAnswer = Math.floor(Math.random() * 2) + 1;

var result = 0;
var first = true;

check();

function check(event) {
    if ((event != null) && (event.target.value == correctAnswer)) {
        $('#result').html(`<b>Score: ${++result}</b>`);
    }

    correctAnswer = Math.floor(Math.random() * 2) + 1;

    $.ajax({
        type: 'GET',
        url: `/Home/Categories/SubCategories/Tests/Test01or05/Test?id=${subCategoryId}`,
        success: function (result) {
            var s = `<div class="name">
                        <p>${result[0].wordLearnLang}</p>
                    `;
            for (let i = 0; i < Object.keys(result).length; i++) {
                if (correctAnswer == i + 1) {
                    s += `<img src="../../../../${result[i].picture}" alt="${result[i].wordNativeLang}">`;
                    break;
                }
            }
            s += `<div>`;
            $('#test').html(s);
        }
    });
}
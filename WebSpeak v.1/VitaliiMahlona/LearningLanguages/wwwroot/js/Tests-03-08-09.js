var correctAnswer = Math.floor(Math.random() * 4) + 1;
var result = 0;
var first = true;

check();

function check() {
    if (($("[type=radio]:checked").val() == correctAnswer)) {
        $('#result').html(`<b>Score: ${++result}</b>`);
    }
    else if (($("[type=radio]:checked").val() == undefined)) {
        if (!first) {
            $('#error').show();
            $('#error').text("Please select an item!");
            return;
        }
        first = false;
    }

    $('#error').hide();

    correctAnswer = Math.floor(Math.random() * 4) + 1;

    $.ajax({
        type: 'GET',
        url: `/Home/Categories/SubCategories/Tests/Test02or03or04or08or09/Test?id=${subCategoryId}`,
        success: function (result) {
            var s = '<div class="words">';
            for (let i = 0; i < Object.keys(result).length; i++) {
                if (testNumber == 3) {
                    s += `
                    <input type="radio" name="test" value="${i + 1}" id="${i + 1}"/>
                    <label class="word btn btn-light" for="${i + 1}">${result[i].wordLearnLang}</label>
                    `;
                }
                if (testNumber == 8 || testNumber == 9) {
                    s += `
                    <input type="radio" name="test" value="${i + 1}" id="${i + 1}"/>
                    <label class="word btn btn-light" for="${i + 1}">${result[i].wordNativeLang}</label>
                    `;
                }
            }
            s += `</div>
                        <div class="QA">`;
            for (let i = 0; i < Object.keys(result).length; i++) {
                if (correctAnswer == i + 1) {
                    if (testNumber == 3)
                    {
                        s += `<img src="../../../../${result[i].picture}" width="256" height="256" alt="${result[i].wordLearnLang}">`;
                    }
                    if (testNumber == 8) {
                        s += `<h1>${result[i].wordLearnLang}</h1>`;
                    }
                    if (testNumber == 9) {
                        s += `<audio controls>
                              <source src="../../../../${result[i].pronounceLearn}" type="audio/mpeg" />
                              Your browser does not support the audio element.
                              </audio >`;
                    }
                    break;
                }
            }
            s += `</div>`;

            $('#test').html(s);
        }
    });
}
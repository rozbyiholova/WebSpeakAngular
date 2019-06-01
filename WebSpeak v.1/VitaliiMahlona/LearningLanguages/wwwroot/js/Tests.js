var correctAnswer = Math.floor(Math.random() * 2) + 1;
var result = 0;
var first = true;

var test4 = false;

if (testNumber == 4) test4 = true;
if ((testNumber == 2) || (testNumber == 4)) testNumber = "2or04";

check();

function check() {
    if (($("[type=radio]:checked").val() == correctAnswer)) {
        console.log("+");
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
    else {
        console.log("-");
    }

    $('#error').hide();

    correctAnswer = Math.floor(Math.random() * 2) + 1;

    $.ajax({
        type: 'GET',
        url: `/Home/Categories/SubCategories/Tests/Test0${testNumber}/Test?id=${subCategoryId}`,
        success: function (result) {
            var s = '<div class="words">';
            for (let i = 0; i < Object.keys(result).length; i++) {
                s += `
                    <label class="word">
                        <p>${result[i].wordLearnLang}</p>
                        <input type="radio" name="test" value="${i + 1}">
                        <img src="../../../../${result[i].picture}" width="256" height="256" alt="${result[i].wordLearnLang}">
                    </label>
                    `;
            }
            s += `</div>
                        <div class="QA">`;
            for (let i = 0; i < Object.keys(result).length; i++) {
                if (correctAnswer == i + 1) {
                    if (test4) {
                        s += `<audio controls>
                              <source src="../../../../${result[i].pronounceLearn}" type="audio/mpeg" />
                              Your browser does not support the audio element.
                              </audio>`
                    }
                    else {
                        s += `<h1>${result[i].wordLearnLang}</h1>`;
                    }
                    break;
                }
            }
            s += `</div>`;

            $('#test').html(s);
        }
    });
}
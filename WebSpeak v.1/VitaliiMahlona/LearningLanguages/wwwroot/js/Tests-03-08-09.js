var correctAnswer = Math.floor(Math.random() * 4) + 1;
var totalResult = 0;
var randomWords = [];
var first = true;
var countOptions = 4;
var randomTestWordsId = [1, 2, 3, 4];
var questionNumber = 0;
var totalQuestions = model.length;
var firstId = model[0].id;
var lastId = model[model.length - 1].id;
model = model.sort(compareRandom);

check();

function check() {
    if (($("[type=radio]:checked").val() == correctAnswer)) {
        $('#result').html(`<b>Score: ${++totalResult}</b>`);
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

    if (questionNumber == totalQuestions) {
        $.ajax({
            type: 'POST',
            url: '/Home/Test',
            data: {
                totalResult,
                subCategoryId,
                testNumber
            },
            success: function (result) {
                $('#test').hide();

                var s = '<button type="submit" class="btn btn-primary" onclick="again()">Again</button>';

                if (result.isUser) {
                    s += `<a class="btn btn-secondary" href="/Account/Statistics" role="button">To general statistics</a>`;
                }

                $('.buttonSubmit').html(s);
            }
        })
        return;
    }
    questionNumber++;

    randomWords = GetTest();

    randomTestWordsId.sort(compareRandom);

    correctAnswer = Math.floor(Math.random() * 4) + 1;

    var s = '<div class="words">';

    for (let i = 0; i < Object.keys(randomWords).length; i++) {
        if (randomTestWordsId[i] - 1 == 0) correctAnswer = i + 1;
        if (testNumber == 3) {
            s += `<input type="radio" name="test" value="${i + 1}" id="${i + 1}"/>
                  <label class="word btn btn-light" for="${i + 1}">${randomWords[randomTestWordsId[i] - 1].wordLearnLang}</label>
                 `;
        }

        if (testNumber == 8 || testNumber == 9) {
            s += `<input type="radio" name="test" value="${i + 1}" id="${i + 1}"/>
                  <label class="word btn btn-light" for="${i + 1}">${randomWords[randomTestWordsId[i] - 1].wordNativeLang}</label>
                 `;
        }
    }

    s += `</div>
             <div class="QA">
         `;

    for (let i = 0; i < Object.keys(randomWords).length; i++) {
        if (correctAnswer == i + 1) {
            if (testNumber == 3)
            {
                s += `<img src="../../../../${randomWords[0].picture}" width="256" height="256" alt="${randomWords[0].wordLearnLang}">`;
            }

            if (testNumber == 8) {
                s += `<h1>${randomWords[0].wordLearnLang}</h1>`;
            }

            if (testNumber == 9) {
                s += `<audio controls>
                         <source src="../../../../${randomWords[0].pronounceLearn}" type="audio/mpeg" />
                         Your browser does not support the audio element.
                      </audio >
                     `;
            }

            break;
        }
    }

    s += '</div>';

    $('#test').html(s);
}

function GetTest() {
    var randomWordsId = [];
    randomWords[0] = model[questionNumber - 1];
    randomWordsId[0] = model[questionNumber - 1].id;

    var randomIds = [];

    for (let i = firstId; i <= lastId; ++i) {
        randomIds.push(i);
    }

    randomIds.splice(randomIds.indexOf(randomWordsId[0]), 1);

    randomIds.sort(compareRandom);

    for (let i = 1; i < countOptions; i++) {
        randomWordsId[i] = randomIds[0];
        randomIds.splice(0, 1);

        for (let j = 0; j < model.length; j++) {
            if (model[j].id == randomWordsId[i]) {
                randomWords[i] = model[j];
            }
        }
    }

    return randomWords;
}


function again() {
    location.reload();
}

function compareRandom(a, b) {
    return Math.random() - 0.5;
}
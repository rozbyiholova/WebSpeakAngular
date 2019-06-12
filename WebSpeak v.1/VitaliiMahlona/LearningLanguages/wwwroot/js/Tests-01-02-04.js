var correctAnswer = Math.floor(Math.random() * 4) + 1;
var countOptions = 4;
var totalResult = 0;
var first = true;
var randomWords = [];
var questionNumber = 0;
var totalQuestions = model.length;
var randomTestWordsId = [1, 2, 3, 4];
var firstId = model[0].id;
var lastId = model[model.length - 1].id;
model = model.sort(compareRandom);

if (testNumber == 1) {
    correctAnswer = Math.floor(Math.random() * 2) + 1;
    countOptions = 2;
    randomTestWordsId = [1, 2];
} 

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

    randomTestWordsId.sort(compareRandom);

    randomWords = GetTest();

    var s = '<div class="words">';

    for (let i = 0; i < Object.keys(randomWords).length; i++) {
        if (randomTestWordsId[i] - 1 == 0) correctAnswer = i + 1;
        s += `<label class="word">
                 <input type="radio" name="test" value="${i + 1}">
                 <img src="../../../../${randomWords[randomTestWordsId[i] - 1].picture}" width="256" height="256" alt="${randomWords[randomTestWordsId[i] - 1].wordLearnLang}">
              </label>
             `;
    }

    s += `</div>
             <div class="QA">
         `;

    for (let i = 0; i < Object.keys(randomWords).length; i++) {
        if (correctAnswer == i + 1) {
            if (testNumber == 4) {
                s += `<audio controls>
                         <source src="../../../../${randomWords[0].pronounceLearn}" type="audio/mpeg" />
                         Your browser does not support the audio element.
                      </audio>
                     `;
            }
            else {
                s += `<h1>${randomWords[0].wordLearnLang}</h1>`;
            }
            break;
        }
    }

    s += `</div>`;

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
var correctAnswer = Math.floor(Math.random() * 4) + 1;
var totalResult = 0;
var fourWords = [];
var first = true;
var randomTestWordsId = [1, 2, 3, 4];
var questionNumber = 0;
var totalQuestions = model.length;
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
                    s += `<a class="btn btn-secondary" href="#" role="button">To general statistics</a>`;
                }

                $('.buttonSubmit').html(s);
            }
        })
        return;
    }
    questionNumber++;

    fourWords = GetTest();

    randomTestWordsId.sort(compareRandom);

    correctAnswer = Math.floor(Math.random() * 4) + 1;

    var s = '<div class="words">';

    for (let i = 0; i < Object.keys(fourWords).length; i++) {
        if (randomTestWordsId[i] - 1 == 0) correctAnswer = i + 1;
        if (testNumber == 3) {
            s += `<input type="radio" name="test" value="${i + 1}" id="${i + 1}"/>
                  <label class="word btn btn-light" for="${i + 1}">${fourWords[randomTestWordsId[i] - 1].wordLearnLang}</label>
                 `;
        }

        if (testNumber == 8 || testNumber == 9) {
            s += `<input type="radio" name="test" value="${i + 1}" id="${i + 1}"/>
                  <label class="word btn btn-light" for="${i + 1}">${fourWords[randomTestWordsId[i] - 1].wordNativeLang}</label>
                 `;
        }
    }

    s += `</div>
             <div class="QA">
         `;

    for (let i = 0; i < Object.keys(fourWords).length; i++) {
        if (correctAnswer == i + 1) {
            if (testNumber == 3)
            {
                s += `<img src="../../../../${fourWords[0].picture}" width="256" height="256" alt="${fourWords[0].wordLearnLang}">`;
            }

            if (testNumber == 8) {
                s += `<h1>${fourWords[0].wordLearnLang}</h1>`;
            }

            if (testNumber == 9) {
                s += `<audio controls>
                         <source src="../../../../${fourWords[0].pronounceLearn}" type="audio/mpeg" />
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
    var countOptions = 4;
    var randomWordsId = [];
    var fourWords = [];
    fourWords[0] = model[questionNumber - 1];
    randomWordsId[0] = model[questionNumber - 1].id;

    for (let i = 1; i < countOptions; ++i) {
        randomWordsId[i] = Math.floor(Math.random() * (model[model.length - 1].id - model[0].id + 1)) + model[0].id;

        for (let j = 0; j < i; j++) {
            while (randomWordsId[j] == randomWordsId[i]) {
                randomWordsId[i] = Math.floor(Math.random() * (model[model.length - 1].id - model[0].id + 1)) + model[0].id;
            }
        }

        for (let j = 0; j < model.length; j++) {
            if (model[j].id == randomWordsId[i]) {
                fourWords[i] = model[j];
            }
        }
    }

    return fourWords;
}

function again() {
    location.reload();
}

function compareRandom(a, b) {
    return Math.random() - 0.5;
}
var correctAnswer = Math.floor(Math.random() * 2) + 1;
var countOptions = 2;
var totalResult = 0;
var first = true;
var randomWords = [];
var questionNumber = 0;
var totalQuestions = model.length;
model = model.sort(compareRandom);

check();

function check(event) {
    if ((event != null) && (event.target.value == correctAnswer)) {
        $('#result').html(`<b>Score: ${++totalResult}</b>`);
    }

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

    randomWords = GetTest();

    correctAnswer = Math.floor(Math.random() * 2) + 1;

    var s = `<div class="name">
                <p>${randomWords[0].wordLearnLang}</p>
            `;

    for (let i = 0; i < Object.keys(randomWords).length; i++) {
        if (correctAnswer == i + 1) {
            s += `<img src="../../../../${randomWords[i].picture}" alt="${randomWords[i].wordNativeLang}">`;
            break;
        }
    }

    s += '<div>';

    $('#test').html(s);
}

function GetTest() {
    var randomWordsId = [];
    randomWords[0] = model[questionNumber - 1];
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
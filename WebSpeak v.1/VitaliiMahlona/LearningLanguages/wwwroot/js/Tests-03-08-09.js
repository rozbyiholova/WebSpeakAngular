var correctAnswer = Math.floor(Math.random() * 4) + 1;
var totalResult = 0;
var fourWords = [];
var first = true;

check();

function check() {
    fourWords = GetTest();

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

    correctAnswer = Math.floor(Math.random() * 4) + 1;

    var s = '<div class="words">';

    for (let i = 0; i < Object.keys(fourWords).length; i++) {
        if (testNumber == 3) {
            s += `<input type="radio" name="test" value="${i + 1}" id="${i + 1}"/>
                  <label class="word btn btn-light" for="${i + 1}">${fourWords[i].wordLearnLang}</label>
                 `;
        }

        if (testNumber == 8 || testNumber == 9) {
            s += `<input type="radio" name="test" value="${i + 1}" id="${i + 1}"/>
                  <label class="word btn btn-light" for="${i + 1}">${fourWords[i].wordNativeLang}</label>
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
                s += `<img src="../../../../${fourWords[i].picture}" width="256" height="256" alt="${fourWords[i].wordLearnLang}">`;
            }

            if (testNumber == 8) {
                s += `<h1>${fourWords[i].wordLearnLang}</h1>`;
            }

            if (testNumber == 9) {
                s += `<audio controls>
                         <source src="../../../../${fourWords[i].pronounceLearn}" type="audio/mpeg" />
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

    for (let i = 0; i < countOptions; ++i) {
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
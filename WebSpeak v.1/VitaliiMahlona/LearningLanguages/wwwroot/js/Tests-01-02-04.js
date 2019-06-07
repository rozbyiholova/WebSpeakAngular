var correctAnswer = Math.floor(Math.random() * 4) + 1;
var countOptions = 4;
var totalResult = 0;
var first = true;
var randomWwords = [];

if (testNumber == 1) {
    correctAnswer = Math.floor(Math.random() * 2) + 1;
    countOptions = 2;
} 

check();

function check() {
    randomWwords = GetTest();

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

    if (testNumber == 1) {
        correctAnswer = Math.floor(Math.random() * 2) + 1;
    } 

    var s = '<div class="words">';

    for (let i = 0; i < Object.keys(randomWwords).length; i++) {
        s += `<label class="word">
                 <input type="radio" name="test" value="${i + 1}">
                 <img src="../../../../${randomWwords[i].picture}" width="256" height="256" alt="${randomWwords[i].wordLearnLang}">
              </label>
             `;
    }

    s += `</div>
             <div class="QA">
         `;

    for (let i = 0; i < Object.keys(randomWwords).length; i++) {
        if (correctAnswer == i + 1) {
            if (testNumber == 4) {
                s += `<audio controls>
                         <source src="../../../../${randomWwords[i].pronounceLearn}" type="audio/mpeg" />
                         Your browser does not support the audio element.
                      </audio>
                     `;
            }
            else {
                s += `<h1>${randomWwords[i].wordLearnLang}</h1>`;
            }
            break;
        }
    }

    s += `</div>`;

    $('#test').html(s);
}

function GetTest() {
    var randomWordsId = [];
    var randomWwords = [];

    for (let i = 0; i < countOptions; ++i) {
        randomWordsId[i] = Math.floor(Math.random() * (model[model.length - 1].id - model[0].id + 1)) + model[0].id;

        for (let j = 0; j < i; j++) {
            while (randomWordsId[j] == randomWordsId[i]) {
                randomWordsId[i] = Math.floor(Math.random() * (model[model.length - 1].id - model[0].id + 1)) + model[0].id;
            }
        }

        for (let j = 0; j < model.length; j++) {
            if (model[j].id == randomWordsId[i]) {
                randomWwords[i] = model[j];
            }
        }
    }

    return randomWwords;
}
var totalResult = 0;
var countOptions = 2;
var correctAnswer = Math.random();
var randomWord;
var first = true;

check();

function check() {
    randomWord = GetTest();

    if (($("#resultInput").val() == correctAnswer)) {
        $('#result').html(`<b>Score: ${++totalResult}</b>`);
    }
    else if (($.trim($("#resultInput").val())) == 0) {
        if (!first) {
            $('#error').show();
            $('#error').text("Please fill out the field!");

            return;
        }

        first = false;
    }

    $('#error').hide();

    correctAnswer = randomWord.wordLearnLang;

    var s = '';

    if (testNumber == 6) {
        s += `<div class="image">
                 <img src="../../../../${randomWord.picture}" alt="${randomWord.wordNativeLang}">
              </div>
             `;
    }

    if (testNumber == 7) {
        s += `<div class="audio">
                 <audio controls>
                    <source src="../../../../${randomWord.pronounceLearn}" type="audio/mpeg" />
                    Your browser does not support the audio element.
                 </audio>
              </div>
             `;
    }

    s += `<div class="form-group w-50">
             <label for="resultInput">Your answer</label>
             <input type="text" class="form-control" id="resultInput" placeholder="Enter answer">
          </div>
         `;

    $('#test').html(s);
}


function GetTest() {
    var randomWordId = Math.floor(Math.random() * (model[model.length - 1].id - model[0].id + 1)) + model[0].id;

    for (let j = 0; j < model.length; j++) {
        if (model[j].id == randomWordId) {
            randomWord = model[j];
        }
    }

    return randomWord;
}
var totalResult = 0;
var countOptions = 2;
var correctAnswer = Math.random();
var randomWord;
var first = true;
var questionNumber = 0;
var totalQuestions = model.length;
model = model.sort(compareRandom);

check();

function check() {
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

    randomWord = GetTest();

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
    randomWord = model[questionNumber - 1];

    return randomWord;
}

function again() {
    location.reload();
}

function compareRandom(a, b) {
    return Math.random() - 0.5;
}
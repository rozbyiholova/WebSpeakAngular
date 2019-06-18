var countOptions = 2;
var correctAnswer = Math.random();
var randomWord;

check();

function check()
{
    if (($("#resultInput").val() == correctAnswer))
    {
        ++totalResult;
    }
    else if (($.trim($("#resultInput").val())) == 0)
    {
        if (!first)
        {
            $('#error').show();
            $('#error').text("Please fill out the field!");

            return;
        }

        first = false;
    }

    $('#result').html(`<b>Score: ${totalResult}  Question ${questionNumber + 1}/${totalQuestions}</b>`);

    $('#error').hide();

    if (questionNumber == totalQuestions)
    {
        $('#result').html(`<b>Score: ${totalResult}  Total Question : ${totalQuestions}</b>`);

        SendAjaxRequest();

        return;
    }

    questionNumber++;

    randomWord = GetTest();

    correctAnswer = randomWord.wordLearnLang;

    var s = '';

    if (testNumber == 6)
    {
        s += `<div class="image">
                 <img src="../../../../${randomWord.picture}" alt="${randomWord.wordNativeLang}">
              </div>
             `;
    }

    if (testNumber == 7)
    {
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


function GetTest()
{
    randomWord = model[questionNumber - 1];

    return randomWord;
}
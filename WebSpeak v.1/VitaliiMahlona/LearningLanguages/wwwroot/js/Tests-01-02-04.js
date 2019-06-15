var countOptions = 4;
var correctAnswer = Math.floor(Math.random() * 4) + 1;

if (testNumber == 1)
{
    correctAnswer = Math.floor(Math.random() * 2) + 1;
    countOptions = 2;
    randomTestWordsId = [1, 2];
} 

check();

function check()
{
    if (($("[type=radio]:checked").val() == correctAnswer))
    {
        ++totalResult;
    }
    else if (($("[type=radio]:checked").val() == undefined))
    {
        if (!first)
        {
            $('#error').show();
            $('#error').text("Please select an item!");

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

    randomTestWordsId.sort(compareRandom);

    randomWords = GetTestRandom();

    var s = '<div class="words">';

    for (let i = 0; i < Object.keys(randomWords).length; i++)
    {
        if (randomTestWordsId[i] - 1 == 0)
        {
            correctAnswer = i + 1;
        }

        s += `<label class="word">
                 <input type="radio" name="test" value="${i + 1}">
                 <img src="../../../../${randomWords[randomTestWordsId[i] - 1].picture}" width="256" height="256" alt="${randomWords[randomTestWordsId[i] - 1].wordLearnLang}">
              </label>
             `;
    }

    s += `</div>
             <div class="QA">
         `;

    for (let i = 0; i < Object.keys(randomWords).length; i++)
    {
        if (correctAnswer == i + 1)
        {
            if (testNumber == 4)
            {
                s += `<audio controls>
                         <source src="../../../../${randomWords[0].pronounceLearn}" type="audio/mpeg" />
                         Your browser does not support the audio element.
                      </audio>
                     `;
            }
            else
            {
                s += `<h1>${randomWords[0].wordLearnLang}</h1>`;
            }
            break;
        }
    }

    s += `</div>`;

    $('#test').html(s);
}


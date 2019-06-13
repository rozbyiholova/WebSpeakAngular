var countOptions = 2;
var correctAnswer = Math.floor(Math.random() * 2) + 1;

check();

function check(event)
{
    if ((event != null) && (event.target.value == correctAnswer))
    {
        $('#result').html(`<b>Score: ${++totalResult}</b>`);
    }

    if (questionNumber == totalQuestions)
    {
        SendAjaxRequest();
    }

    questionNumber++;

    randomWords = GetTestRandom();

    correctAnswer = Math.floor(Math.random() * 2) + 1;

    var s = `<div class="name">
                <p>${randomWords[0].wordLearnLang}</p>
            `;

    for (let i = 0; i < Object.keys(randomWords).length; i++)
    {
        if (correctAnswer == i + 1)
        {
            s += `<img src="../../../../${randomWords[i].picture}" alt="${randomWords[i].wordNativeLang}">`;
            break;
        }
    }

    s += '<div>';

    $('#test').html(s);
}

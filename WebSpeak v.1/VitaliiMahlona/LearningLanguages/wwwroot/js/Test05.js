var countOptions = 2;
var correctAnswer = Math.floor(Math.random() * 2) + 1;

check();

function check(event)
{
    if ((event != null) && (event.target.value == correctAnswer))
    {
        ++totalResult;
    }

    $('#result').html(`<b>Score: ${totalResult}  Question ${questionNumber + 1}/${totalQuestions}</b>`);

    if (questionNumber == totalQuestions)
    {
        $('#result').html(`<b>Score: ${totalResult}  Total Question : ${totalQuestions}</b>`);

        SendAjaxRequest();

        return;
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

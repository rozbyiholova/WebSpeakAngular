var totalResult = 0;
var first = true;
var questionNumber = 0;
var randomWords = [];
var totalQuestions = model.length;
var firstId = model[0].id;
var lastId = model[model.length - 1].id;
var randomTestWordsId = [1, 2, 3, 4];

model = model.sort(compareRandom);

function again()
{
    location.reload();
}

function compareRandom(a, b)
{
    return Math.random() - 0.5;
}

function SendAjaxRequest()
{
    $.ajax({
        type: 'POST',
        url: '/Home/Test',
        data:
        {
            totalResult,
            subCategoryId,
            testNumber
        },
        success: function (result)
        {
            $('#test').hide();
            $('#answer').hide();

            var s = '<button type="submit" class="btn btn-primary" onclick="again()">Again</button>';

            if (result.isUser)
            {
                s += `<a class="btn btn-secondary" href="/Account/Statistics" role="button">To general statistics</a>`;
            }

            $('.buttonSubmit').html(s);
        }
    })
}

function GetTestRandom()
{
    var randomWordsId = [];
    randomWords[0] = model[questionNumber - 1];
    randomWordsId[0] = model[questionNumber - 1].id;

    var randomIds = [];

    for (let i = firstId; i <= lastId; ++i)
    {
        randomIds.push(i);
    }

    randomIds.splice(randomIds.indexOf(randomWordsId[0]), 1);

    randomIds.sort(compareRandom);

    for (let i = 1; i < countOptions; i++)
    {
        randomWordsId[i] = randomIds[0];
        randomIds.splice(0, 1);

        for (let j = 0; j < model.length; j++)
        {
            if (model[j].id == randomWordsId[i])
            {
                randomWords[i] = model[j];
            }
        }
    }

    return randomWords;
}
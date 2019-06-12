var countOptions = 4;
var totalResult = 0;
var result = 0;
var numberQA = 0;
var randomWords = [];
var fourWords = [];
var randomTestWordsId = [1, 2, 3, 4];
var questionNumber = 0;
var firstId = model[0].id;
var lastId = model[model.length - 1].id;
model = model.sort(compareRandom);
randomWords = model;

GetExtra();

var totalQuestions = randomWords.length;

check(false);

function check(cancel) {
    result = 0;
    if (cancel == false) {

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
                    $('#answer').hide();

                    var s = '<button type="submit" class="btn btn-primary" onclick="again()">Again</button>';

                    if (result.isUser) {
                        s += `<a class="btn btn-secondary" href="/Account/Statistics" role="button">To general statistics</a>`;
                    }

                    $('.buttonSubmit').html(s);
                }
            })
            return;
        }

        GetTest();
        
        randomTestWordsId.sort(compareRandom);
    }

    $('#result').html(`<b>Score: ${totalResult}</b>`);

    $('.buttonSubmit').html(`<button type="button" class="btn btn-primary" onclick="next()">Next</button>
                             <button type="button" class="btn btn-danger" onclick="cancel()">Cancel</button>
                            `);

    $('#answer').html('<h3>Your answer: </h3>');

    var s = `<div class="container">
                <div class="row">
                    <div class="col-sm" id="leftColumn">
            `;

    for (let i = 0; i < Object.keys(fourWords).length; i++)
    {
        s += `<input type="radio" name="leftColumn" value="${i + 1}" id="leftColumn${i + 1}"/>
              <label class="word btn btn-light" for="leftColumn${i + 1}">${fourWords[i].wordLearnLang}</label>
             `;
    }

    s += `</div>
            <div class="col-sm" id="rightColumn">
         `;

    for (let i = 0; i < Object.keys(fourWords).length; i++)
    {
        s += `<input type="radio" name="rightColumn" value="${randomTestWordsId[i]}" id="rightColumn${randomTestWordsId[i]}"/>
              <label class="word btn btn-light" for="rightColumn${randomTestWordsId[i]}">${fourWords[randomTestWordsId[i] - 1].wordNativeLang}</label>
             `;
    }

    s += `     </div>
             </div>
          </div
         `;
            
    $('#test').html(s);
}

function next()
{
    questionNumber++;

    $('#error').hide();

    if (($("[name=leftColumn]:checked").val() != undefined) && ($("[name=rightColumn]:checked").val() != undefined)) {
        $(`<p>${$(`label[for=${$("[name=leftColumn]:checked").attr("id")}]`).text()} - ${$(`label[for=${$("[name=rightColumn]:checked").attr("id")}]`).text()}</p>`).appendTo('#answer');

        numberQA++;

        if ($("[name=leftColumn]:checked").val() == $("[name=rightColumn]:checked").val()) {
            $('#result').html(`<b>Score: ${++totalResult}</b>`);
            result++;
        }

        $(`label[for=${$("[name=leftColumn]:checked").attr("id")}]`).detach();
        $(`label[for=${$("[name=rightColumn]:checked").attr("id")}]`).detach();

        $("[name=leftColumn]:checked").detach();
        $("[name=rightColumn]:checked").detach();

        if (numberQA == 4) {
            $('.buttonSubmit').html(`<button id="submit" type="submit" class="btn btn-success" onclick="check(false)">Submit</button>
                                     <button type="button" class="btn btn-danger" onclick="cancel()">Cancel</button>`);
            numberQA = 0;
        }
    }
    else if (($("#leftColumn:checked").val() == undefined)) {
        $('#error').show();
        $('#error').text("Please select an item!");

        return;
    }
}

function cancel() {
    totalResult -= result;
    questionNumber -= result;
    numberQA = 0;
    check(true);
}

function GetTest() {
    for (let i = 0; i < countOptions; i++)
    {
        fourWords[i] = randomWords[0];
        randomWords.splice(0, 1);
    }
}

function GetExtra() {
    var remainderOfDivision = randomWords.length % countOptions;

    if (remainderOfDivision == 0) return;

    var randomIds = [];

    for (let i = firstId; i <= lastId; ++i) {
        randomIds.push(i);
    }

    for (let i = 0; i < remainderOfDivision; ++i) {
        randomIds.splice(randomIds.indexOf(randomWords[randomWords.length - i - 1].id), 1);
    }

    randomIds.sort(compareRandom);

    for (let i = 0; i < 4 - remainderOfDivision; ++i) {
        randomWords.push(model[i]);
    }
}

function again() {
    location.reload();
}

function compareRandom(a, b) {
    return Math.random() - 0.5;
}
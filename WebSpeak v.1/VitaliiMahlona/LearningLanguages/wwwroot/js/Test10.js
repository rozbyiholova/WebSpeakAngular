var correctAnswer;
var totalResult = 0;
var result = 0;
var numberQA = 0;
var fourWords = [];
var randomTestWordsId = [1, 2, 3, 4];

check(false);


function check(cancel) {
    if (cancel == false) {
        fourWords = GetTest();
        randomTestWordsId.sort(compareRandom);
    }

    $('#result').html(`<b>Score: ${totalResult+=result}</b>`);

    $('#buttons').html(`<button type="button" class="btn btn-primary" onclick="next()">Next</button>
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
    $('#error').hide();

    if (($("[name=leftColumn]:checked").val() != undefined) && ($("[name=rightColumn]:checked").val() != undefined)) {
        $(`<p>${$(`label[for=${$("[name=leftColumn]:checked").attr("id")}]`).text()} - ${$(`label[for=${$("[name=rightColumn]:checked").attr("id")}]`).text()}</p>`).appendTo('#answer');

        numberQA++;

        if ($("[name=leftColumn]:checked").val() == $("[name=rightColumn]:checked").val()) {
            result++;
        }

        $(`label[for=${$("[name=leftColumn]:checked").attr("id")}]`).detach();
        $(`label[for=${$("[name=rightColumn]:checked").attr("id")}]`).detach();

        $("[name=leftColumn]:checked").detach();
        $("[name=rightColumn]:checked").detach();

        if (numberQA == 4) {
            $('#buttons').html(`<button id="submit" type="submit" class="btn btn-success" onclick="check(false)">Submit</button>
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
    numberQA = 0;
    result = 0;
    check(true);
}

function GetTest() {
    var countOptions = 4;
    var randomWordsId = [];
    var fourWords = [];

    for (let i = 0; i < countOptions; ++i)
    {
        randomWordsId[i] = Math.floor(Math.random() * (model[model.length - 1].id - model[0].id + 1)) + model[0].id;

        for (let j = 0; j < i; j++)
        {
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

function compareRandom(a, b) {
    return Math.random() - 0.5;
}
var correctAnswer;
var totalResult = 0;
var result = 0;
var numberQA = 0;

check();

function check() {
    $('#result').html(`<b>Score: ${totalResult+=result}</b>`);

    $('#buttons').html(`<button type="button" class="btn btn-primary" onclick="next()">Next</button>
                        <button type="button" class="btn btn-danger" onclick="cancel()">Cancel</button>
                       `);

    $('#answer').html('');

    $.ajax({
        type: 'GET',
        url: `/Home/Categories/SubCategories/Tests/Test02or03or04or08or09/Test?id=${subCategoryId}`,
        success: function (result)
        {
            var s = `<div class="container">
                         <div class="row">
                            <div class="col-sm" id="leftColumn">
                    `;
            for (let i = 0; i < Object.keys(result).length; i++)
            {
                s += `<input type="radio" name="leftColumn" value="${i + 1}" id="leftColumn${i + 1}"/>
                      <label class="word btn btn-light" for="leftColumn${i + 1}">${result[i].wordLearnLang}</label>
                     `;
            }
            s += `</div>
                    <div class="col-sm" id="rightColumn">
                 `;

            var randomWordsId = [1, 2, 3, 4];

            function compareRandom(a, b) {
                return Math.random() - 0.5;
            }

            randomWordsId.sort(compareRandom);

            for (let i = 0; i < Object.keys(result).length; i++)
            {
                s += `<input type="radio" name="rightColumn" value="${randomWordsId[i]}" id="rightColumn${randomWordsId[i]}"/>
                      <label class="word btn btn-light" for="rightColumn${randomWordsId[i]}">${result[randomWordsId[i] - 1].wordNativeLang}</label>
                     `;
            }
            s += `      </div>
                    </div>
                  </div`;
            
            $('#test').html(s);
        }
    });
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
            $('#buttons').html(`<button id="submit" type="submit" class="btn btn-success" onclick="check()">Submit</button>
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
    check();
}
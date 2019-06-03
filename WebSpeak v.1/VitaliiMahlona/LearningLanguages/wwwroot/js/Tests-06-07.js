var result = 0;

var correctAnswer = Math.random();

var first = true;

check();


function check() {
    if (($("#resultInput").val() == correctAnswer)) {
        $('#result').html(`<b>Score: ${++result}</b>`);
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

    $.ajax({
        type: 'GET',
        url: `/Home/Categories/SubCategories/Tests/Test06or07/Test?id=${subCategoryId}`,
        success: function (result) {
            correctAnswer = result.wordLearnLang;
            var s = `<div class="audio">
                        <audio controls>
                            <source src="../../../../${result.pronounceLearn}" type="audio/mpeg" />
                            Your browser does not support the audio element.
                        </audio >
                     </div>
                     <div class="form-group w-50">
                         <label for="resultInput">Your answer</label>
                         <input type="text" class="form-control" id="resultInput" placeholder="Enter answer">
                     </div>
                     `;

            $('#test').html(s);
        }
    });
}
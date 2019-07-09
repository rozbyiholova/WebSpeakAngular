var i = -1;
var b;
var array = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 12, 13, 14, 15]
var Colors = ["danger", "danger", "success", "success", "warning", "warning", "primary", "primary"];
var IdColumn = "SetLeft"; var b;
var IdColumsChange = "SetRight";
var light = false;
var ClickedWord = [];
var colorTry = 8;
var submited = true;
var score = 0;


if (cuted == "0")
{
    var ranEl
    if (Model.length % 4 != 0)
    {
        do
        {
            ranEl = Math.round(Math.random() * (Model.length));
            Model.push(Model[ranEl]);
            ranEl = 0;
        } while (Model.length % 4 != 0);
    }
} // add objects for Test 10

Model.sort(function () {
    return Math.random() - 0.5;
});

var Test = [
    {
        "TestName": "10",
        "Name": "Test10",
        "Picture_count": 0,
        "Words_count": 4,
        "Pronounce_count": 0,
        "CheckValue": "set-of-pair",
    },
    {
     "TestName": "1",
     "Name": "Test01",
     "Picture_count": 2,
     "Words_count": 1,
     "Pronounce_count": 0,
     "CheckValue": "picture", 
    },
    {
     "TestName": "2",
     "Name": "Test02",
     "Picture_count": 4,
     "Words_count": 1,
     "Pronounce_count": 0,
     "CheckValue": "picture",  
    },
    {
        "TestName": "3",
        "Name": "Test03",
        "Picture_count": 1,
        "Words_count": 4,
        "Pronounce_count": 0,
        "CheckValue": "word",
    },
    {
        "TestName": "4",
        "Name": "Test04",
        "Picture_count": 4,
        "Words_count": 0,
        "Pronounce_count": 1,
        "CheckValue": "picture",
    },
    {
        "TestName": "5",
        "Name": "Test05",
        "Picture_count": 0,
        "Words_count": 0,
        "Pronounce_count": 0,
        "CheckValue": "picture",
    },
    {
        "TestName": "6",
        "Name": "Test06",
        "Picture_count": 1,
        "Words_count": 1,
        "Pronounce_count": 0,
        "CheckValue": "set",
    },
    {
        "TestName": "7",
        "Name": "Test07",
        "Picture_count": 0,
        "Words_count": 1,
        "Pronounce_count": 1,
        "CheckValue": "set",
    },
    {
        "TestName": "8",
        "Name": "Test08",
        "Picture_count": 0,
        "Words_count": 4,
        "Pronounce_count": 0,
        "CheckValue": "word",
        "word": "word-word"
    },
    {
        "TestName": "9",
        "Name": "Test09",
        "Picture_count": 0,
        "Words_count": 4,
        "Pronounce_count": 1,
        "CheckValue": "word",
    }
]

function ExceptionTest()
{
    document.getElementById('alert').innerHTML = `<div class="alert alert-danger alert-dismissible fade show" role="alert">
  <button type="button" class="close" data-dismiss="alert" aria-label="Close">
    <span aria-hidden="true">&times;</span>
  </button>
  <strong>Set ALLLLLLLL</strong>
</div>`;
}


function displayWord(i) {


    var position = Math.round(Math.random() * (Test[cuted].Words_count - 1));
    var idWord = new Array();
    idWord.push(i);

    for (var j = 0; j < Test[cuted].Words_count; j++) {
        do {
            var random = Math.round(Math.random() * (Model.length - 1));
        } while (idWord.includes(random));
        idWord.push(random); 

        if (position == j) {
            if (Test[cuted].CheckValue == "set")
            {
                var o = document.createElement('input');
                o.className = "col-md-3 form-control form-control-lg";
            }
            else
            {
                var o = document.createElement('div');
                o.className = "col-md-3";
            }
            if (Test[cuted].CheckValue == "word")
            {
                o.innerText = `${Model[i].native}`;
            }
            if (Test[cuted].CheckValue != "word" && Test[cuted].CheckValue !="set")
            {
                o.innerText = `${Model[i].translation}`;
            }

            
            o.name = `${Model[i].translation}`;

            o.id = `wordId${i}`;
            if (Test[cuted].CheckValue == "word") {
                o.onclick = function () { Submited(this.id) };
                o.onmouseover = function () { onmove(this.id) };
                o.onmouseout = function () { onout(this.id) };
            }
            else
            {
                o.style.alt = "selected";
            }

            document.getElementById('word').appendChild(o);
        }
        if (position != j) {


            var o = document.createElement('div');
            o.className = "col-md-3";



            if (Test[cuted].CheckValue == "word")
            {
                o.innerText = `${Model[random].native}`;
            }
            if (Test[cuted].CheckValue != "word" && Test[cuted].CheckValue != "set")
            {
                o.innerText = `${Model[random].translation}`;

            }
            o.name = `${Model[random].translation}`;




            o.id = `wordId${random}`;
            if (Test[cuted].CheckValue == "word")
            {
                o.onclick = function () { Submited(this.id) };
                o.onmouseover = function () { onmove(this.id) };
                o.onmouseout = function () { onout(this.id) };
            }
            else
            {
                o.style.alt = "selected";
            }
            document.getElementById('word').appendChild(o);
        }

    }

}

function displayWordforTest9 (i)
{
  

    var WordDiv = document.createElement('div');
    WordDiv.className = 'col-md-3';
    WordDiv.innerText = `${Model[i].translation}`;
    WordDiv.name = `${Model[i].translation}`;
    WordDiv.id = `word-wordId${i}`;

    document.getElementById('picture').appendChild(WordDiv);
}

function displayPicture(i) {


    var position = Math.round(Math.random() * (Test[cuted].Picture_count - 1));
    var idWord = new Array();
    idWord.push(i);
    for (var j = 0; j < Test[cuted].Picture_count; j++)
    {
        do {
            var random = Math.round(Math.random() * (Model.length - 1));
            
        } while (idWord.includes(random));
        idWord.push(random); 

        if (position == j) {
            var ImgDiv = document.createElement('div');
            ImgDiv.className = 'col-md-6';
            var o = document.createElement('img');
            o.className = "img-fluid mt-2 mx-auto d-block";
            o.style.objectFit = "cover";
            o.src = `../../${Model[i].picture}`;
            o.name = `${Model[i].translation}`;
            o.id = `imgId${i}`;
            
            if (Test[cuted].CheckValue == "picture")
            {
                o.onclick = function () { Submited(this.id) };
                o.onmouseover = function () { onmove(this.id) };
                o.onmouseout = function () { onout(this.id) };
            }
            else
            {
                o.style.alt = "selected";
            }

            ImgDiv.appendChild(o);
        }
        if (position != j)
        {
            var ImgDiv = document.createElement('div');
            ImgDiv.className = 'col-md-6';
            var o = document.createElement('img');
            o.className = "img-fluid mt-2 mx-auto d-block";
            o.style.objectFit = "cover";
            o.src = `../../${Model[random].picture}`;
            o.name = `${Model[random].translation}`;
            o.id = `imgId${random}`;

     

            if (Test[cuted].CheckValue == "picture")
            {
                o.onclick = function () { Submited(this.id) };
                o.onmouseover = function () { onmove(this.id) };
                o.onmouseout = function () { onout(this.id) };
            }
            else {
                o.style.alt = "selected";
            }
            ImgDiv.appendChild(o);
        }

        document.getElementById('picture').appendChild(ImgDiv);
    }
    
}

function displayPronounce(i) {
   

    var position = Math.round(Math.random() * (Test[cuted].Pronounce_count - 1));
    var idPronounce = new Array();
    idPronounce.push(i);
    for (var j = 0; j < Test[cuted].Pronounce_count; j++)
    {
        do
        {
            var random = Math.round(Math.random() * (Model.length - 1));
            
        } while (idPronounce.includes(random));
        idPronounce.push(random);

        if (position == j)
        {
            var PronounceDiv = document.createElement('div');
            PronounceDiv.className = 'col-md-4 flex';
            
            var o = document.createElement('audio');
            o.controls = "controls";
            var s = document.createElement('source');
            s.src = `../../${Model[i].pronounceLearn}`;
            PronounceDiv.id = `PronounceId${i}`;

           
            PronounceDiv.name = `${Model[i].translation}`;


            if (Test[cuted].CheckValue == "pronounce")
            {
                o.onclick = function () { Submited(this.id) };
                o.onmouseover = function () { onmove(this.id) };
                o.onmouseout = function () { onout(this.id) };
            }
            else {
                PronounceDiv.style.alt = "selected";
            }
            o.appendChild(s);
            PronounceDiv.appendChild(o);

        }
        if (position != j) {
            var PronounceDiv = document.createElement('div');
            PronounceDiv.className = 'col-md-3';
            var o = document.createElement('audio');
            o.controls = "controls";
            var s = document.createElement('source');
            s.src = `../../${Model[random].pronounceLearn}`;
            PronounceDiv.id = `PronounceId${random}`;

            PronounceDiv.name = `${Model[random].translation}`;
            

   

            if (Test[cuted].CheckValue == "pronounce") {
                o.onclick = function () { Submited(this.id) };
                o.onmouseover = function () { onmove(this.id) };
                o.onmouseout = function () { onout(this.id) };
            }
            else {
                PronounceDiv.style.alt = "selected";
            }
            o.appendChild(s);

            PronounceDiv.appendChild(o);

        }

        document.getElementById('pronounce').appendChild(PronounceDiv);
    }

}

function displayTest10(i)
{
    var RefreshButton = document.createElement('refresh');
    RefreshButton.onclick = function () { RefreshSelectedTest10() };
 
    var SetLeft = document.createElement('div');
    SetLeft.className = "col-md-6 ";
    SetLeft.id = "SetLeft";

    var SetRight = document.createElement('div');
    SetRight.className = "col-md-6 ";
    SetRight.id = "SetRight";

    var buf = [];

    for (var q=0; q < 4; q++)
    {
        i++;
        var DivLeft = document.createElement('div');
        DivLeft.className = "col-md-12 d-flex  justify-content-center "

        var LeftBtn = document.createElement('button');
        LeftBtn.className = "btn btn-light active my-2";
        LeftBtn.innerText = `${Model[i].translation}`;
        LeftBtn.style.alt = `${Model[i].translation}`;
        LeftBtn.id = `Leftbtn${i}`;
        LeftBtn.onclick = function () { SubmitedTest10Left(this.id) };
        LeftBtn.style.name = "light";
        LeftBtn.type = "button";

        DivLeft.appendChild(LeftBtn);
        SetLeft.appendChild(DivLeft);


        var DivRight = document.createElement('div'); //right set words
        DivRight.className = "col-md-12 d-flex  justify-content-center "

        var RightBtn = document.createElement('button');
        RightBtn.className = "btn btn-light disabled my-2";
        RightBtn.innerText = `${Model[i].native}`;
        RightBtn.style.alt = `${Model[i].translation}`;
        RightBtn.id = `Rightbtn${i}`;
       
        RightBtn.style.name = "light";
        RightBtn.type = "button";

        DivRight.appendChild(RightBtn);

        buf.push(DivRight);     
    }

    buf.sort(function () {
        return Math.random() - 0.5;
    });

    for (var q = 0; q < buf.length; q++)
    {
        SetRight.appendChild(buf[q]);
    }

    document.getElementById('word').appendChild(SetLeft);

    document.getElementById('word').appendChild(SetRight);

    return i;
}

function RefreshSelectedTest10()
{
    var SetLeft = document.getElementById(`SetLeft`).getElementsByClassName('btn');
    var SetRight = document.getElementById(`SetRight`).getElementsByClassName('btn');

    for (var r = 0; r < SetLeft.length; r++)
    {
        SetLeft[r].classList.remove("disabled");
        SetLeft[r].classList.add("active");
        SetLeft[r].onclick = function () { SubmitedTest10Left(this.id) }; 
        SetLeft[r].style.name = "light";

        SetLeft[r].classList.remove("btn-danger");
        SetLeft[r].classList.remove("btn-success");
        SetLeft[r].classList.remove("btn-warning");
        SetLeft[r].classList.remove("btn-primary");

        SetLeft[r].classList.add("btn-light");

    }

    for (var r = 0; r < SetRight.length; r++)
    {
        SetRight[r].classList.remove("active");
        SetRight[r].classList.add("disabled");
        SetRight[r].onclick = "";
        SetRight[r].style.name = "light";
        SetRight[r].classList.remove("btn-danger");
        SetRight[r].classList.remove("btn-success");
        SetRight[r].classList.remove("btn-warning");
        SetRight[r].classList.remove("btn-primary");
        SetRight[r].classList.add("btn-light");
    }
    colorTry = 0;
    ClickedWord = [];
    IdColumn = "SetLeft";
    IdColumsChange = "SetRight";
}

function SubmitedTest10Left(click_id)
{
    var buttons = document.getElementById(`${IdColumn}`).getElementsByClassName('btn'); 

    for (var r = 0; r < buttons.length; r++)
    {
        if (buttons[r].id != click_id) {

            buttons[r].classList.remove("active");
            buttons[r].classList.add("disabled");
            
        }
        buttons[r].onclick = "";
    }

    var element = document.getElementById(`${click_id}`);

    if (element.style.name == "light")
    { //color change

        element.classList.remove("btn-light");
        element.classList.add(`btn-${Colors[colorTry]}`);
        element.style.name = `${Colors[colorTry]}`;

        ClickedWord.push(element);
       
      

        light = false;

         var OtherColumn = document.getElementById(`${IdColumsChange}`).getElementsByClassName('btn');

        for (var r = 0; r < OtherColumn.length; r++)
        {
            OtherColumn[r].classList.remove("disabled");
            OtherColumn[r].classList.add("active");
            OtherColumn[r].onclick = function () { SubmitedTest10Left(this.id) };


           
            if (ClickedWord.includes(OtherColumn[r]))
            {
                OtherColumn[r].onclick = "";
            }
        }  
    }

    b = IdColumn;
    IdColumn = IdColumsChange;
    IdColumsChange = b;

    if (element.style.name == "light")
    {
        b = IdColumn;
        IdColumn = IdColumsChange;
        IdColumsChange = b;
    }

    colorTry++;
}

function CheckTrueTest10()
{
    var SetLeft = document.getElementById(`SetLeft`).getElementsByClassName('btn');
    var SetRight = document.getElementById(`SetRight`).getElementsByClassName('btn');

    for (var i = 0; i < 4; i++)
    {
        for (var j = 0; j < 4; j++)
        {
            if ((SetLeft[i].style.name == SetRight[j].style.name) && (SetLeft[i].style.alt == SetRight[j].style.alt))
            {
                Score();
            }
        }
    }

}

function onmove(click_id)
{
    if (document.getElementById(`${click_id}`).style.border == "")
    {
        document.getElementById(`${click_id}`).style.border = "thick solid #2EFE2E";
    }
}

function onout(click_id)
{
    if (document.getElementById(`${click_id}`).style.border == "thick solid rgb(46, 254, 46)")
    {
        document.getElementById(`${click_id}`).style.border = "";
    }
}

function Score()
{
    score = score + 1;  
}

function displayScore()
{
    if (cuted == "0")
    {
        document.getElementById('score').textContent = `Test number ${(i + 1) / 4}/${(Model.length) / 4} Your Score: ${score} / ${Model.length}`;
    }
    if (cuted != "0")
    {
        document.getElementById('score').textContent = `Test number ${i + 1}/${Model.length} Your Score: ${score} / ${Model.length}`;
    }
}

function CheckTrue(qwe)
{
    var objectWord = document.getElementById('word'); // check/select pictures 
    var elementsWord = objectWord.getElementsByClassName('col-md-3');
    var selectedWord ;
    var selectedWordValue;

    for (var i = 0; i < elementsWord.length; i++)// check/select Words
    {
        if (elementsWord[i].style.alt == "selected") {
            selectedWord = elementsWord[i].name;
            if (Test[cuted].CheckValue == "set")
            {
                selectedWordValue = elementsWord[i].value;
            }

        }
    }

    var objectImg = document.getElementById('picture'); // check/select pictures 
    var elementsImg = objectImg.getElementsByClassName('img-fluid');
    var selectedImg  ;

    for (var i = 0; i < elementsImg.length; i++)// check/select Words
    { 
        if (elementsImg[i].style.alt == "selected")
        {
           selectedImg = elementsImg[i].name; 
        }      
    }

    var objectPronounce = document.getElementById('pronounce'); // check/select pronounce 
    var elementsPronounce = objectPronounce.getElementsByClassName('col-md-4');
    var selectedPronounce;

    for (var i = 0; i < elementsPronounce.length; i++)// check/select Words
    {
        if (elementsPronounce[i].style.alt == "selected")
        {
            selectedPronounce = elementsPronounce[i].name;
        }
    }

    if (((selectedImg == selectedWord) || (selectedImg == selectedPronounce) || (selectedWord == selectedPronounce)) && ((Test[cuted].CheckValue != "set") && (Test[cuted].word != "word-word")))
    {
        Score();
    }

    if ((Test[cuted].CheckValue == "set") && ((selectedWordValue == selectedImg) || (selectedWordValue == selectedPronounce)))
    {
        Score(); 
    }




    if ((Test[cuted].word == "word-word") && (document.getElementById(`word-wordId${qwe}`).name == selectedWord))
    {
        Score();
    }
}

function Submited(click_id)
{
    var tagid;
    var clas;
    if (Test[cuted].CheckValue == "word")
     { tagid = "word"; clas = "col-md-3" }
    if (Test[cuted].CheckValue == "picture")
     { tagid = "picture"; clas = "img-fluid mt-2 mx-auto d-block" }

    var object = document.getElementById(`${tagid}`); // check/select pictures 
    var elements = object.getElementsByClassName(`${clas}`);

    for (var i = 0; i < elements.length; i++) {
        if (elements[i]) {
            elements[i].style.border = "";
            elements[i].style.alt = "";
        }
    }
    submited = true;
    document.getElementById(`${click_id}`).style.border = `thick solid green`;
    document.getElementById(`${click_id}`).style.alt = "selected";

}

function clearAll() {
    document.getElementById('word').innerHTML = "";
    document.getElementById('picture').innerHTML = "";
    document.getElementById('pronounce').innerHTML = "";
    document.getElementById('alert').innerHTML = "";
}

function TestFinish()
{
    clearAll();
    document.getElementById('score').innerText = `Congratulate!! Your Score is ${score}`;
    document.getElementById('next').style.visibility = "hidden";

    document.getElementById('finishTest').classList.remove("invisible");
    document.getElementById('finishTest').classList.add("visible");
}

function FinishTest()
{
    window.open(`${window.location.origin}/Home/GetTestResult?score=${score}&icon=${cuted}&lang_id=${Model[0].languageId}&cat_id=${Model[0].categoryId}`);
    document.getElementById('finishTest').classList.remove("visible");
    document.getElementById('finishTest').classList.add("invisible");
}

function next() {


    if ((submited == true) && (cuted != "0")) {

        if (i == -1) {
            displayScore();
        }

        if (cuted != "0") {
            i++;
        }

        if ((i > 0) && (cuted != "0") && (submited == true)) {
            CheckTrue(i - 1);
        }

        if ((cuted != "0") && (i < Model.length)) {
            displayScore();
        }

        if ((cuted != "0")&& (i < Model.length)) {
            clearAll();
            displayWord(i);
        }

        if ((Test[cuted].word == "word-word")&& (i < Model.length)) {
            displayWordforTest9(i);
        }
        if (i < Model.length)
        {
            displayPicture(i);
            displayPronounce(i);
        }
        
    }

    if ((submited == false)&&(cuted != "0")&&(i != 0))
    {
        
        ExceptionTest();
    }


    if (cuted == "0")
    {   
        document.getElementById('refresh').classList.remove("invisible");
        document.getElementById('refresh').classList.add("visible");

        if (colorTry == 8) { submited = true}

        

        if ((i > -1)&&(colorTry==8))
        {
            CheckTrueTest10();   
            
        }   

        if (colorTry == 8) {
            clearAll();
            i = displayTest10(i);
            colorTry = 0;
            ClickedWord = [];
        }   
        if (submited == false)
        {
           
            ExceptionTest();
        }
        displayScore();
    }


    if (i == Model.length)
    {
        TestFinish();
    }

    submited = false;


}







    






var i = -1;
var b;

Model.sort(function () {
    return Math.random() - 0.5;
});

var Test = [
    {},
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
    }]

TestNum = parseInt(cuted, 10);
var TestName = Test[TestNum];
var score = -1;

function displayWord(i) {

    var position = Math.round(Math.random() * (Test[cuted].Words_count - 1));

    for (var j = 0; j < Test[cuted].Words_count; j++) {
        do {
            var random = Math.round(Math.random() * (Model.length - 1));
        } while (random == i);

        if (position == j) {
 
            var o = document.createElement('div');
            o.className = "col-md-3";

            o.innerText = `${ Model[i].translation}`;
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

            o.innerText = `${Model[random].translation}`;
 
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

function displayPicture(i) {

    var position = Math.round(Math.random() * (Test[cuted].Picture_count-1));

    for (var j = 0; j < Test[cuted].Picture_count; j++)
    {
        do {
            var random = Math.round(Math.random() * (Model.length - 1));
        } while (random == i);

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


function Score() {
    score = score + 1;
    document.getElementById('score').textContent = `${score}`;
}

function CheckTrue()
{
    var objectWord = document.getElementById('word'); // check/select pictures 
    var elementsWord = objectWord.getElementsByClassName('col-md-3');
    var selectedWord;

    for (var i = 0; i < elementsWord.length; i++)// check/select Words
    {
        if (elementsWord[i].style.alt == "selected") {
            selectedWord = elementsWord[i].name;
        }
    }

    var objectImg = document.getElementById('picture'); // check/select pictures 
    var elementsImg = objectImg.getElementsByClassName('img-fluid');
    var selectedImg;

    for (var i = 0; i < elementsImg.length; i++)// check/select Words
    { 
        if (elementsImg[i].style.alt == "selected")
        {
           selectedImg = elementsImg[i].name; 
        }      
    }

    if (selectedImg == selectedWord)
    {
        Score();
    }

}

function Submited(click_id) {

    var tagid;
    var clas;
    if (Test[cuted].CheckValue == "word") { tagid = "word"; clas = "col-md-3" }
    if (Test[cuted].CheckValue == "picture") { tagid = "picture"; clas = "img-fluid mt-2 mx-auto d-block" }


    var object = document.getElementById(`${tagid}`); // check/select pictures 
    var elements = object.getElementsByClassName(`${clas}`);

    for (var i = 0; i < elements.length; i++) {
        if (elements[i]) {
            elements[i].style.border = "";
            elements[i].style.alt = "";
        }
    }

    document.getElementById(`${click_id}`).style.border = "thick solid #088A08";
    document.getElementById(`${click_id}`).style.alt = "selected";
}


function clearAll() {
    document.getElementById('word').innerHTML = "";
    document.getElementById('picture').innerHTML = "";
}

function next() {
    i++;


    CheckTrue();

    if (i > Model.length - 2) { document.getElementById('next').style.visibility = "hidden" }
    clearAll();
    displayWord(i);
    displayPicture(i);
}





    




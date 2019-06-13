
var info;
var testResult;

function getRandomInt(min, max) {
    return Math.floor(Math.random() * (max - min)) + min;
}

function shuffle(a) {
    for (let i = a.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [a[i], a[j]] = [a[j], a[i]];
    }
    return a;
}

function InitNewTest(categoriesArray, picturesCount, textCount, soundsCount) {

    shuffle(categoriesArray);
    info = new TestInfo(categoriesArray, picturesCount, textCount, soundsCount);
    testResult = new TestResult();
}

function NextTest() {

    if (info.currentIndex < info.categories.length) {
        LoadPictures();
        LoadText();
        info.indexIncrease();
    } else {
        GenerateResult(this.testResult);
    }
}

function LoadPictures() {

    const TEST_IMAGES = "test_images";
    var test_images = document.getElementsByClassName(TEST_IMAGES)[0];
    $("." + TEST_IMAGES).empty();

    var picturesInfo = this.info;

    var selectedCategories = new Array();

    var categoriesLength = picturesInfo.categories.length;
    selectedCategories[0] = picturesInfo.categories[info.currentIndex];
    let i = 1;
    while (i < picturesInfo.picturesCount) {
        let random = getRandomInt(0, categoriesLength);
        var category = picturesInfo.categories[random];
        if (!selectedCategories.includes(category)) {
            selectedCategories.push(category);
            i++;
        }       
    }  
    shuffle(selectedCategories);
    for (let j = 0; j < selectedCategories.length; j++) {
        var category = selectedCategories[j];
        var div = document.createElement('div');
        div.className = "col-md-6"
        var img = document.createElement('img');
        img.src = '../../' + category.picture;
        img.className = "img-fluid";
        img.alt = category.translation;
        img.onclick = function () { CheckPictureWithText(this) };
        div.appendChild(img);
        test_images.appendChild(div);
    }
}

function LoadText() {
    const TEST_WORD = "test_word";
    let test_word = document.getElementsByClassName(TEST_WORD)[0];
    $("." + TEST_WORD).empty();

    let textInfo = this.info;
    let indexes = new Array();
    indexes[0] = textInfo.currentIndex;

    let i = 1;
    while (i < textInfo.textsCount)
    {
        let randomTextIndex = getRandomInt(0, 2);
        if (!indexes.includes(randomTextIndex)) {
            indexes.push(randomTextIndex);
            i++;
        }
    }

    for (let j = 0; j < indexes.length; j++) {

        let text = textInfo.categories[indexes[j]].translation;
        let h3 = document.createElement("h3");
        let textNode = document.createTextNode(text);
        h3.appendChild(textNode);
        test_word.appendChild(h3);
    }



}

function CheckPictureWithText(picture) {
    const TEST_WORD = "test_word";
    let test_word_div = document.getElementsByClassName(TEST_WORD)[0];
    let word = test_word_div.childNodes[0].innerText;

    if (picture.alt == word) {
        this.info.increaseScore();
        this.testResult.QuestionNames.push(word);
        this.testResult.QuestionResults.push(true);
        NextTest();
    } else {
        this.testResult.QuestionNames.push(word);
        this.testResult.QuestionResults.push(false);
        NextTest();
    }
}

function GenerateResult(result) {
    
    if (result != null) {
        const TEST = "test";
        const TEST_RESULT = "test_result";
        let testResult_div = document.getElementsByClassName(TEST_RESULT)[0];
        let test_div = document.getElementsByClassName(TEST)[0];
        let names = result.QuestionNames;
        let testResults = result.QuestionResults;


        console.log(test_div);
        console.log(testResult_div);
        test_div.style.display = "none";
        testResult_div.setAttribute("style", "display: block;");

        let table = document.createElement('table');

        for (let i = 0; i < result.GetLength(); i++) {
            let tr = document.createElement('tr');
            let NameTextNode = document.createTextNode(result.QuestionNames[i]);
            let ResultTextNode = document.createTextNode(result.QuestionResults[i]);

            let tdName = document.createElement('td');
            let tdResult = document.createElement('td');
            tdName.appendChild(NameTextNode);
            tdResult.appendChild(ResultTextNode);

            tr.appendChild(tdName);
            tr.appendChild(tdResult);
            table.appendChild(tr);
        }

        let trTotal = document.createElement('tr');
        let total = result.getTotal();
        trTotal.innerHTML = "<td>Total</td>";
        trTotal.innerHTML += "<td>" + total + "</td>";
        table.appendChild(trTotal);

        testResult_div.appendChild(table);

    } else {

        return;
    }
}

class TestInfo {

    currentIndex;
    categories;
    picturesCount;
    textsCount;
    soundsCount;
    currentScore;

    constructor(array, pictures, texts, sounds) {
        this.currentIndex = 0;
        this.currentScore = 0;
        this.categories = array;
        this.picturesCount = pictures;
        this.textsCount = texts;
        this.soundsCount = sounds;
    }

    indexIncrease = () => {
        this.currentIndex = this.currentIndex + 1;
    }

    increaseScore = () => {
        this.currentScore = this.currentScore + 1;
    }
}

class TestResult {
    QuestionNames;
    QuestionResults;

    constructor() {
        this.QuestionNames = new Array();
        this.QuestionResults = new Array();
    }

    GetLength = () => {
        return this.QuestionNames.length;
    }

    getTotal = () => {
        let sum = 0;
        for (let item in this.QuestionResults) {
            if (item) { sum++; }
        }
        return sum;
    }
}

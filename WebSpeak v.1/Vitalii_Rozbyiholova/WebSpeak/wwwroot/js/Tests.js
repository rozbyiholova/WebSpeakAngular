
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
        LoadSounds();
        info.indexIncrease();
    } else {
        GenerateResult(testResult);
    }
}

function LoadPictures() {

    let picturesInfo = this.info;
    if (picturesInfo.picturesCount < 1) { return; }

    const TEST_IMAGES = "test_images";
    var test_images = document.getElementsByClassName(TEST_IMAGES)[0];
    $("." + TEST_IMAGES).empty();
    
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

        var radio = document.createElement('input');
        radio.type = "radio";
        let IdText = "rd" + j;
        radio.id = IdText
        var label = document.createElement('label');
        label.for = IdText;

        var img = document.createElement('img');
        img.src = '../../' + category.picture;
        img.className = "img-fluid";
        img.alt = category.translation;

        label.appendChild(img);
        test_images.appendChild(radio);
        test_images.appendChild(label);

        $('.' + TEST_IMAGES + ' input:radio').addClass('input_hidden');
        $('.' + TEST_IMAGES + ' label').click(function () {
            $(this).addClass('selected').siblings().removeClass('selected');
        });
    }
}

function LoadText() {
    let textInfo = info;
    if (textInfo.textsCount < 1) { return; }

    const TEST_WORD = "test_word";
    let test_word = document.getElementsByClassName(TEST_WORD)[0];
    $("." + TEST_WORD).empty();

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

    shuffle(indexes);
    for (let j = 0; j < indexes.length; j++) {

        let text = textInfo.categories[indexes[j]].translation;
        let h3 = document.createElement("h3");
        let textNode = document.createTextNode(text);
        h3.appendChild(textNode);
        test_word.appendChild(h3);
    }
}

function LoadSounds() {
    let soundsInfo = this.info;
    let count = soundsInfo.soundsCount;
    if (count < 1 || count > 4) { return; }

    const TEST_SOUNDS = "test_sounds";
    let sounds = document.querySelector('.' + TEST_SOUNDS);
    $('.' + TEST_SOUNDS).empty();

    let indexes = new Array();
    indexes[0] = soundsInfo.currentIndex;

    let i = 1;
    while (i < count) {
        let randomIndex = getRandomInt(0, 2);
        if (!indexes.includes(randomIndex)) {
            indexes.push(randomIndex);
            i++;
        }
    }

    shuffle(indexes);
    for (let k = 0; k < indexes.length; k++) {

        console.log(soundsInfo.categories[indexes[k]]);
        let pronounce = soundsInfo.categories[indexes[k]].translationPronounce;
        let html = '<audio controls="controls" src="../../' + pronounce + '" type="audio/mpeg">';
        sounds.innerHTML += html;
    }
} //do checking

function GenerateResult(result) {
     
    if (result != null) {
        const TEST = "test";
        const TEST_RESULT = "test_result";
        let testResult_div = document.getElementsByClassName(TEST_RESULT)[0];
        let test_div = document.getElementsByClassName(TEST)[0];
        let names = result.QuestionNames;
        let testResults = result.QuestionResults;

        test_div.style.display = "none";
        testResult_div.setAttribute("style", "display: block;");

        let table = document.createElement('table');

        for (let i = 0; i < result.GetLength(); i++) {
            let tr = document.createElement('tr');
            let NameTextNode = document.createTextNode(names[i]);
            let ResultTextNode = document.createTextNode(testResults[i]);

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

function CheckPictureWithText() {

    const TEST_WORD = "test_word";
    let test_word_div = document.querySelector('.' + TEST_WORD);
    let word = test_word_div.childNodes[0].innerText;
    let selectedLabel = document.querySelector('.selected');

    if (selectedLabel != undefined && word != undefined) {
    let picture = selectedLabel.childNodes[0];
        if (picture.alt == word) {
            info.increaseScore();
            testResult.QuestionNames.push(word);
            testResult.QuestionResults.push(true);
            selectedLabel.classList.remove('selected');
            NextTest();
        } else {
            testResult.QuestionNames.push(word);
            testResult.QuestionResults.push(false);
            selectedLabel.classList.remove('selected');
            NextTest();
        }
    } else {
        alert("Nothing selected. Please select any item");
    }    
}

function CheckPictureWithSound() {
    const TEST_SOUND = "test_sound";
    let test_word_div = document.querySelector('.' + TEST_WORD);
    let word = test_word_div.childNodes[0].innerText;
    let selectedLabel = document.querySelector('.selected');

    if (selectedLabel != undefined && word != undefined) {
        let picture = selectedLabel.childNodes[0];
        if (picture.alt == word) {
            info.increaseScore();
            testResult.QuestionNames.push(word);
            testResult.QuestionResults.push(true);
            selectedLabel.classList.remove('selected');
            NextTest();
        } else {
            testResult.QuestionNames.push(word);
            testResult.QuestionResults.push(false);
            selectedLabel.classList.remove('selected');
            NextTest();
        }
    } else {
        alert("Nothing selected. Please select any item");
    }    
}

class TestInfo {

    currentIndex;
    categories;
    picturesCount;
    textsCount;
    soundsCount;
    currentScore;
    checkMethod;

    constructor(array, pictures, texts, sounds) {
        this.currentIndex = 0;
        this.currentScore = 0;
        this.categories = array;
        this.picturesCount = pictures;
        this.textsCount = texts;
        this.soundsCount = sounds;

        if (this.picturesCount > 1 && this.textsCount > 0) {
            this.checkMethod = CheckPictureWithText;
        } else if (this.picturesCount > 1 && this.soundsCount > 0) {
            this.checkMethod = 
        }

        let button = document.querySelector('.confirm');
        button.onclick = this.checkMethod;
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
        for (let i = 0; i < this.GetLength(); i++) {
            if (this.QuestionResults[i]) { sum++;}
        }
        return sum;
    }
}


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
        if (info.picturesCount == 1 && info.textsCount == 1) {
            LoadPictures();
            LoadRandomText();
        } else {
            LoadPictures();
            LoadText();
            LoadSounds();
        }
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
            $(this).addClass('selected_picture').siblings().removeClass('selected_picture');
        });

        console.log(img.alt);
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
    let textCount = textInfo.textsCount;
    while (i < textCount)
    {
        let randomTextIndex = getRandomInt(0, textCount + 1);
        if (!indexes.includes(randomTextIndex)) {
            indexes.push(randomTextIndex);
            i++;
        }
    }

    shuffle(indexes);
    for (let j = 0; j < indexes.length; j++) {

        var radio = document.createElement('input');
        radio.type = "radio";
        let IdText = "rd" + j;
        radio.id = IdText
        var label = document.createElement('label');
        label.for = IdText;

        let text = textInfo.categories[indexes[j]].translation;
        let h3 = document.createElement("h3");
        let textNode = document.createTextNode(text);
        h3.appendChild(textNode);
        label.appendChild(h3);
        test_word.appendChild(radio);
        test_word.appendChild(label);

        $('.' + TEST_WORD + ' input:radio').addClass('input_hidden');
        $('.' + TEST_WORD + ' label').click(function () {
            $(this).addClass('selected_text').siblings().removeClass('selected_text');
        });
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

        let pronounce = soundsInfo.categories[indexes[k]].translationPronounce;
        let translation = soundsInfo.categories[indexes[k]].translation;
        let audio = document.createElement('audio');
        audio.controls = "controls";
        audio.src = `../../${pronounce}`;
        audio.type = "audio/mpeg";
        audio.style.alt = translation;
        sounds.appendChild(audio);
    }
}

function LoadRandomText() {
    let randomTextInfo = info;

    const TEST_WORD = "test_word";
    let test_word = document.getElementsByClassName(TEST_WORD)[0];
    $("." + TEST_WORD).empty();

    let current = randomTextInfo.currentIndex;
    let startIndex = current - 2;
    let endIndex;

    //do not let indexes be out of the array
    if (startIndex < 0) {
        startIndex = 0;
        endIndex = 6 - current;
    } else {
        endIndex = current + 3;
    }

    let length = randomTextInfo.categories.length;
    if (endIndex >= length) {
        startIndex = length - 5
        endIndex = length;
    }

    var radio = document.createElement('input');
    radio.type = "radio";
    let IdText = "rd" + current;
    radio.id = IdText
    var label = document.createElement('label');
    label.for = IdText;

    let randomIndex = getRandomInt(startIndex, endIndex);
    let text = randomTextInfo.categories[randomIndex].translation;
    let h3 = document.createElement("h3");
    let textNode = document.createTextNode(text);
    h3.appendChild(textNode);
    label.appendChild(h3);
    test_word.appendChild(radio);
    test_word.appendChild(label);

    $('.' + TEST_WORD + ' input:radio').addClass('input_hidden');
    $('.' + TEST_WORD + ' label').click(function () {
        $(this).addClass('selected_text').siblings().removeClass('selected_text');
    });

    console.log(text);
}

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
    let label = test_word_div.getElementsByTagName('label')[0];    
    let word = label.childNodes[0].innerText;
    let selectedLabel = document.querySelector('.selected_picture');

    if (selectedLabel != undefined && word != undefined) {
    let picture = selectedLabel.childNodes[0];
        if (picture.alt == word) {
            info.increaseScore();
            testResult.QuestionNames.push(word);
            testResult.QuestionResults.push(true);
            selectedLabel.classList.remove('selected_picture');
            NextTest();
        } else {
            testResult.QuestionNames.push(word);
            testResult.QuestionResults.push(false);
            selectedLabel.classList.remove('selected_picture');
            NextTest();
        }
    } else {
        alert("Nothing selected. Please select any item");
    }    
}

function CheckPictureWithSound(){
    const TEST_SOUND = "test_sounds";
    let test_sound_div = document.getElementsByClassName(TEST_SOUND)[0];
    let alt = test_sound_div.childNodes[0].style.alt;
    let selectedLabel = document.querySelector('.selected_picture');

    if (selectedLabel != undefined && alt != undefined) {
        let picture = selectedLabel.childNodes[0];
        if (picture.alt == alt) {
            info.increaseScore();
            testResult.QuestionNames.push(alt);
            testResult.QuestionResults.push(true);
            selectedLabel.classList.remove('selected_picture');
            NextTest();
        } else {
            testResult.QuestionNames.push(alt);
            testResult.QuestionResults.push(false);
            selectedLabel.classList.remove('selected_picture');
            NextTest();
        }
    } else {
        alert("Nothing selected. Please select any item");
    }    
}

function CheckTextWithPicture() {

    const TEST_IMAGES = "test_images";
    let test_picture_div = document.getElementsByClassName(TEST_IMAGES)[0];
    let label = test_picture_div.getElementsByTagName('label')[0];
    let pictureText = label.childNodes[0].alt;
    let selectedLabel = document.querySelector('.selected_text');

    if (selectedLabel != undefined && pictureText != undefined) {
        let text = selectedLabel.childNodes[0].innerText;
        if (pictureText == text) {
            info.increaseScore();
            testResult.QuestionNames.push(text);
            testResult.QuestionResults.push(true);
            selectedLabel.classList.remove('selected_picture');
            NextTest();
        } else {
            testResult.QuestionNames.push(text);
            testResult.QuestionResults.push(false);
            selectedLabel.classList.remove('selected_picture');
            NextTest();
        }
    } else {
        alert("Nothing selected. Please select any item");
    }  
}

function CheckPictureWithInput() {

    const TEST_INPUT = "test_input";
    const TEST_IMAGE = "test_images"

    let test_input_div = document.querySelector('.' + TEST_INPUT);
    let input = test_input_div.getElementsByTagName('input')[0];
    let value = input.value;
    let test_picture_div = document.querySelector('.' + TEST_IMAGE);
    let label = test_picture_div.getElementsByTagName('label')[0];
    let word = label.childNodes[0].alt;

    if (value != undefined && word != undefined) {
        if (value == word) {
            info.increaseScore();
            testResult.QuestionNames.push(word);
            testResult.QuestionResults.push(true);
            label.classList.remove('selected_picture');                
            NextTest();
            input.value = "";
        } else {
            testResult.QuestionNames.push(word);
            testResult.QuestionResults.push(false);
            label.classList.remove('selected_picture');  
            NextTest();
            input.value = "";
        }
    } else {
        alert("Input is empty. Please type your answear");
    } 
}

function CheckSoundWithInput() {
    const TEST_INPUT = "test_input";
    const TEST_SOUND = "test_sounds"

    let test_input_div = document.querySelector('.' + TEST_INPUT);
    let input = test_input_div.getElementsByTagName('input')[0];
    let value = input.value;
    let test_sound_div = document.querySelector('.' + TEST_SOUND);
    let word = test_sound_div.childNodes[0].style.alt;

    if (input != undefined && word != undefined) {
        if (value == word) {
            info.increaseScore();
            testResult.QuestionNames.push(word);
            testResult.QuestionResults.push(true);
            NextTest();
            input.value = "";
        } else {
            testResult.QuestionNames.push(word);
            testResult.QuestionResults.push(false);
            NextTest();
            input.value = "";
        }
    } else {
        alert("Input is empty. Please type your answear");
    } 
}

function CheckSoundWithText() {
    const TEST_SOUND = "test_sounds"
    
    let test_sound_div = document.querySelector('.' + TEST_SOUND);
    let word = test_sound_div.childNodes[0].style.alt;

    let selectedLabel = document.querySelector('.selected_text')

    if (selectedLabel != undefined && word != undefined) {
        let text = selectedLabel.childNodes[0].innerText;

        if (text == word) {
            info.increaseScore();
            testResult.QuestionNames.push(word);
            testResult.QuestionResults.push(true);
            selectedLabel.classList.remove('selected_text');
            NextTest();
        } else {
            testResult.QuestionNames.push(word);
            testResult.QuestionResults.push(false);
            selectedLabel.classList.remove('selected_text');
            NextTest();
        }
    } else {
        alert("Nothing selected. Please select any item");
    } 
}

//do checking for 5 test
//change result table:   "true" -> "correct"
//                      "false" -> "uncorrect"

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
            this.checkMethod = CheckPictureWithSound;
        } else if (this.textsCount > 1 && this.picturesCount > 0) {
            this.checkMethod = CheckTextWithPicture;
        } else if (this.picturesCount == 1 && this.textsCount < 1 && this.soundsCount < 1) {
            this.checkMethod = CheckPictureWithInput;
        } else if (this.picturesCount < 1 && this.textsCount < 1 && this.soundsCount > 0) {
            this.checkMethod = CheckSoundWithInput;
        } else if (this.textsCount > 3 && this.soundsCount == 1) {
            this.checkMethod = CheckSoundWithText;
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

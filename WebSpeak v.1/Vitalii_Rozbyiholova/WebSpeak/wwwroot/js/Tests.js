const TEST10 = "test10";
const TEST_SOUND = "test_sounds";
const TEST_IMAGES = "test_images";
const TEST_WORD = "test_word";
const TEST_INPUT = "test_input";
const TEST_FOREIGN = "test_foreign";
const FOREIGN = "foreign_words";
const NATIVE = "native_words";
const INTERMEDIATE = "intermediate";
const SELECTED_F_TEXT = "selected_f_text";
const SELECTED_N_TEXT = "selected_n_text"
const SELECTED_TEXT = "selected_text";
const SELECTED_PICTURE = "selected_picture";

var info;
var testResult;
let TestResults10;

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
        } else if (info.textsCount == 5) {
            LoadNativeText();
        } else if (info.textsCount == 0 && info.soundsCount == 0 && info.picturesCount == 0) {
            LoadPairs();
        } else {
            LoadPictures();
            LoadText();
            LoadSounds();
        }
        info.indexIncrease();
    }
    else
    {
        GenerateResult(testResult);
    }
}

function LoadPictures() {

    let picturesInfo = this.info;
    if (picturesInfo.picturesCount < 1) { return; }

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
            $(this).addClass(SELECTED_PICTURE).siblings().removeClass(SELECTED_PICTURE);
        });
    }
}

function LoadText() {
    let textInfo = info;
    if (textInfo.textsCount < 1) { return; }

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
            $(this).addClass(SELECTED_TEXT).siblings().removeClass(SELECTED_TEXT);
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
    let startIndex = current;
    let endIndex = current + 2;

    //do not let indexes be out of the array
    let length = randomTextInfo.categories.length;
    if (endIndex > length) {
        startIndex = current - 2;
        endIndex = current;
    }

    let randomIndex = getRandomInt(startIndex, endIndex);
    let text = randomTextInfo.categories[randomIndex].translation;
    let h3 = document.createElement("h3");
    let textNode = document.createTextNode(text);

    h3.appendChild(textNode);
    test_word.appendChild(h3);

    $('.btn-group button').click(function () {
        $(this).addClass('clicked').siblings().removeClass('clicked');
    });
}

function LoadNativeText() {
    let nativeTextInfo = info;

    let test_foreign_word = document.getElementsByClassName(TEST_FOREIGN)[0];
    let test_native_word = document.getElementsByClassName(TEST_WORD)[0];
    $("." + TEST_WORD).empty();
    $("." + TEST_FOREIGN).empty();

    let current = nativeTextInfo.currentIndex;
    let foreign_text = nativeTextInfo.categories[current].translation;
    let f_h3 = document.createElement("h3");
    let f_textNode = document.createTextNode(foreign_text);
    f_h3.appendChild(f_textNode);
    test_foreign_word.appendChild(f_h3);

    let indexes = new Array();
    indexes[0] = nativeTextInfo.currentIndex;

    let i = 1;
    let textCount = nativeTextInfo.textsCount - 1;
    while (i < textCount) {
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

        let text = nativeTextInfo.categories[indexes[j]].native;
        let h3 = document.createElement("h3");
        let textNode = document.createTextNode(text);
        h3.appendChild(textNode);
        label.style.alt = nativeTextInfo.categories[indexes[j]].translation;
        label.appendChild(h3);
        test_native_word.appendChild(radio);
        test_native_word.appendChild(label);

        $('.' + TEST_WORD + ' input:radio').addClass('input_hidden');
        $('.' + TEST_WORD + ' label').click(function () {
            $(this).addClass(SELECTED_TEXT).siblings().removeClass(SELECTED_TEXT);
        });
    }
}

function LoadPairs() {
    TestResults10 = new TestResult();

    $("." + FOREIGN).empty();
    $("." + NATIVE).empty();
    $("." + INTERMEDIATE).empty();
    

    let pairsInfo = info;

    let foreign_div = document.getElementsByClassName(FOREIGN)[0];
    let native_div = document.getElementsByClassName(NATIVE)[0];

    //to prevent multicast event 
    document.querySelector('.check').removeEventListener('click', callMakePairs);
    document.querySelector('.confirm').removeEventListener('click', checkLength);

    let wordsNumber = 4;
    let current = pairsInfo.currentIndex;
    
    let categories = pairsInfo.categories.slice(current, current + wordsNumber);
    let native_words_array = new Array();
    let foreign_words_array = new Array();
    for (let k = 0; k < categories.length; k++) {

        //push entire category to be able to get word translation when compares selected words
        native_words_array.push(categories[k]);
        foreign_words_array.push(categories[k].translation);
        info.indexIncrease();
    }

    shuffle(native_words_array);
    shuffle(foreign_words_array);

    for (let j = 0; j < native_words_array.length; j++) {
        //translation words
        let t_radio = document.createElement('input');
        t_radio.type = "radio";
        let t_IdText = "f_rd" + j;
        t_radio.id = t_IdText
        var t_label = document.createElement('label');
        t_label.for = t_IdText;

        let t_text = foreign_words_array[j];
        let t_h3 = document.createElement("h3");
        let t_textNode = document.createTextNode(t_text);
        t_h3.appendChild(t_textNode);
        t_label.appendChild(t_h3);
        foreign_div.appendChild(t_radio);
        foreign_div.appendChild(t_label);

        $('.' + FOREIGN + ' input:radio').addClass('input_hidden');
        $('.' + FOREIGN + ' label').click(function () {
            $(this).addClass(SELECTED_F_TEXT).siblings().removeClass(SELECTED_F_TEXT);
        });

        //native words
        let n_radio = document.createElement('input');
        n_radio.type = "radio";
        let n_IdText = "n_rd" + j;
        n_radio.id = n_IdText
        var n_label = document.createElement('label');
        n_label.for = n_IdText;

        let n_text = native_words_array[j].native;
        let n_h3 = document.createElement("h3");
        let n_textNode = document.createTextNode(n_text);
        n_h3.appendChild(n_textNode);
        n_label.appendChild(n_h3);
        n_label.style.alt = native_words_array[j].translation;
        native_div.appendChild(n_radio);
        native_div.appendChild(n_label);

        $('.' + NATIVE + ' input:radio').addClass('input_hidden');
        $('.' + NATIVE + ' label').click(function () {
            $(this).addClass(SELECTED_N_TEXT).siblings().removeClass(SELECTED_N_TEXT);
        });
    }

    document.querySelector('.check').addEventListener('click', callMakePairs);
    document.querySelector('.confirm').addEventListener('click', checkLength);

    //because NextTest() increases index
    info.currentIndex--;
}

function makePair() {
    $("." + INTERMEDIATE).empty();

    let intermediate = document.getElementsByClassName(INTERMEDIATE)[0];
    let native_label = document.querySelector('.' + SELECTED_N_TEXT);
    let trans_label = document.querySelector('.' + SELECTED_F_TEXT);

    if (native_label != null && trans_label != null) {
        
        let trans_word = trans_label.childNodes[0].innerText;
        let native_word = native_label.style.alt;

        let text = `${trans_word} — ${native_label.childNodes[0].innerText}`;
        let comparison = native_word == trans_word;
        if (comparison) {
            info.increaseScore();
            TestResults10.QuestionNames.push(text);
            TestResults10.QuestionResults.push("correct");
            testResult.QuestionNames.push(text);
            testResult.QuestionResults.push("correct");
            native_label.classList.remove(SELECTED_N_TEXT);
            trans_label.classList.remove(SELECTED_F_TEXT);
        } else {
            TestResults10.QuestionNames.push(text);
            TestResults10.QuestionResults.push("uncorrect");
            testResult.QuestionNames.push(text);
            testResult.QuestionResults.push("uncorrect");
            native_label.classList.remove(SELECTED_N_TEXT);
            trans_label.classList.remove(SELECTED_F_TEXT);
        }

        native_label.innerHTML = "";
        trans_label.innerHTML = "";

        let table = document.createElement('table');
        for (let i = 0; i < TestResults10.GetLength(); i++) {
            let tr = document.createElement('tr');
            let NameTextNode = document.createTextNode(TestResults10.QuestionNames[i]);
            let ResultTextNode = document.createTextNode(TestResults10.QuestionResults[i]);


            let tdName = document.createElement('td');
            let tdResult = document.createElement('td');
            tdName.appendChild(NameTextNode);
            tdResult.appendChild(ResultTextNode);

            tr.appendChild(tdName);
            tr.appendChild(tdResult);
            table.appendChild(tr);
        }
        intermediate.appendChild(table);
    } else {
        alert('Nothing selected');
    }    
}

function checkLength() {

    let native_div = document.querySelector("." + NATIVE);

    if (info.currentIndex + 1 >= info.categories.length) {
        GenerateResult(testResult);
    } else {
        if (TestResults10.QuestionNames.length == native_div.childNodes.length / 2) {
            NextTest();
        } else {
            alert("Make all the pairs");
        }
    }
}

function callMakePairs() {
    makePair();
}

function GenerateResult(result) {
     
    if (result != null) {

        
        const TEST = "test";
        const TEST_RESULT = "test_result";
        let testResult_div = document.getElementsByClassName(TEST_RESULT)[0];
        let test_div = document.getElementsByClassName(TEST)[0];
        let names = result.QuestionNames;
        let testResults = result.QuestionResults;
        if (test_div == undefined) {
            test_div = document.querySelector('.test10');
            document.querySelector('.intermediate').innerHTML = "";
        } 

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

    let test_word_div = document.querySelector('.' + TEST_WORD);
    let label = test_word_div.getElementsByTagName('label')[0];    
    let word = label.childNodes[0].innerText;
    let selectedLabel = document.querySelector('.selected_picture');

    if (selectedLabel != undefined && word != undefined) {
    let picture = selectedLabel.childNodes[0];
        if (picture.alt == word) {
            selectedLabel.classList.remove('selected_picture');
            testResult.correctAnswear(word);
        } else {
            selectedLabel.classList.remove('selected_picture');
            testResult.uncorrectAnswear(word);
        }
    } else {
        alert("Nothing selected. Please select any item");
    }    
}

function CheckPictureWithSound(){
    let test_sound_div = document.getElementsByClassName(TEST_SOUND)[0];
    let alt = test_sound_div.childNodes[0].style.alt;
    let selectedLabel = document.querySelector('.selected_picture');

    if (selectedLabel != undefined && alt != undefined) {
        let picture = selectedLabel.childNodes[0];
        if (picture.alt == alt) {
            selectedLabel.classList.remove('selected_picture');
            testResult.correctAnswear(alt);
        } else {
            selectedLabel.classList.remove('selected_picture');
            testResult.uncorrectAnswear(alt);
        }
    } else {
        alert("Nothing selected. Please select any item");
    }    
}

function CheckTextWithPicture() {

    let test_picture_div = document.getElementsByClassName(TEST_IMAGES)[0];
    let label = test_picture_div.getElementsByTagName('label')[0];
    let pictureText = label.childNodes[0].alt;
    let selectedLabel = document.querySelector('.selected_text');

    if (selectedLabel != undefined && pictureText != undefined) {
        let text = selectedLabel.childNodes[0].innerText;
        if (pictureText == text) {
            selectedLabel.classList.remove('selected_picture');
            testResult.correctAnswear(text);
        } else {
            selectedLabel.classList.remove('selected_picture');
            testResult.uncorrectAnswear(text);
        }
    } else {
        alert("Nothing selected. Please select any item");
    }  
}

function CheckPictureWithInput() {

    let test_input_div = document.querySelector('.' + TEST_INPUT);
    let input = test_input_div.getElementsByTagName('input')[0];
    let value = input.value;
    let test_picture_div = document.querySelector('.' + TEST_IMAGE);
    let label = test_picture_div.getElementsByTagName('label')[0];
    let word = label.childNodes[0].alt;

    if (value != undefined && word != undefined) {
        if (value == word) {
            label.classList.remove('selected_picture');
            testResult.correctAnswear(word);
            input.value = "";
        } else {
            label.classList.remove('selected_picture');
            testResult.uncorrectAnswear(word);
            input.value = "";
        }
    } else {
        alert("Input is empty. Please type your answear");
    } 
}

function CheckSoundWithInput() {

    let test_input_div = document.querySelector('.' + TEST_INPUT);
    let input = test_input_div.getElementsByTagName('input')[0];
    let value = input.value;
    let test_sound_div = document.querySelector('.' + TEST_SOUND);
    let word = test_sound_div.childNodes[0].style.alt;

    if (input != undefined && word != undefined) {
        if (value == word) {
            testResult.correctAnswear(word);
            input.value = "";
        } else {
            testResult.uncorrectAnswear(word);
            input.value = "";
        }
    } else {
        alert("Input is empty. Please type your answear");
    } 
}

function CheckSoundWithText() {    
    
    let test_sound_div = document.querySelector('.' + TEST_SOUND);
    let word = test_sound_div.childNodes[0].style.alt;

    let selectedLabel = document.querySelector('.selected_text')

    if (selectedLabel != undefined && word != undefined) {
        let text = selectedLabel.childNodes[0].innerText;

        if (text == word) {
            selectedLabel.classList.remove('selected_text');
            testResult.correctAnswear(word);
        } else {
            selectedLabel.classList.remove('selected_text');
            testResult.uncorrectAnswear(word);
        }
    } else {
        alert("Nothing selected. Please select any item");
    } 
}

function CheckTrueOrFalse() {
    $(document).ready(function () {

        let btn_div = document.getElementsByClassName('buttons')[0];
        let button_yes = btn_div.childNodes[1];
        let button_no = btn_div.childNodes[3];

        let picture_div = document.querySelector('.' + TEST_IMAGES);
        let picture = picture_div.getElementsByTagName('label')[0].childNodes[0];
        let text = picture.alt;
        let text_div = document.getElementsByClassName(TEST_WORD)[0];
        let word = text_div.childNodes[0].innerText;

        if (picture != undefined && word != undefined) {

            let checkText = text == word;
            let yes_pressed = button_yes.classList.contains('clicked');
            let no_pressed = button_no.classList.contains('clicked');

            if ((checkText && yes_pressed) || (!checkText && no_pressed)) {
                testResult.correctAnswear(text);
            } else {
                testResult.uncorrectAnswear(text);
            }
        }

    });
    
}

function CheckTranslationWithNative() {

    let translation_word_div = document.querySelector('.' + TEST_FOREIGN);
    let word = translation_word_div.childNodes[0].innerText;
    let selectedLabel = document.querySelector('.selected_text');

    if (selectedLabel != undefined && word != undefined) {
        let native_word = selectedLabel.style.alt;
        if (native_word == word) {
            selectedLabel.classList.remove('selected_picture');
            testResult.correctAnswear(word);
        } else {
            selectedLabel.classList.remove('selected_picture');
            testResult.uncorrectAnswear(word);
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
            this.checkMethod = CheckPictureWithSound;
        } else if (this.textsCount > 1 && this.picturesCount > 0) {
            this.checkMethod = CheckTextWithPicture;
        } else if (this.picturesCount == 1 && this.textsCount < 1 && this.soundsCount < 1) {
            this.checkMethod = CheckPictureWithInput;
        } else if (this.picturesCount < 1 && this.textsCount < 1 && this.soundsCount > 0) {
            this.checkMethod = CheckSoundWithInput;
        } else if (this.textsCount > 3 && this.soundsCount == 1) {
            this.checkMethod = CheckSoundWithText;
        } else if (this.textsCount == 1 && this.picturesCount == 1) {
            this.checkMethod = CheckTrueOrFalse;
        } else if (this.textsCount == 5) {
            this.checkMethod = CheckTranslationWithNative;
        }

        let button = document.querySelector('.confirm');
        let button_yes, button_no;
        if (button != null) {
            button.onclick = this.checkMethod;
        } else {
            button_yes = document.querySelector('.button_yes');
            button_no = document.querySelector('.button_no');
            button_yes.onclick = this.checkMethod;
            button_no.onclick = this.checkMethod;
        }
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
            if (this.QuestionResults[i] == "correct") { sum++;}
        }
        return sum;
    }

    correctAnswear = (word) => {
        info.increaseScore();
        this.QuestionNames.push(word);
        this.QuestionResults.push("correct");
        NextTest();
    }

    uncorrectAnswear = (word) => {
        this.QuestionNames.push(word);
        this.QuestionResults.push("uncorrect");
        NextTest();
    }
}


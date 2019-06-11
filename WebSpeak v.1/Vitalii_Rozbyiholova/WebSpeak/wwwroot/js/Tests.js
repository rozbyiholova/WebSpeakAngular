
var info;

function getRandomInt(min, max) {
    return Math.floor(Math.random() * (max - min)) + min;
}

function InitNewTest(categoriesArray, picturesCount, textCount, soundsCount) {

    //shufle categories array

    this.info = new Info(categoriesArray, picturesCount, textCount, soundsCount);
}

function NextTest(array, picturesCount, textCount) {


    index.indexIncrease();
    
}

function LoadPictures() {

    const TEST_IMAGES = "test_images";
    var test_images = document.getElementsByClassName(TEST_IMAGES)[0];
    $("." + TEST_IMAGES).empty();

    var info = this.info;

    var selectedCategories = new Array();

    var categoriesLength = info.categories.length;
    selectedCategories[0] = info.categories[info.currentIndex];
    let i = 1;
    while (i < index.picturesCount) {
        let random = getRandomInt(0, categoriesLength);
        var category = info.categories[random];
        if (!selectedCategories.includes(category)) {
            selectedCategories.push(category);
        }

        //shufle selected categories array
        for (let i = 0; i < selectedCategories.length; i++) {
            var category = selectedCategories[i];
            var div = document.createElement('div');
            div.className = "col-md-6"
            var img = document.createElement('img');
            img.src = '../../' + category.picture;
            img.className = "img-fluid";
            div.appendChild(img);
            test_images.appendChild(div);
            i++;
        }
    }    
}

function LoadText(array, count) {
    const TEST_WORD = "test_word";
    var test_word = document.getElementsByClassName(TEST_WORD)[0];
    $("." + TEST_WORD).empty();

    var indexes = new Array();
    let i = 0;
    do {
        var randomTextIndex = getRandomInt(0, 2);
        if (!indexes.includes(randomTextIndex)) {
            var text = array[randomTextIndex].translation;
            var h3 = document.createElement("h3");
            var textNode = document.createTextNode(text);
            h3.appendChild(textNode);
            test_word.appendChild(h3);
            i++;
        }
    }
    while (i < count);
}

function CheckPictureWithText(category, picture) {
    const TEST_WORD = "test_word";
    var test_word = document.getElementsByClassName(TEST_WORD)[0];
    
    if (category.translation == test_word.firstChild) {
        picture.setAttribute("style:borderColor", "#00FF00");
    } else {
        picture.classList.add("uncorrect");
    }
}

class TestInfo {

    currentIndex;
    categories;
    picturesCount;
    textsCount;
    soundsCount;

    constructor(array, pictures, texts, sounds) {
        this.currentIndex = 0;
        this.categories = array;
        this.picturesCount = pictures;
        this.textsCount = texts;
        this.soundsCount = sounds;
    }

    indexIncrease = () => {
        this.currentIndex = this.currentIndex + 1;
    }
}
$('#slideshow').on('slide.bs.carousel', function (e) {
    var listNames = [];
    var duration = 0;

    listNames.push("sound");
    listNames.push("pronounceLearn");
    listNames.push("pronounceNative");

    var listAudio = GetList(listNames);

    play(listAudio);

    for (let i = 0; i < listAudio.length; ++i) {
        duration += listAudio[i].duration;
    }

    var slideshow = document.getElementsByClassName('carousel-item')

    for (let i = 0; i < slideshow.length; i++) {
        slideshow[i].style.transitionDelay = duration + "s";
    }
})

function play(list) {
    for (let i = 0; i < list.length; i++) {
        if (i === 0) {
            list[i].play()
        } else {
            list[i - 1].addEventListener('ended', function () {
                list[i].play()
            })
        }
    }
}

function GetList(listNames) {
    var listAudio = [];

    for (let i = 0; i < listNames.length; ++i) {
        var soundId = $('#slideshow').find(`.active .carousel-caption audio.${listNames[i]}`).attr('id');

        if (soundId != null) {
            var sound = document.getElementById(soundId);
            listAudio.push(sound);
        }
    }

    return listAudio;
}
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DataService } from '../../../services/data.service';
import { NgbCarouselConfig } from '@ng-bootstrap/ng-bootstrap';
var SlideshowComponent = /** @class */ (function () {
    function SlideshowComponent(dataService, activeRoute, config) {
        var _this_1 = this;
        this.dataService = dataService;
        this.config = config;
        this.duration = 500;
        this.iterator = 1;
        config.showNavigationArrows = false;
        config.keyboard = false;
        config.wrap = true;
        config.pauseOnHover = false;
        this.subscription = activeRoute.queryParams.subscribe(function (queryParam) {
            _this_1.idSubCat = queryParam['id'];
        });
    }
    SlideshowComponent.prototype.ngOnInit = function () {
        this.loadWords();
    };
    SlideshowComponent.prototype.loadWords = function () {
        var _this_1 = this;
        this.dataService.getWords(this.idSubCat)
            .subscribe(function (data) { return _this_1.words = data; });
    };
    SlideshowComponent.prototype.onSlide = function (slideData) {
        var listAudio = [];
        if (this.words[0].enableSound) {
            var sound = new Audio();
            var word = this.words[this.iterator];
            if (word) {
                sound.src = word.sound;
            }
            if (!sound.src.includes('null')) {
                listAudio.push(sound);
            }
        }
        if (this.words[0].enablePronounceLearnLang) {
            var pronounceLearn = new Audio();
            var word = this.words[this.iterator];
            if (word) {
                pronounceLearn.src = word.pronounceLearn;
            }
            listAudio.push(pronounceLearn);
        }
        if (this.words[0].enablePronounceNativeLang) {
            var pronounceNative = new Audio();
            var word = this.words[this.iterator];
            if (word) {
                pronounceNative.src = word.pronounceNative;
            }
            listAudio.push(pronounceNative);
        }
        var _loop_1 = function (i) {
            _this = this_1;
            listAudio[i].addEventListener('loadedmetadata', function () {
                _this.duration += listAudio[i].duration * 1000;
            });
        };
        var this_1 = this, _this;
        for (var i = 0; i < listAudio.length; ++i) {
            _loop_1(i);
        }
        var _loop_2 = function (i) {
            if (i === 0) {
                listAudio[i].load();
                listAudio[i].play();
            }
            else {
                listAudio[i - 1].addEventListener('ended', function () {
                    listAudio[i].load();
                    listAudio[i].play();
                });
            }
        };
        for (var i = 0; i < listAudio.length; i++) {
            _loop_2(i);
        }
        if (this.iterator == this.words.length - 1) {
            this.iterator = 0;
        }
        else {
            this.iterator++;
        }
    };
    SlideshowComponent = __decorate([
        Component({
            selector: 'slideshow',
            templateUrl: './slideshow.component.html',
            providers: [NgbCarouselConfig],
            styleUrls: ['./slideshow.component.scss']
        }),
        __metadata("design:paramtypes", [DataService, ActivatedRoute, NgbCarouselConfig])
    ], SlideshowComponent);
    return SlideshowComponent;
}());
export { SlideshowComponent };
//# sourceMappingURL=slideshow.component.js.map
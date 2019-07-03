import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { DataService } from '../../../services/data.service';
import { DTO } from '../../../models/DTO';
import { NgbCarouselConfig } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'slideshow',
    templateUrl: './slideshow.component.html',
    providers: [NgbCarouselConfig],
    styleUrls: ['./slideshow.component.scss']
})
export class SlideshowComponent implements OnInit {
    idSubCat: number;
    words: DTO[];
    duration: number = 500;
    iterator: number = 1;

    private subscription: Subscription;

    constructor(private dataService: DataService, activeRoute: ActivatedRoute, public config: NgbCarouselConfig) {
        config.showNavigationArrows = false;
        config.keyboard = false;
        config.wrap = true;
        config.pauseOnHover = false;

        this.subscription = activeRoute.queryParams.subscribe(
            (queryParam: any) => {
                this.idSubCat = queryParam['id'];
            }
        );
    }

    ngOnInit() {
        this.loadWords();
    }

    loadWords() {
        this.dataService.getWords(this.idSubCat)
            .subscribe((data: DTO[]) => this.words = data);
    }

    onSlide(slideData: any) {
        var listAudio: Array<HTMLAudioElement> = [];

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

        for (let i = 0; i < listAudio.length; ++i) {
            var _this = this;

            listAudio[i].addEventListener('loadedmetadata', function () {
                _this.duration += listAudio[i].duration * 1000;
            });
        }

        for (let i = 0; i < listAudio.length; i++) {
            if (i === 0) {
                listAudio[i].load();
                listAudio[i].play();
            } else {
                listAudio[i - 1].addEventListener('ended', function () {
                    listAudio[i].load();
                    listAudio[i].play()
                })
            }
        }

        if (this.iterator == this.words.length - 1) {
            this.iterator = 0;
        }
        else {
            this.iterator++;
        }
    }

}
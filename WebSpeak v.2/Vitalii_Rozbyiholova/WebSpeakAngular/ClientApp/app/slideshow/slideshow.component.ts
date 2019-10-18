import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { DataService } from '../data.service';
import { ActivatedRoute } from '@angular/router';
import { DTO } from '../../Models/DTO';
import { Subscription } from 'rxjs';
import { NgbCarouselConfig ,NgbCarousel, NgbSlideEvent, NgbSlideEventSource } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: "slideshow",
    templateUrl: "./slideshow.component.html",
    styleUrls: ["./slideshow.component.scss"],
    providers: [DataService]
})
export class SlideShowComponent implements OnInit {

    type: string;
    subcategoryId: number;
    words: DTO[];
    subscription: Subscription;

    private pronounceNative: boolean = true;
    private pronounceLearning: boolean = true;
    private paused: boolean = true;
    private nativeDuration: number = 0;
    private learningDuration: number = 0;

    @ViewChild("carousel", { static: true }) carousel: NgbCarousel;
    carouselRef: ElementRef;

    constructor(private dataService: DataService, activeRoute: ActivatedRoute, config: NgbCarouselConfig) {
        this.subscription = activeRoute.params
            .subscribe(params => this.subcategoryId = params["subcategoryId"]);
        this.subscription = activeRoute.queryParams
            .subscribe(params => this.type = params["type"]);

        config.keyboard = true;
        config.wrap = true;
        config.interval = 0;

        if (this.type === "slideshow") {
            if (this.pronounceLearning) {
                this.playLearning();
            }
        }
    }

    ngOnInit(): void {
        this.loadWords(this.subcategoryId);
        this.carouselRef.nativeElement.addEventListener("loadedmetadata",
            (ev: Event) => {
                 this.onMetadataLoaded(ev);
            });
    }

    loadWords(subcategoryId: number): void {
        this.dataService.getWords(subcategoryId)
            .subscribe((data: DTO[]) => {
                this.words = data;
            });
    }

    private playLearning() {
        const audio = this.getAudio("learn-pron");
        audio.play();
    }

    private playNative() {
        const audio = this.getAudio("native-pron");
        audio.play();
    }


    /*
     TODO:
      - play sounds (finished in constructor)
     */






    private togglePaused() {
        if (this.paused) {
            this.carousel.cycle();
        } else {
            this.carousel.pause();
        }
        this.paused = !this.paused;
    }

    onMetadataLoaded(ev: Event) {
        if (this.pronounceNative) {
            const audio: HTMLAudioElement = this.getAudio("native-pron");
            this.nativeDuration = this.getDuration(audio);
        }
        if (this.pronounceLearning) {}
    }

    onSlide(event: NgbSlideEvent) {
        this.nativeDuration = 0;
        this.learningDuration = 0;
    }

    private addEventListener() {
        document.getElementById("carousel").addEventListener("loadedmetadata", (e) => this.onMetadataLoaded(e));
    }

    private getAudio(targetClassSelector: string) {
        const target = document.querySelector(`.${targetClassSelector}`)[0] as HTMLElement;
        const audio = target.querySelector("check-audio")[0] as HTMLAudioElement;
        return audio;
    }

    private getDuration(audio: HTMLAudioElement): number {
        return audio.duration;
    }
}
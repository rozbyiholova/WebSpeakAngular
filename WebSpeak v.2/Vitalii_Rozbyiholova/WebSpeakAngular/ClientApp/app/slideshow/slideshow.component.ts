import { Component, OnInit } from '@angular/core';
import { DataService } from '../data.service';
import { ActivatedRoute } from '@angular/router';
import { DTO } from '../../Models/DTO';
import { Subscription } from 'rxjs';
import { NgbCarouselConfig } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'slideshow',
    templateUrl: `./slideshow.component.html`,
    providers: [DataService]
})
export class SlideShowComponent implements OnInit {

    subcategoryId: number;
    words: DTO[];
    subscription: Subscription;

    constructor(private dataService: DataService, activeRoute: ActivatedRoute, config: NgbCarouselConfig) {
        this.subscription = activeRoute.params
            .subscribe(params => this.subcategoryId = params['subcategoryId']);

        config.interval = 3000;
        config.wrap = false;
        config.keyboard = false;
        config.pauseOnHover = false;
        config.wrap = true;
    }

    ngOnInit(): void {
        this.loadWords(this.subcategoryId);
    }

    loadWords(subcategoryId: number): void {
        this.dataService.getWords(subcategoryId)
            .subscribe((data: DTO[]) => {
                this.words = data;
            });
    }
}
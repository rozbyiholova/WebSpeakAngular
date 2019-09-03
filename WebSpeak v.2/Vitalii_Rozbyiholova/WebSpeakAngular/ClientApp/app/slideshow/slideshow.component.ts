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

    type: string;
    subcategoryId: number;
    words: DTO[];
    subscription: Subscription;

    constructor(private dataService: DataService, activeRoute: ActivatedRoute, config: NgbCarouselConfig) {
        this.subscription = activeRoute.params
            .subscribe(params => this.subcategoryId = params["subcategoryId"]);
        this.subscription = activeRoute.queryParams
            .subscribe(params => this.type = params["type"]);

        if (this.type == "slideshow") {
            config.interval = 3000;
            config.wrap = false;
            config.keyboard = false;
            config.pauseOnHover = false;
            config.wrap = true;
        }
        else if(this.type == "manual") {
            config.interval = 0;
        } else {
            console.log("Unknown type");
        }
        
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
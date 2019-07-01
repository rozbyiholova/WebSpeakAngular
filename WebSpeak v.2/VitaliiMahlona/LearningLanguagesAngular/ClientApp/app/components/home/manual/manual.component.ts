import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { DataService } from '../../../services/data.service';
import { DTO } from '../../../models/DTO';
import { NgbCarouselConfig } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'manual',
    templateUrl: './manual.component.html',
    providers: [NgbCarouselConfig],
    styleUrls: ['./manual.component.scss']
})
export class ManualComponent implements OnInit {
    idSubCat: number;
    words: DTO[];

    private subscription: Subscription;

    constructor(private dataService: DataService, activeRoute: ActivatedRoute, config: NgbCarouselConfig) {
        config.interval = 0;
        config.showNavigationArrows = true;
        config.keyboard = true;
        config.wrap = true;

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
}
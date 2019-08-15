import { Component, OnInit } from '@angular/core';
import { DataService } from '../data.service';
import { ActivatedRoute } from '@angular/router';
import { DTO } from '../../Models/DTO';
import { Subscription } from 'rxjs';

@Component({
    selector: 'manual',
    templateUrl: `./manual.component.html`,
    providers: [DataService]
})
export class ManualComponent implements OnInit {

    subcategoryId: number;
    words: DTO[];
    subscription: Subscription;

    constructor(private dataService: DataService, activeRoute: ActivatedRoute) {
        this.subscription = activeRoute.params.subscribe(params => this.subcategoryId = params['subcategoryId']);
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
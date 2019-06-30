import { Component, OnInit } from '@angular/core';
import { DataService } from '../../../services/data.service';

import { trigger, style, animate, transition } from '@angular/animations';

@Component({
    selector: 'account-rating',
    animations: [
        trigger(
            'enterAnimation', [
                //transition(':enter', [
                //    style({ transform: 'translateY(0)', opacity: 0 }),
                //    animate('500ms', style({ opacity: 1 }))
                //]),
                //transition(':leave', [
                //    style({ transform: 'translateY(50%)', opacity: 1 }),
                //    animate('500ms', style({ opacity: 0 }))
                //])
            ]
        )
    ],
    templateUrl: './rating.component.html',
    styleUrls: ['./rating.component.scss']
})
export class AccountRatingComponent implements OnInit {
    rating: any;
    usersInfo: any;
    toggleLang: boolean[] = [];

    constructor(private dataService: DataService) { }

    ngOnInit() {
        this.getUsersInfo();
        this.loadRating();
    }

    loadRating() {
        this.dataService.getRating()
            .subscribe((data: any) => this.rating = data);
    }

    getUsersInfo() {
        this.dataService.getUsersInfo().subscribe((data: any) => this.usersInfo = data);
    }

    doToggleLang(event: any, id: number) {
        event.stopPropagation();
        event.stopImmediatePropagation();

        var item: boolean = this.toggleLang[id];
        this.toggleLang.fill(false);
        this.toggleLang[id] = !item;
    }
}
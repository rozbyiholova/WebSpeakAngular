import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DataService } from '../data.service';
import { Subscription } from 'rxjs';
import { DTO } from '../../Models/DTO';

@Component({
    selector: 'subcategories',
    templateUrl: `./subcategories.component.html`,
    providers: [DataService]
})
export class SubcategoryComponent implements OnInit {

    parentId: number;
    categories: DTO[];
    subscription: Subscription;

    constructor(private dataService: DataService, activeRoute: ActivatedRoute) {
        this.subscription = activeRoute.params.subscribe(params => this.parentId = params['parentId']);
    }

    ngOnInit(): void {
        this.loadSubcategories(this.parentId);
    }

    loadSubcategories(parentId: number): void {
        this.dataService.getSubcategories(parentId)
            .subscribe((data: DTO[]) => {
                this.categories = data;
            });
    }
}
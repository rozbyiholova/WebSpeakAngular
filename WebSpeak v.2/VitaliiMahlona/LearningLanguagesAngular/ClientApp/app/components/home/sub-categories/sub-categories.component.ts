import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { DataService } from '../../../services/data.service';
import { DTO } from '../../../models/DTO';

@Component({
    selector: 'sub-categories',
    templateUrl: './sub-categories.component.html',
    styleUrls: ['../list-items.component.scss']
})
export class SubCategoriesComponent implements OnInit {
    idCat: number;
    subCategories: DTO[];

    private subscription: Subscription;

    constructor(private dataService: DataService, activeRoute: ActivatedRoute) {
        this.subscription = activeRoute.queryParams.subscribe(
            (queryParam: any) => {
                this.idCat = queryParam['id'];
            }
        );
    }

    ngOnInit() {
        this.loadSubCategories();
    }

    loadSubCategories() {
        this.dataService.getSubCategories(this.idCat)
            .subscribe((data: DTO[]) => this.subCategories = data);
    }
}
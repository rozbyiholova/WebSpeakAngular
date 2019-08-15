import { Component, OnInit } from '@angular/core';
import { DataService } from '../data.service';
import { Router } from '@angular/router';
import { DTO } from '../../Models/DTO';

@Component({
    selector: 'categories',
    templateUrl: `./categories.component.html`,
    providers: [DataService]
})
export class CategoriesComponent implements OnInit {

    categories: DTO[];

    constructor(private dataService: DataService){}

    ngOnInit(): void {
        this.loadCategories();
    }

    loadCategories(): void {
        this.dataService.getCategories()
            .subscribe((data: DTO[]) => {
                this.categories = data;
            });
    }
}
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { DataService } from '../../../services/data.service';
import { DTO } from '../../../models/DTO';

@Component({
    selector: 'tests',
    templateUrl: './tests.component.html',
    styleUrls: ['../list-items.component.scss']
})
export class TestsComponent implements OnInit {
    idSubCat: number;
    tests: DTO[];

    private subscription: Subscription;

    constructor(private dataService: DataService, activeRoute: ActivatedRoute) {
        this.subscription = activeRoute.queryParams.subscribe(
            (queryParam: any) => {
                this.idSubCat = queryParam['id'];
            }
        );
    }

    ngOnInit() {
        this.loadTests();
    }

    loadTests() {
        this.dataService.getTests(this.idSubCat)
            .subscribe((data: DTO[]) => this.tests = data);
    }
}
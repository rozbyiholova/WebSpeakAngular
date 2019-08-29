import { Component, OnInit } from '@angular/core';
import { DataService } from '../../data.service';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { DTO } from '../../../Models/DTO';

@Component({
    selector: 'tests',
    templateUrl: `./testIndex.component.html`,
    providers: [DataService]
})
export class TestIndexComponent implements OnInit {

    private subcategoryId: number;
    private subscription: Subscription;
    tests: DTO[];

    constructor(private dataService: DataService, activeRoute: ActivatedRoute) {
            this.subscription = activeRoute.params.subscribe(params => {
                this.subcategoryId = params["subcategoryId"];
            });
        }

    ngOnInit(): void {
        this.loadTests();
    }

    loadTests(): void {
        this.dataService.getAllTests(this.subcategoryId)
            .subscribe((data: DTO[]) => {
                this.tests = data;
            });
    }
}
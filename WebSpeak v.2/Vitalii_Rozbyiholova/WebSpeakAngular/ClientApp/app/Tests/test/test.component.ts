import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DataService } from '../../data.service';
import { Subscription } from 'rxjs';
import { DTO } from '../../../Models/DTO';

@Component({
    selector: 'test',
    templateUrl: `./testLayout.html`,
    styleUrls: ['./test.style.scss'],
    providers: [DataService]
})
export class TestComponent implements OnInit {

    private testSetting: Object;
    private testId: number;
    private subscription: Subscription;

    test: DTO[];
    
    constructor(private dataService: DataService, activeRoute: ActivatedRoute) {
        this.subscription = activeRoute.params.subscribe(params => this.testId = params['testId']);
    }

    ngOnInit(): void {
        this.loadSubcategories(this.testId);
    }

    private loadSubcategories(testId: number): void {
        this.dataService.getTest(testId)
            .subscribe((data: DTO[]) => {
                this.test = data;
            });
    }
    
    /*----------Work with test----------*/


    
}
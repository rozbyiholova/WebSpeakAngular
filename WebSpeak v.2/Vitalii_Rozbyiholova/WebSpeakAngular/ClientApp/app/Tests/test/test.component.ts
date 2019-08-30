import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DataService } from '../../data.service';
import { Subscription } from 'rxjs';
import { DTO } from '../../../Models/DTO';
import { TestInfo } from '../helpers/TestInfo';

@Component({
    selector: 'test',
    templateUrl: `./testLayout.html`,
    styleUrls: ['./testStyle.scss'],
    encapsulation: ViewEncapsulation.None,
    providers: [DataService]
})
export class TestComponent implements OnInit {

    private testId: number;
    private subcategoryId: number;
    private subscription: Subscription;
    private test: DTO[];
    public testInfo: TestInfo;

    constructor(private dataService: DataService, activeRoute: ActivatedRoute) {
        this.subscription = activeRoute.params.subscribe(params => {
            this.testId = params['testId'];
            this.subcategoryId = params["subcategoryId"];
        });
    }

    ngOnInit(): void {
        this.loadSubcategories(this.testId);
    }

    private loadSubcategories(testId: number): void {
        this.dataService.getTest(this.subcategoryId)
            .subscribe((data: DTO[]) => {
                this.test = data;
                this.initTest();
            });
    }

    private initTest(): void {
        this.testInfo = new TestInfo(this.test, this.testId);
        this.testInfo.loadNextTest();
    }
    
    /*----------Work with test----------*/
    
    
}
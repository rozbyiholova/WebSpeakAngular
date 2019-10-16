import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DataService } from '../../data.service';
import { Subscription } from 'rxjs';
import { DTO } from '../../../Models/DTO';
import { User } from "../../../Models/User";
import { TestInfo } from '../helpers/TestInfo';
import { AuthService } from "../../auth.service";

@Component({
    selector: 'test',
    templateUrl: `./testLayout.html`,
    styleUrls: ['./testStyle.scss'],
    encapsulation: ViewEncapsulation.None,
    providers: [DataService]
})
export class TestComponent implements OnInit {

    private user: User = null;
    private testId: number;
    private subcategoryId: number;
    private subscription: Subscription;
    private test: DTO[];
    public testInfo: TestInfo;

    constructor(
        private dataService: DataService,
        private auth: AuthService,
        private activeRoute: ActivatedRoute
    ) {
        this.subscription = activeRoute.params.subscribe(params => {
            this.testId = params['testId'];
            this.subcategoryId = params["subcategoryId"];
        });
    }

    ngOnInit(): void {
        if (this.auth.isLoggedIn()) {
            this.auth.getUser().subscribe(u => this.user = u["user"] as User);
        }

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
        this.testInfo = new TestInfo(this.test, this.testId, this.subcategoryId, this.user);
        this.testInfo.loadNextTest();
    }

    private onTestEnded(result: Object) {
        console.log("test.component - OnTestEnded");
        this.dataService.saveTestResult(result);
    }
    
}
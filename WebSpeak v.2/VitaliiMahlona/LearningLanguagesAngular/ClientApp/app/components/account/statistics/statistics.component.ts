import { Component, OnInit } from '@angular/core';
import { DataService } from '../../../services/data.service';

import { trigger, style, animate, transition } from '@angular/animations';

@Component({
    selector: 'account-statistics',
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
    templateUrl: './statistics.component.html',
    styleUrls: ['./statistics.component.scss']
})
export class AccountStatisticsComponent implements OnInit {
    statistics: any;
    usersInfo: any;
    toggleLang: boolean[] = [];
    toggleCat: boolean[] = [];
    toggleSubCat: boolean[] = [];
    toggleTest: boolean[] = [];

    constructor(private dataService: DataService) { }

    ngOnInit() {
        this.getUsersInfo();
        this.loadStatistics();
    }

    loadStatistics() {
        this.dataService.getStatistics()
            .subscribe((data: any) => this.statistics = data);
    }

    getCategories(langId: number) {
        var uniqueEl = new Set<string>();

        for (let i = 0; i < this.statistics.testResults.length; ++i) {
            if (this.statistics.testResults[i].langName == this.statistics.langList[langId].wordNativeLang) {
                uniqueEl.add(this.statistics.testResults[i].categoryName);
            }
        }

        return uniqueEl;
    }

    getSubCategories(langId: number, cat: string) {
        var uniqueEl = new Set<string>();

        for (let i = 0; i < this.statistics.testResults.length; ++i) {
            if (this.statistics.testResults[i] && this.statistics.langList[langId] && this.statistics.testResults[i].langName == this.statistics.langList[langId].wordNativeLang
                && this.statistics.testResults[i].categoryName == cat) {
                uniqueEl.add(this.statistics.testResults[i].subCategoryName);
            }
        }  

        return uniqueEl;
    }

    getTests(langId: number, subCat: string) {
        var uniqueEl = new Set<string>();

        for (let i = 0; i < this.statistics.testResults.length; ++i) {
            if (this.statistics.testResults[i] && this.statistics.langList[langId] && this.statistics.testResults[i].langName == this.statistics.langList[langId].wordNativeLang
                && this.statistics.testResults[i].subCategoryName == subCat) {
                uniqueEl.add(this.statistics.testResults[i].testName.replace("\\", "/"));
            }
        }

        return uniqueEl;
    }

    getTest(langId: number, cat: string, subCat: string, test: string) {
        var testResults: any[] = [];

        for (let i = 0; i < this.statistics.testResults.length; ++i) {
            if (this.statistics.testResults[i] && this.statistics.langList[langId] && this.statistics.testResults[i].langName == this.statistics.langList[langId].wordNativeLang
                && this.statistics.testResults[i].categoryName == cat && this.statistics.testResults[i].subCategoryName == subCat && this.statistics.testResults[i].testName.replace("\\", "/") == test) {
                var testResult = {
                    testDate: this.statistics.testResults[i].testDate,
                    result: this.statistics.testResults[i].result
                };
                testResults.push(testResult);
            }
        }

        return testResults;
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

    doToggleCat(event: any, id: number) {
        event.stopPropagation();
        event.stopImmediatePropagation();

        var item: boolean = this.toggleCat[id];
        this.toggleCat.fill(false);
        this.toggleCat[id] = !item;
    }

    doToggleSubCat(event: any, id: number) {
        event.stopPropagation();
        event.stopImmediatePropagation();

        var item: boolean = this.toggleSubCat[id];
        this.toggleSubCat.fill(false);
        this.toggleSubCat[id] = !item;
    }

    doToggleTest(event: any, id: number) {
        event.stopPropagation();
        event.stopImmediatePropagation();

        var item: boolean = this.toggleTest[id];
        this.toggleTest.fill(false);
        this.toggleTest[id] = !item;
    }
}
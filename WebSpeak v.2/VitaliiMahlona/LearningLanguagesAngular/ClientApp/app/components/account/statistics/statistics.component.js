var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
import { Component } from '@angular/core';
import { DataService } from '../../../services/data.service';
import { trigger } from '@angular/animations';
var AccountStatisticsComponent = /** @class */ (function () {
    function AccountStatisticsComponent(dataService) {
        this.dataService = dataService;
        this.toggleLang = [];
        this.toggleCat = [];
        this.toggleSubCat = [];
        this.toggleTest = [];
    }
    AccountStatisticsComponent.prototype.ngOnInit = function () {
        this.getUsersInfo();
        this.loadStatistics();
    };
    AccountStatisticsComponent.prototype.loadStatistics = function () {
        var _this = this;
        this.dataService.getStatistics()
            .subscribe(function (data) { return _this.statistics = data; });
    };
    AccountStatisticsComponent.prototype.getCategories = function (langId) {
        var uniqueEl = new Set();
        for (var i = 0; i < this.statistics.testResults.length; ++i) {
            if (this.statistics.testResults[i].langName == this.statistics.langList[langId].wordNativeLang) {
                uniqueEl.add(this.statistics.testResults[i].categoryName);
            }
        }
        return uniqueEl;
    };
    AccountStatisticsComponent.prototype.getSubCategories = function (langId, cat) {
        var uniqueEl = new Set();
        for (var i = 0; i < this.statistics.testResults.length; ++i) {
            if (this.statistics.testResults[i] && this.statistics.langList[langId] && this.statistics.testResults[i].langName == this.statistics.langList[langId].wordNativeLang
                && this.statistics.testResults[i].categoryName == cat) {
                uniqueEl.add(this.statistics.testResults[i].subCategoryName);
            }
        }
        return uniqueEl;
    };
    AccountStatisticsComponent.prototype.getTests = function (langId, subCat) {
        var uniqueEl = new Set();
        for (var i = 0; i < this.statistics.testResults.length; ++i) {
            if (this.statistics.testResults[i] && this.statistics.langList[langId] && this.statistics.testResults[i].langName == this.statistics.langList[langId].wordNativeLang
                && this.statistics.testResults[i].subCategoryName == subCat) {
                uniqueEl.add(this.statistics.testResults[i].testName.replace("\\", "/"));
            }
        }
        return uniqueEl;
    };
    AccountStatisticsComponent.prototype.getTest = function (langId, cat, subCat, test) {
        var testResults = [];
        for (var i = 0; i < this.statistics.testResults.length; ++i) {
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
    };
    AccountStatisticsComponent.prototype.getUsersInfo = function () {
        var _this = this;
        this.dataService.getUsersInfo().subscribe(function (data) { return _this.usersInfo = data; });
    };
    AccountStatisticsComponent.prototype.doToggleLang = function (event, id) {
        event.stopPropagation();
        event.stopImmediatePropagation();
        var item = this.toggleLang[id];
        this.toggleLang.fill(false);
        this.toggleLang[id] = !item;
    };
    AccountStatisticsComponent.prototype.doToggleCat = function (event, id) {
        event.stopPropagation();
        event.stopImmediatePropagation();
        var item = this.toggleCat[id];
        this.toggleCat.fill(false);
        this.toggleCat[id] = !item;
    };
    AccountStatisticsComponent.prototype.doToggleSubCat = function (event, id) {
        event.stopPropagation();
        event.stopImmediatePropagation();
        var item = this.toggleSubCat[id];
        this.toggleSubCat.fill(false);
        this.toggleSubCat[id] = !item;
    };
    AccountStatisticsComponent.prototype.doToggleTest = function (event, id) {
        event.stopPropagation();
        event.stopImmediatePropagation();
        var item = this.toggleTest[id];
        this.toggleTest.fill(false);
        this.toggleTest[id] = !item;
    };
    AccountStatisticsComponent = __decorate([
        Component({
            selector: 'account-statistics',
            animations: [
                trigger('enterAnimation', [
                //transition(':enter', [
                //    style({ transform: 'translateY(0)', opacity: 0 }),
                //    animate('500ms', style({ opacity: 1 }))
                //]),
                //transition(':leave', [
                //    style({ transform: 'translateY(50%)', opacity: 1 }),
                //    animate('500ms', style({ opacity: 0 }))
                //])
                ])
            ],
            templateUrl: './accountStatistics.component.html',
            styleUrls: ['./accountStatistics.component.scss']
        }),
        __metadata("design:paramtypes", [DataService])
    ], AccountStatisticsComponent);
    return AccountStatisticsComponent;
}());
export { AccountStatisticsComponent };
//# sourceMappingURL=statistics.component.js.map
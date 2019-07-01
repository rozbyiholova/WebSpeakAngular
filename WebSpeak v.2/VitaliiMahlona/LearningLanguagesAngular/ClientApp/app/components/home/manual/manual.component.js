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
import { ActivatedRoute } from '@angular/router';
import { DataService } from '../../../services/data.service';
import { NgbCarouselConfig } from '@ng-bootstrap/ng-bootstrap';
var ManualComponent = /** @class */ (function () {
    function ManualComponent(dataService, activeRoute, config) {
        var _this = this;
        this.dataService = dataService;
        config.interval = 0;
        config.showNavigationArrows = true;
        config.keyboard = true;
        config.wrap = true;
        this.subscription = activeRoute.queryParams.subscribe(function (queryParam) {
            _this.idSubCat = queryParam['id'];
        });
    }
    ManualComponent.prototype.ngOnInit = function () {
        this.loadWords();
    };
    ManualComponent.prototype.loadWords = function () {
        var _this = this;
        this.dataService.getWords(this.idSubCat)
            .subscribe(function (data) { return _this.words = data; });
    };
    ManualComponent = __decorate([
        Component({
            selector: 'manual',
            templateUrl: './manual.component.html',
            providers: [NgbCarouselConfig],
            styleUrls: ['./manual.component.scss']
        }),
        __metadata("design:paramtypes", [DataService, ActivatedRoute, NgbCarouselConfig])
    ], ManualComponent);
    return ManualComponent;
}());
export { ManualComponent };
//# sourceMappingURL=manual.component.js.map
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
import { DataService } from '../data.service';
import { ActivatedRoute } from '@angular/router';
var ManualComponent = /** @class */ (function () {
    function ManualComponent(dataService, activeRoute) {
        var _this = this;
        this.dataService = dataService;
        this.subscription = activeRoute.params.subscribe(function (params) { return _this.subcategoryId = params['subcategoryId']; });
    }
    ManualComponent.prototype.ngOnInit = function () {
        this.loadWords(this.subcategoryId);
    };
    ManualComponent.prototype.loadWords = function (subcategoryId) {
        var _this = this;
        this.dataService.getWords(subcategoryId)
            .subscribe(function (data) {
            _this.words = data;
        });
    };
    ManualComponent = __decorate([
        Component({
            selector: 'manual',
            templateUrl: "./manual.component.html",
            providers: [DataService]
        }),
        __metadata("design:paramtypes", [DataService, ActivatedRoute])
    ], ManualComponent);
    return ManualComponent;
}());
export { ManualComponent };
//# sourceMappingURL=manual.component.js.map
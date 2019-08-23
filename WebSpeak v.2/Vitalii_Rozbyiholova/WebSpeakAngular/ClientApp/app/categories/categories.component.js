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
import { Router } from '@angular/router';
var CategoriesComponent = /** @class */ (function () {
    function CategoriesComponent(dataService, router) {
        this.dataService = dataService;
        this.router = router;
    }
    CategoriesComponent.prototype.ngOnInit = function () {
        this.loadCategories();
    };
    CategoriesComponent.prototype.loadCategories = function () {
        var _this = this;
        this.dataService.getCategories()
            .subscribe(function (data) {
            _this.categories = data;
        });
    };
    CategoriesComponent = __decorate([
        Component({
            selector: 'categories',
            templateUrl: "./categories.component.html",
            providers: [DataService]
        }),
        __metadata("design:paramtypes", [DataService, Router])
    ], CategoriesComponent);
    return CategoriesComponent;
}());
export { CategoriesComponent };
//# sourceMappingURL=categories.component.js.map
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
import { DataService } from '../data.service';
var SubcategoryComponent = /** @class */ (function () {
    function SubcategoryComponent(dataService, activeRoute) {
        var _this = this;
        this.dataService = dataService;
        this.subscription = activeRoute.params.subscribe(function (params) { return _this.parentId = params['parentId']; });
    }
    SubcategoryComponent.prototype.ngOnInit = function () {
        this.loadSubcategories(this.parentId);
    };
    SubcategoryComponent.prototype.loadSubcategories = function (parentId) {
        var _this = this;
        this.dataService.getSubcategories(parentId)
            .subscribe(function (data) {
            _this.categories = data;
        });
    };
    SubcategoryComponent = __decorate([
        Component({
            selector: 'subcategories',
            templateUrl: "./subcategories.component.html",
            providers: [DataService]
        }),
        __metadata("design:paramtypes", [DataService, ActivatedRoute])
    ], SubcategoryComponent);
    return SubcategoryComponent;
}());
export { SubcategoryComponent };
//# sourceMappingURL=subcategories.component.js.map
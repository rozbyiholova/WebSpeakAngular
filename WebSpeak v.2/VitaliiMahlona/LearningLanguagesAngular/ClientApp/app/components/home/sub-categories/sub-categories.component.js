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
var SubCategoriesComponent = /** @class */ (function () {
    function SubCategoriesComponent(dataService, activeRoute) {
        var _this = this;
        this.dataService = dataService;
        this.subscription = activeRoute.queryParams.subscribe(function (queryParam) {
            _this.idCat = queryParam['id'];
        });
    }
    SubCategoriesComponent.prototype.ngOnInit = function () {
        this.loadSubCategories();
    };
    SubCategoriesComponent.prototype.loadSubCategories = function () {
        var _this = this;
        this.dataService.getSubCategories(this.idCat)
            .subscribe(function (data) { return _this.subCategories = data; });
    };
    SubCategoriesComponent = __decorate([
        Component({
            selector: 'sub-categories',
            templateUrl: './sub-categories.component.html',
            styleUrls: ['../list-items.component.scss']
        }),
        __metadata("design:paramtypes", [DataService, ActivatedRoute])
    ], SubCategoriesComponent);
    return SubCategoriesComponent;
}());
export { SubCategoriesComponent };
//# sourceMappingURL=sub-categories.component.js.map
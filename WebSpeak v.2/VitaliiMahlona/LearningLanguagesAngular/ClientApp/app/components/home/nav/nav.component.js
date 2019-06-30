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
import { Router } from '@angular/router';
var NavComponent = /** @class */ (function () {
    function NavComponent(dataService, router) {
        this.dataService = dataService;
        this.router = router;
        this.returnUrl = this.router.url;
    }
    NavComponent.prototype.ngOnInit = function () {
        this.getUsersInfo();
    };
    NavComponent.prototype.getUsersInfo = function () {
        var _this = this;
        this.dataService.getUsersInfo().subscribe(function (data) { _this.usersInfo = data; console.log(_this.usersInfo); });
    };
    NavComponent.prototype.logout = function () {
        var _this = this;
        this.dataService.logout().subscribe(function (data) {
            _this.router.navigate(["#"]);
            _this.getUsersInfo();
        });
    };
    NavComponent = __decorate([
        Component({
            selector: 'app-nav',
            styles: [" \n        .active {color:black;}\n    "],
            templateUrl: './nav.component.html'
        }),
        __metadata("design:paramtypes", [DataService, Router])
    ], NavComponent);
    return NavComponent;
}());
export { NavComponent };
//# sourceMappingURL=nav.component.js.map
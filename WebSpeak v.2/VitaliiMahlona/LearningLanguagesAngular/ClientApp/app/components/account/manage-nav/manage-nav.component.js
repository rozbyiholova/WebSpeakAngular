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
var ManageNavComponent = /** @class */ (function () {
    function ManageNavComponent(dataService) {
        this.dataService = dataService;
    }
    ManageNavComponent.prototype.ngOnInit = function () {
        this.getUsersInfo();
    };
    ManageNavComponent.prototype.getUsersInfo = function () {
        var _this = this;
        this.dataService.getUsersInfo().subscribe(function (data) { return _this.usersInfo = data; });
    };
    ManageNavComponent = __decorate([
        Component({
            selector: 'manage-nav',
            styles: [" \n        .active {color:black;}\n    "],
            templateUrl: './manageNav.component.html'
        }),
        __metadata("design:paramtypes", [DataService])
    ], ManageNavComponent);
    return ManageNavComponent;
}());
export { ManageNavComponent };
//# sourceMappingURL=manage-nav.component.js.map
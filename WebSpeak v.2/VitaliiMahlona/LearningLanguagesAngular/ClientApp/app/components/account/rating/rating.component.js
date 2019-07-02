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
var AccountRatingComponent = /** @class */ (function () {
    function AccountRatingComponent(dataService) {
        this.dataService = dataService;
        this.toggleLang = [];
    }
    AccountRatingComponent.prototype.ngOnInit = function () {
        this.getUsersInfo();
        this.loadRating();
    };
    AccountRatingComponent.prototype.loadRating = function () {
        var _this = this;
        this.dataService.getRating()
            .subscribe(function (data) { return _this.rating = data; });
    };
    AccountRatingComponent.prototype.getUsersInfo = function () {
        var _this = this;
        this.dataService.getUsersInfo().subscribe(function (data) { return _this.usersInfo = data; });
    };
    AccountRatingComponent.prototype.doToggleLang = function (event, id) {
        event.stopPropagation();
        event.stopImmediatePropagation();
        var item = this.toggleLang[id];
        this.toggleLang.fill(false);
        this.toggleLang[id] = !item;
    };
    AccountRatingComponent = __decorate([
        Component({
            selector: 'account-rating',
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
            templateUrl: './rating.component.html',
            styleUrls: ['./rating.component.scss']
        }),
        __metadata("design:paramtypes", [DataService])
    ], AccountRatingComponent);
    return AccountRatingComponent;
}());
export { AccountRatingComponent };
//# sourceMappingURL=rating.component.js.map
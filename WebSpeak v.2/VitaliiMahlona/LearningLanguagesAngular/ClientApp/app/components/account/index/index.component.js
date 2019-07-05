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
var AccountIndexComponent = /** @class */ (function () {
    function AccountIndexComponent(dataService) {
        this.dataService = dataService;
        this.submitted = false;
    }
    AccountIndexComponent.prototype.ngOnInit = function () {
        this.loadSettings();
    };
    AccountIndexComponent.prototype.loadSettings = function () {
        var _this = this;
        this.dataService.getSettings()
            .subscribe(function (data) {
            _this.settings = data;
            if (_this.settings.enableNativeLang.includes("true")) {
                _this.enableNativeLang = true;
            }
            if (_this.settings.enableSound.includes("true")) {
                _this.enableSound = true;
            }
            if (_this.settings.enablePronounceNativeLang.includes("true")) {
                _this.enablePronounceNativeLang = true;
            }
            if (_this.settings.enablePronounceLearnLang.includes("true")) {
                _this.enablePronounceLearnLang = true;
            }
        });
    };
    AccountIndexComponent.prototype.setSettings = function () {
        this.submitted = true;
        this.settings.enableNativeLang = this.enableNativeLang.toString();
        this.settings.enableSound = this.enableSound.toString();
        this.settings.enablePronounceNativeLang = this.enablePronounceNativeLang.toString();
        this.settings.enablePronounceLearnLang = this.enablePronounceLearnLang.toString();
        this.dataService.setSettings(this.settings).subscribe();
    };
    AccountIndexComponent = __decorate([
        Component({
            selector: 'account-index',
            templateUrl: './index.component.html'
        }),
        __metadata("design:paramtypes", [DataService])
    ], AccountIndexComponent);
    return AccountIndexComponent;
}());
export { AccountIndexComponent };
//# sourceMappingURL=index.component.js.map
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
import { AuthService } from '../auth.service';
var HeaderComponent = /** @class */ (function () {
    function HeaderComponent(auth) {
        this.auth = auth;
        this.loggedIn = false;
    }
    HeaderComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.loggedIn = this.auth.isLoggedIn();
        this.setUser();
        this.auth.loggedIn.subscribe(function (isLoggedIn) {
            _this.loggedIn = isLoggedIn;
            _this.setUser();
        });
    };
    HeaderComponent.prototype.setUser = function () {
        var _this = this;
        if (this.loggedIn) {
            this.auth.getUser().subscribe(function (u) {
                _this.user = u["user"];
                _this.userName = _this.user.UserName;
            });
        }
    };
    HeaderComponent = __decorate([
        Component({
            selector: 'app-header',
            templateUrl: './app-header.html',
            styles: []
        }),
        __metadata("design:paramtypes", [AuthService])
    ], HeaderComponent);
    return HeaderComponent;
}());
export { HeaderComponent };
//# sourceMappingURL=app-header.js.map
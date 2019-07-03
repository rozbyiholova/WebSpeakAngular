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
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { DataService } from '../../../services/data.service';
import { Router } from '@angular/router';
import { NavComponent } from '../nav/nav.component';
var LoginComponent = /** @class */ (function () {
    function LoginComponent(dataService, formBuilder, router, activeRoute, nav) {
        var _this = this;
        this.dataService = dataService;
        this.formBuilder = formBuilder;
        this.router = router;
        this.nav = nav;
        this.submitted = false;
        this.returnUrl = '#';
        this.errorMessage = '';
        this.subscription = activeRoute.queryParams.subscribe(function (queryParam) {
            if (queryParam['returnUrl'] != undefined) {
                _this.returnUrl = queryParam['returnUrl'];
            }
        });
    }
    LoginComponent.prototype.ngOnInit = function () {
        this.loginGet();
        this.loginForm = this.formBuilder.group({
            email: ['', [Validators.required]],
            password: ['', [Validators.required]],
            rememberMe: [false]
        });
    };
    Object.defineProperty(LoginComponent.prototype, "f", {
        get: function () { return this.loginForm.controls; },
        enumerable: true,
        configurable: true
    });
    LoginComponent.prototype.loginGet = function () {
        var _this = this;
        this.dataService.loginGet(this.returnUrl)
            .subscribe(function (data) {
            _this.returnUrl = data.returnUrl;
            _this.externalLogins = data.externalLogins;
        });
    };
    LoginComponent.prototype.loginPost = function () {
        var _this = this;
        this.submitted = true;
        if (this.loginForm.invalid) {
            return;
        }
        this.dataService.loginPost(this.loginForm.value)
            .subscribe(function (data) {
            _this.returnUrl = data.returnUrl;
            _this.errorMessage = data.errorMessage;
            _this.nav.getUsersInfo();
            if (_this.errorMessage == "") {
                _this.router.navigate([_this.returnUrl]);
            }
        }, function (err) { return _this.router.navigate(['/Account/Login']); });
    };
    LoginComponent.prototype.externalLogin = function (provider) {
        this.dataService.externalLogin(provider, this.returnUrl);
    };
    LoginComponent = __decorate([
        Component({
            selector: 'login',
            templateUrl: './login.component.html',
            providers: [NavComponent]
        }),
        __metadata("design:paramtypes", [DataService, FormBuilder, Router, ActivatedRoute, NavComponent])
    ], LoginComponent);
    return LoginComponent;
}());
export { LoginComponent };
//# sourceMappingURL=login.component.js.map
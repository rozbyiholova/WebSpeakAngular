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
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from '../../../services/data.service';
import { FormBuilder, Validators } from '@angular/forms';
var ExternalLoginComponent = /** @class */ (function () {
    function ExternalLoginComponent(dataService, activeRoute, formBuilder, router) {
        var _this = this;
        this.dataService = dataService;
        this.formBuilder = formBuilder;
        this.router = router;
        this.errorMessage = null;
        this.submitted = false;
        this.subscription = activeRoute.queryParams.subscribe(function (queryParam) {
            _this.returnUrl = queryParam['returnUrl'];
            if (queryParam['remoteError']) {
                _this.remoteError = queryParam['remoteError'];
            }
        });
    }
    ExternalLoginComponent.prototype.ngOnInit = function () {
        this.externalLoginForm = this.formBuilder.group({
            email: ['', [Validators.required, Validators.email]]
        });
        this.loadModel();
    };
    Object.defineProperty(ExternalLoginComponent.prototype, "f", {
        get: function () { return this.externalLoginForm.controls; },
        enumerable: true,
        configurable: true
    });
    ExternalLoginComponent.prototype.loadModel = function () {
        var _this = this;
        this.dataService.callbackGet(this.returnUrl, this.remoteError).subscribe(function (data) {
            _this.errorMessage = data.errorMessage;
            if (_this.errorMessage == null) {
                if (!data.loginProvider) {
                    _this.router.navigate([_this.returnUrl]);
                }
                _this.data = data;
            }
        });
    };
    ExternalLoginComponent.prototype.login = function () {
        var _this = this;
        this.submitted = true;
        if (this.externalLoginForm.invalid) {
            return;
        }
        this.dataService.callbackPost(this.data).subscribe(function (data) {
            _this.errorMessage = data.errorMessage;
            if (_this.errorMessage == null) {
                _this.router.navigate([_this.returnUrl]);
            }
        }, function (e) { return console.log(e); });
    };
    ExternalLoginComponent = __decorate([
        Component({
            selector: 'external-login',
            templateUrl: './externalLogin.component.html'
        }),
        __metadata("design:paramtypes", [DataService, ActivatedRoute, FormBuilder, Router])
    ], ExternalLoginComponent);
    return ExternalLoginComponent;
}());
export { ExternalLoginComponent };
//# sourceMappingURL=external-login.component.js.map
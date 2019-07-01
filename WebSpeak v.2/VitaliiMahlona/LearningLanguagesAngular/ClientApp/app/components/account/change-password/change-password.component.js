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
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MustMatch } from '../../../_helpers/must-match.validator';
var AccountChangePasswordComponent = /** @class */ (function () {
    function AccountChangePasswordComponent(dataService, formBuilder, router) {
        this.dataService = dataService;
        this.formBuilder = formBuilder;
        this.router = router;
        this.errorMessage = null;
        this.submitted = false;
        this.correctSubmitted = false;
    }
    AccountChangePasswordComponent.prototype.ngOnInit = function () {
        this.changePasswordForm = this.formBuilder.group({
            oldPassword: ['', [Validators.required]],
            newPassword: ['', [Validators.required, Validators.pattern("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]],
            confirmPassword: ['', Validators.required]
        }, {
            validator: MustMatch('newPassword', 'confirmPassword')
        });
    };
    Object.defineProperty(AccountChangePasswordComponent.prototype, "f", {
        get: function () { return this.changePasswordForm.controls; },
        enumerable: true,
        configurable: true
    });
    AccountChangePasswordComponent.prototype.setNewPassword = function () {
        var _this = this;
        this.submitted = true;
        if (this.changePasswordForm.invalid) {
            return;
        }
        this.correctSubmitted = true;
        this.dataService.setPassword(this.changePasswordForm.value)
            .subscribe(function (data) { return _this.errorMessage = data.errorMessage; }, function (e) { return console.log(e); });
    };
    AccountChangePasswordComponent = __decorate([
        Component({
            selector: 'account-change-password',
            templateUrl: './accountChangePassword.component.html'
        }),
        __metadata("design:paramtypes", [DataService, FormBuilder, Router])
    ], AccountChangePasswordComponent);
    return AccountChangePasswordComponent;
}());
export { AccountChangePasswordComponent };
//# sourceMappingURL=change-password.component.js.map
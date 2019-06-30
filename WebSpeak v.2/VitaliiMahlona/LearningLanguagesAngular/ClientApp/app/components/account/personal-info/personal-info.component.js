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
var AccountPersonalInfoComponent = /** @class */ (function () {
    function AccountPersonalInfoComponent(dataService, formBuilder, router) {
        this.dataService = dataService;
        this.formBuilder = formBuilder;
        this.router = router;
        this.errorMessage = null;
        this.submitted = false;
        this.correctSubmitted = false;
    }
    AccountPersonalInfoComponent.prototype.ngOnInit = function () {
        this.personalInfoForm = this.formBuilder.group({
            email: ['', [Validators.required, Validators.email]],
            firstName: ['', [Validators.required, Validators.minLength(3)]],
            lastName: ['', [Validators.required, Validators.minLength(3)]],
            username: ['', [Validators.required, Validators.minLength(3)]],
            avatar: [null]
        });
        this.loadPersonalInfo();
    };
    Object.defineProperty(AccountPersonalInfoComponent.prototype, "f", {
        get: function () { return this.personalInfoForm.controls; },
        enumerable: true,
        configurable: true
    });
    AccountPersonalInfoComponent.prototype.loadPersonalInfo = function () {
        var _this = this;
        this.dataService.getPersonalInfo()
            .subscribe(function (data) {
            _this.errorMessage = data.errorMessage;
            if (_this.errorMessage == null) {
                _this.personalInfoForm.setValue({
                    email: data.email,
                    firstName: data.firstName,
                    lastName: data.lastName,
                    username: data.username,
                    avatar: data.avatar
                });
            }
        });
    };
    AccountPersonalInfoComponent.prototype.setPersonalInfo = function () {
        var _this = this;
        this.submitted = true;
        if (this.personalInfoForm.invalid) {
            return;
        }
        this.correctSubmitted = true;
        this.dataService.setPersonalInfo(this.personalInfoForm.value)
            .subscribe(function (data) {
            _this.errorMessage = data.errorMessage;
        }, function (e) { return console.log(e); });
    };
    AccountPersonalInfoComponent = __decorate([
        Component({
            selector: 'account-personal-info',
            templateUrl: './accountPersonalInfo.component.html'
        }),
        __metadata("design:paramtypes", [DataService, FormBuilder, Router])
    ], AccountPersonalInfoComponent);
    return AccountPersonalInfoComponent;
}());
export { AccountPersonalInfoComponent };
//# sourceMappingURL=personal-info.component.js.map
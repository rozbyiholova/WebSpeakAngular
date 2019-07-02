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
import { DataService } from '../../../services/data.service';
import { Router } from '@angular/router';
import { MustMatch } from '../../../_helpers/must-match.validator';
var RegisterComponent = /** @class */ (function () {
    function RegisterComponent(dataService, formBuilder, router) {
        this.dataService = dataService;
        this.formBuilder = formBuilder;
        this.router = router;
        this.submitted = false;
    }
    RegisterComponent.prototype.ngOnInit = function () {
        this.registerForm = this.formBuilder.group({
            email: ['', [Validators.required, Validators.email]],
            password: ['', [Validators.required, Validators.pattern("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]],
            passwordConfirm: ['', Validators.required],
            avatar: [null]
        }, {
            validator: MustMatch('password', 'passwordConfirm')
        });
    };
    Object.defineProperty(RegisterComponent.prototype, "f", {
        get: function () { return this.registerForm.controls; },
        enumerable: true,
        configurable: true
    });
    RegisterComponent.prototype.register = function () {
        var _this = this;
        this.submitted = true;
        if (this.registerForm.invalid) {
            return;
        }
        this.dataService.register(this.prepareSaveUserInfo())
            .subscribe(function (data) { return _this.router.navigate(['/']); }, function (err) { return _this.router.navigate(['/Account/Register']); });
    };
    RegisterComponent.prototype.fileChange = function (files) {
        if (files && files[0].size > 0) {
            this.registerForm.patchValue({
                avatar: files[0]
            });
        }
    };
    RegisterComponent.prototype.prepareSaveUserInfo = function () {
        var formModel = this.registerForm.value;
        var formData = new FormData();
        formData.append("email", formModel.email);
        formData.append("password", formModel.password);
        formData.append("passwordConfirm", formModel.passwordConfirm);
        formData.append("avatar", formModel.avatar);
        return formData;
    };
    RegisterComponent = __decorate([
        Component({
            selector: 'register',
            templateUrl: './register.component.html'
        }),
        __metadata("design:paramtypes", [DataService, FormBuilder, Router])
    ], RegisterComponent);
    return RegisterComponent;
}());
export { RegisterComponent };
//# sourceMappingURL=register.component.js.map
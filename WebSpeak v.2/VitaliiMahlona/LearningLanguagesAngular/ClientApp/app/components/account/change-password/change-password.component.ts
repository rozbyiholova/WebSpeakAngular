import { Component, OnInit } from '@angular/core';
import { DataService } from '../../../services/data.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { MustMatch } from '../../../_helpers/must-match.validator'; 

import { ChangePasswordViewModel } from '../../../models/ChangePasswordViewModel';

@Component({
    selector: 'account-change-password',
    templateUrl: './change-password.component.html'
})
export class AccountChangePasswordComponent implements OnInit {
    errorMessage: string = null;
    submitted = false;
    correctSubmitted = false;

    changePasswordForm: FormGroup;

    constructor(private dataService: DataService, private formBuilder: FormBuilder, private router: Router) { }

    ngOnInit() {
        this.changePasswordForm = this.formBuilder.group({
            oldPassword: ['', [Validators.required]],
            newPassword: ['', [Validators.required, Validators.pattern("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]],
            confirmPassword: ['', Validators.required]
        },  {
                validator: MustMatch('newPassword', 'confirmPassword')
            });
    }

    // convenience getter for easy access to form fields
    get f() { return this.changePasswordForm.controls; }

    setNewPassword() {
        this.submitted = true;

        if (this.changePasswordForm.invalid) {
            return;
        }

        this.correctSubmitted = true;

        this.dataService.setPassword(this.changePasswordForm.value)
            .subscribe(
                (data: ChangePasswordViewModel) => this.errorMessage = data.errorMessage,
                (e: any) => console.log(e));
    }
}
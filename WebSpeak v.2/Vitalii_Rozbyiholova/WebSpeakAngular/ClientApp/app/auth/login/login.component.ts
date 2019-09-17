import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import {AuthService} from "../../auth.service"
import { ActivatedRoute, Router } from '@angular/router';
import {LoginModel} from "../../../Models/LoginModel";

@Component({
    selector: 'login',
    templateUrl: `./login.component.html`
})
export class LoginComponent implements OnInit {

    private submitted: boolean = false;
    private loginForm: FormGroup;
    private loginModel: LoginModel;

    constructor(
        private authService: AuthService,
        private router: Router,
        private formBuilder: FormBuilder) {}

    ngOnInit(): void {
        this.loginForm = this.formBuilder.group({
            login: [null, [Validators.required]],
            password: [null, [Validators.required, Validators.minLength(6)]]
        });
    }

    public provideLogin(): void {
        this.submitted = true;

        if (this.loginForm.invalid) {
            console.log(this.loginForm.errors);
            return;
        }

        const credentials = JSON.stringify(this.loginForm.value);
        this.loginModel = JSON.parse(credentials) as LoginModel;
        this.authService.login(this.loginModel)
            .subscribe(response => {
                    const token = (<any>response).token;
                    localStorage.setItem("jwt", token);
                    this.authService.notifyLogin(this.loginModel.login);
                    this.router.navigate(["/"]);
                },
                err => {
                    console.log(err);
                });
    }
}
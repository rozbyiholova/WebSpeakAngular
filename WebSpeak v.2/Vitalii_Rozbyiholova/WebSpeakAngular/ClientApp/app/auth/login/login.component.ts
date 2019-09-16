import { Component} from '@angular/core';
import { NgForm } from "@angular/forms";
import {AuthService} from "../../auth.service"
import { ActivatedRoute, Router } from '@angular/router';
import {LoginModel} from "../../../Models/LoginModel";

@Component({
    selector: 'login',
    templateUrl: `./login.component.html`,
    providers: [AuthService]
})
export class LoginComponent {

    public invalidLogin: boolean;
    private user: LoginModel;

    constructor(private authService: AuthService, private router: Router) {

    }


    public provideLogin(form: NgForm): void {
        const credentials = JSON.stringify(form.value);
        this.user = JSON.parse(credentials) as LoginModel;
        this.authService.login(this.user)
            .subscribe(response => {
                const token = (<any>response).token;
                localStorage.setItem("jwt", token);
                this.invalidLogin = false;
                this.router.navigate(["/"]);
                this.authService.emitLogin(this.user.login);
            },
            err => {
                this.invalidLogin = true;
            });
    }
}
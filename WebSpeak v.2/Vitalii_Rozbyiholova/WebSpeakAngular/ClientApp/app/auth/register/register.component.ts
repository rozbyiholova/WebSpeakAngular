import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { first } from 'rxjs/operators';
import { AuthService } from '../../auth.service';
import { User } from "../../../Models/User";

@Component({
    selector: "register",
    templateUrl: "./register.component.html",
    styles: [],
    providers: [AuthService]
})
export class RegisterComponent implements OnInit{
    public registerForm: FormGroup;
    private user: User;
    private loading: boolean = false;
    private submitted: boolean = false;
    public isLoginTaken: boolean = false;

    constructor(
        private auth: AuthService,
        private router: Router,
        private formBuilder: FormBuilder) { }

    ngOnInit(): void {
        this.registerForm = this.formBuilder.group({
            username: [null, [Validators.required]],
            email: [null, [Validators.required, Validators.email]],
            passwordHash: [null, [Validators.required, Validators.minLength(6)]]
        });
    }
    
    public provideRegistration() {
        this.submitted = true;

        if (this.registerForm.invalid) { return; }

        this.loading = true;

        const credentials = JSON.stringify(this.registerForm.value);
        this.user = JSON.parse(credentials) as User;
        this.auth.register(this.user)
            .subscribe(response => {
                    const token = (<any>response).token;
                    localStorage.setItem("jwt", token);
                    this.router.navigate(["/"]);
                    this.auth.notifyLogin(this.user.email);
                },
                err => {
                    console.log(err);
                });
    }
}
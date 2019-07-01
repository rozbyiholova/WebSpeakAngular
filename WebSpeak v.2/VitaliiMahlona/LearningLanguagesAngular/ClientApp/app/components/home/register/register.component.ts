import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DataService } from '../../../services/data.service';
import { Router } from '@angular/router';

import { MustMatch } from '../../../_helpers/must-match.validator';

@Component({
    selector: 'register',
    templateUrl: './register.component.html'
})
export class RegisterComponent implements OnInit {
    submitted = false;

    registerForm: FormGroup;

    constructor(private dataService: DataService, private formBuilder: FormBuilder, private router: Router) { }

    ngOnInit() {
        this.registerForm = this.formBuilder.group({
            email: ['', [Validators.required, Validators.email]],
            password: ['', [Validators.required, Validators.pattern("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]],
            passwordConfirm: ['', Validators.required],
            avatar: [null]
        }, {
                validator: MustMatch('password', 'passwordConfirm')
            });
    }

    get f() { return this.registerForm.controls; }

    register() {
        this.submitted = true;

        if (this.registerForm.invalid) {
            return;
        }

        this.dataService.register(this.registerForm.value)
            .subscribe((data: any) => this.router.navigate(['/']),
                        err => this.router.navigate(['/Account/Register']));
    }
}
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { DataService } from '../../../services/data.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { ExternalLoginViewModel } from '../../../models/ExternalLoginViewModel'

@Component({
    selector: 'external-login',
    templateUrl: './external-login.component.html'
})
export class ExternalLoginComponent implements OnInit {
    data: ExternalLoginViewModel;
    errorMessage: string = null;
    submitted = false;
    returnUrl: string;
    remoteError: string;

    externalLoginForm: FormGroup;

    private subscription: Subscription;

    constructor(private dataService: DataService, activeRoute: ActivatedRoute, private formBuilder: FormBuilder, private router: Router) {
        this.subscription = activeRoute.queryParams.subscribe(
            (queryParam: any) => {
                this.returnUrl = queryParam['returnUrl'];

                if (queryParam['remoteError']) {
                    this.remoteError = queryParam['remoteError'];
                }
            }
        );
    }

    ngOnInit() {
        this.externalLoginForm = this.formBuilder.group({
            email: ['', [Validators.required, Validators.email]]
        });

        this.loadModel();
    }

    get f() { return this.externalLoginForm.controls; }

    loadModel() {
        this.dataService.callbackGet(this.returnUrl, this.remoteError).subscribe((data: ExternalLoginViewModel) => {
            this.errorMessage = data.errorMessage;

            if (this.errorMessage == null) {
                if (!data.loginProvider) {
                    this.router.navigate([this.returnUrl]);
                }

                this.data = data;
            }
        });
    }

    login() {
        this.submitted = true;

        if (this.externalLoginForm.invalid) {
            return;
        }

        this.dataService.callbackPost(this.data).subscribe(
            (data: ExternalLoginViewModel) => {
                this.errorMessage = data.errorMessage;

                if (this.errorMessage == null) {
                    this.router.navigate([this.returnUrl]);
                }
            },
            (e: any) => console.log(e));
    }
}
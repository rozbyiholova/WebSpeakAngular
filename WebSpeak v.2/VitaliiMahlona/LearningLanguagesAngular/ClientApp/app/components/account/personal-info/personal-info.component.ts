import { Component, OnInit } from '@angular/core';
import { DataService } from '../../../services/data.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { PersonalInfoViewModel } from '../../../models/PersonalInfoViewModel';

@Component({
    selector: 'account-personal-info',
    templateUrl: './personal-info.component.html'
})
export class AccountPersonalInfoComponent implements OnInit {
    errorMessage: string = null;
    submitted = false;
    correctSubmitted = false;

    personalInfoForm: FormGroup;

    constructor(private dataService: DataService, private formBuilder: FormBuilder, private router: Router) { }

    ngOnInit() {
        this.personalInfoForm = this.formBuilder.group({
            email: ['', [Validators.required, Validators.email]],
            firstName: ['', [Validators.required, Validators.minLength(3)]],
            lastName: ['', [Validators.required, Validators.minLength(3)]],
            username: ['', [Validators.required, Validators.minLength(3)]],
            avatar: [null]
        });

        this.loadPersonalInfo();
    }

    get f() { return this.personalInfoForm.controls; }

    loadPersonalInfo() {
        this.dataService.getPersonalInfo()
            .subscribe((data: PersonalInfoViewModel) => {
                this.errorMessage = data.errorMessage;

                if (this.errorMessage == null) {
                    console.log(data);
                    this.personalInfoForm.setValue({
                        email: data.email,
                        firstName: data.firstName,
                        lastName: data.lastName,
                        username: data.username,
                        avatar: data.avatar
                    });
                }
            });
    }

    setPersonalInfo() {
        this.submitted = true;

        if (this.personalInfoForm.invalid) {
            return;
        }

        this.correctSubmitted = true;

        this.dataService.setPersonalInfo(this.prepareSaveUserInfo())
            .subscribe(
            (data: PersonalInfoViewModel) => {
                this.errorMessage = data.errorMessage;
            },
            (e: any) => console.log(e));
    }

    fileChange(files: FileList) {
        if (files && files[0].size > 0) {
            this.personalInfoForm.patchValue({
                avatar: files[0]
            });
        }
    }

    prepareSaveUserInfo(): FormData {
        const formModel = this.personalInfoForm.value;

        let formData = new FormData();
        formData.append("email", formModel.email);
        formData.append("firstName", formModel.firstName);
        formData.append("lastName", formModel.lastName);
        formData.append("username", formModel.username);
        formData.append("avatar", formModel.avatar);

        return formData;
    }
}
import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthService } from '../auth.service';
import { Subscription } from 'rxjs';

@Component({
    selector: 'app-header',
    templateUrl: './app-header.html',
    styles: []
})
export class HeaderComponent implements OnInit, OnDestroy {
    

    public subscription: Subscription;
    public loggedIn: boolean;
    public userName: string;

    constructor(private auth: AuthService) {
       
    }

    ngOnInit(): void {
        this.loggedIn = this.auth.isLoggedIn();
        const token = this.auth.getDecodedUser();

        if (token) { this.userName = token["userLogin"]; }

        this.subscription = this.auth.getLoggedIn().subscribe((name: string) => {
            if (name) {
                this.loggedIn = true;
                this.userName = name;
            }
        });
    }

    ngOnDestroy(): void {
        this.subscription.unsubscribe();
    }
}
import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthService } from '../auth.service';
import { Subscription } from 'rxjs';
import { User } from "../../Models/User";

@Component({
    selector: 'app-header',
    templateUrl: './app-header.html',
    styles: []
})
export class HeaderComponent implements OnInit {
    
    public loggedIn: boolean = false;
    public userName: string;
    public user: User;

    constructor(private auth: AuthService) {
        
    }

    ngOnInit(): void {
        this.loggedIn = this.auth.isLoggedIn();

        this.setUser();

        this.auth.loggedIn.subscribe((isLoggedIn: boolean) => {
            this.loggedIn = isLoggedIn;
            this.setUser();
        });
    }

    private setUser(): void {
        if (this.loggedIn) {
            this.auth.getUser().subscribe(u => {
                this.user = u["user"] as User;
                this.userName = this.user.UserName;
            });
        }
    }
}
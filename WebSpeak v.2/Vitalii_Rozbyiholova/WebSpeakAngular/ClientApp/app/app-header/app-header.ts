import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth.service';

@Component({
    selector: 'app-header',
    templateUrl: './app-header.html',
    styles: [],
    providers: [AuthService]
})
export class HeaderComponent {
    public loggedIn: boolean;
    public userName: string;

    constructor(private auth: AuthService) {
        this.loggedIn = auth.isLoggedIn();
        this.auth.loggedIn.subscribe((name: string) => {
            console.log("caught an login event");
            this.userName = name;
            this.loggedIn = true;
        });
    }
}
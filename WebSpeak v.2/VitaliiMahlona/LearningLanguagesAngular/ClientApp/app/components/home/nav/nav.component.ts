import { Component, OnInit } from '@angular/core';
import { DataService } from '../../../services/data.service';
import { Router } from '@angular/router';

@Component({
    selector: 'app-nav',
    styleUrls: ['./nav.component.scss'],
    templateUrl: './nav.component.html'
})
export class NavComponent implements OnInit {
    usersInfo: any;
    returnUrl: string;

    constructor(private dataService: DataService, private router: Router) {
        this.returnUrl = this.router.url;
    }

    ngOnInit() {
        this.getUsersInfo();
    }

    getUsersInfo() {
        this.dataService.getUsersInfo().subscribe((data: any) => { this.usersInfo = data; console.log(this.usersInfo); });
    }

    logout() {
        this.dataService.logout().subscribe((data: any) => {
            this.router.navigate(["#"]);
            this.getUsersInfo();
        });
    }
}
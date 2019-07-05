import { Component, OnInit } from '@angular/core';
import { DataService } from '../../../services/data.service';
import { Router } from '@angular/router';

import { DTOUsersInfo } from '../../../models/DTOUsersInfo'

import { EventEmitterService } from '../../../services/event-emitter.service'

@Component({
    selector: 'app-nav',
    styleUrls: ['./nav.component.scss'],
    templateUrl: './nav.component.html'
})
export class NavComponent implements OnInit {
    usersInfo: DTOUsersInfo;
    returnUrl: string;

    constructor(private dataService: DataService, private router: Router, private eventEmitterService: EventEmitterService) {
        this.returnUrl = this.router.url;
    }

    ngOnInit() {
        this.getUsersInfo();

        if (this.eventEmitterService.subsVar == undefined) {
            this.eventEmitterService.subsVar = this.eventEmitterService.
                invokeUsersInfo.subscribe((name: string) => {
                    this.getUsersInfo();
                });
        }  
    }

    getUsersInfo() {
        this.dataService.getUsersInfo().subscribe((data: DTOUsersInfo) => { this.usersInfo = data; console.log(this.usersInfo); });
    }

    logout() {
        this.dataService.logout().subscribe((data: any) => {
            this.router.navigate(["#"]);
            this.getUsersInfo();
        });
    }
}
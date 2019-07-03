import { Component, OnInit } from '@angular/core';
import { DataService } from '../../../services/data.service';

import { DTOUsersInfo } from '../../../models/DTOUsersInfo'

@Component({
    selector: 'manage-nav',
    styles: [` 
        .active {color:black;}
    `],
    templateUrl: './manage-nav.component.html'
})
export class ManageNavComponent implements OnInit {
    usersInfo: DTOUsersInfo;

    constructor(private dataService: DataService) { }

    ngOnInit() {
        this.getUsersInfo();
    }

    getUsersInfo() {
        this.dataService.getUsersInfo().subscribe((data: DTOUsersInfo) => this.usersInfo = data);
    }
}
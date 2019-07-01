import { Component, OnInit } from '@angular/core';
import { DataService } from '../../../services/data.service';

@Component({
    selector: 'manage-nav',
    styles: [` 
        .active {color:black;}
    `],
    templateUrl: './manage-nav.component.html'
})
export class ManageNavComponent implements OnInit {
    usersInfo: any;

    constructor(private dataService: DataService) { }

    ngOnInit() {
        this.getUsersInfo();
    }

    getUsersInfo() {
        this.dataService.getUsersInfo().subscribe((data: any) => this.usersInfo = data);
    }
}
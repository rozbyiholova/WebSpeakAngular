import { Component, OnInit } from '@angular/core';
import { DataService } from '../../data.service';
import { DTO } from '../../../Models/DTO';

@Component({
    selector: 'tests',
    templateUrl: `./testIndex.component.html`,
    providers: [DataService]
})
export class TestIndexComponent implements OnInit {

    tests: DTO[];

    constructor(private dataService: DataService) { }

    ngOnInit(): void {
        this.loadTests();
    }

    loadTests(): void {
        this.dataService.getAllTests()
            .subscribe((data: DTO[]) => {
                this.tests = data;
            });
    }
}
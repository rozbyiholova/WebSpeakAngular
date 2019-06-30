import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
    selector: 'breadcrumb',
    templateUrl: './breadcrumb.component.html'
})
export class BreadcrumbComponent implements OnInit {
    displayBreadcrumbList: any[];
    route: string;
    masterBreadcrumbList: any;
    initialUrl: any;
    lastLink: any;
    isHome: boolean;

    constructor(private router: Router, activeRoute: ActivatedRoute) {

    }

    ngOnInit() {
        this.setBreadcrumb();
    }

    setBreadcrumb() {
        this.router.events.subscribe((val) => {
            this.displayBreadcrumbList = [];

            if (location.hash !== '#/') {
                this.isHome = false;
                var paramIndex = location.hash.indexOf("?");

                if (paramIndex == -1) {
                    this.route = location.hash;
                }
                else {
                    this.route = location.hash.slice(0, paramIndex);
                }

                this.masterBreadcrumbList = this.route.split('/');
                this.masterBreadcrumbList = this.masterBreadcrumbList.slice(1, this.masterBreadcrumbList.length);

                for (let i = 0; i < this.masterBreadcrumbList.length; i++) {
                    if (i !== 0) {
                        this.initialUrl = this.displayBreadcrumbList[i - 1].url;
                    } else {
                        this.initialUrl = '/';
                    }

                    const breadCrumbObj = {
                        name: this.masterBreadcrumbList[i],
                        url: this.initialUrl + this.masterBreadcrumbList[i] + '/',
                        id: i
                    };

                    this.displayBreadcrumbList.push(breadCrumbObj);
                }

                this.displayBreadcrumbList = this.displayBreadcrumbList.slice(1);
                this.lastLink = this.displayBreadcrumbList[this.displayBreadcrumbList.length - 1];
                this.displayBreadcrumbList = this.displayBreadcrumbList.slice(0, this.displayBreadcrumbList.length - 1);
            } else {
                this.isHome = true;

                const breadCrumbObj = {
                    name: 'Home',
                    url: '#',
                    id: -1
                };

                this.lastLink = breadCrumbObj;
            }
        });
    }
}
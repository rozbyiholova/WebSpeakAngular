import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute, NavigationEnd } from "@angular/router";
import {filter} from "rxjs/operators";

interface IBreadcrumb {
    label: string;
    params?: number;
    url: string;
}

@Component({
    selector: "breadcrumbs",
    templateUrl: "./breadcrumbs.component.html",
    styleUrls: ["./breadcrumbsStyle.scss"]
})
export class BreadcrumbComponent implements OnInit {

    public breadcrumbs: IBreadcrumb[];
    
    constructor(private activatedRoute: ActivatedRoute, private router: Router) {
       
        this.breadcrumbs = [];

    }
    
    ngOnInit() {
        this.router.events.pipe(filter(event => event instanceof NavigationEnd)).subscribe(event => {
            let url: string = document.location.hash;
            let urlSplited: string[] = url.split('/');
            urlSplited = urlSplited.filter(element => element != '' && element != null && element != "#");
            urlSplited = urlSplited.map(this.deleteQueryParams);
            
            this.breadcrumbs = this.makeBreadcrumbs(urlSplited);
        });
    }

    private makeBreadcrumbs(array: string[]): IBreadcrumb[] {
        let result: IBreadcrumb[] = [];
        let currentPosition: number = 0;
        let isParams: boolean = false;

        for (currentPosition; currentPosition < array.length; currentPosition++) {
            let breadcrumb: IBreadcrumb;

            let paramIndex: number = this.findNumber(array, currentPosition);
            if (paramIndex !== -1) {isParams = true;}

            if (isParams && paramIndex == currentPosition + 1) {
                let breadcrumbUrl: string = '/' + array.slice(0, paramIndex).join('/');
                breadcrumb = {
                    label: array[paramIndex - 1],
                    params: +array[paramIndex],
                    url: breadcrumbUrl
                }
                currentPosition++;
            } else {
                let breadcrumbUrl: string = '/' + array.slice(0, currentPosition + 1).join('/');
                breadcrumb = {
                    label: array[currentPosition],
                    params: null,
                    url: breadcrumbUrl
                }
            }
            isParams = false;
            result.push(breadcrumb);
        }
        return result;
    }

    private findNumber(array: any[], startPosition: number = 0): number {
        if (array === undefined || array === null || array.length === 0) {return -1;}
        if (startPosition > array.length || startPosition < 0) { return -1; }

        for (let i = startPosition; i < array.length; i++) {
            if (!isNaN(+array[i])) { return i; }
        }
        return -1;
    }

    private deleteQueryParams(url: string): string {
        const questionMarkIndex: number = url.indexOf("?");
        if (questionMarkIndex !== -1) {
            const partToRemove: string = url.slice(questionMarkIndex);
            let res = url.replace(partToRemove, "");
            return res;
        }
        return url;
    }
}
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class DataService {
    
    private categoriesUrl: string = "/Categories";
    private subcategoriesUrl: string = "/Categories/Subcategories/";
    private wordsUrl: string = "Categories/Subcategories/Words/";
    private testIndexUrl: string = "Categories/Subcategories/Tests";
    private testUrl: string = "Categories/Subcategories/Tests/Test/";

    constructor(private http: HttpClient) {}

    getCategories() {
        return this.http.get(this.categoriesUrl);
    }

    getSubcategories(parentId: number) {
        return this.http.get(this.subcategoriesUrl + parentId);
    }

    getWords(subcategoryId: number) {
        return this.http.get(this.wordsUrl + subcategoryId);
    }

    getAllTests() {
        return this.http.get(this.testIndexUrl);
    }

    getTest(subcategoryId: number) {
        return this.http.get(this.testUrl + subcategoryId);
    }
}
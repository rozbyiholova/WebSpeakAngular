import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class DataService {
    
    private categoriesUrl: string = "Home/Categories";
    private subcategoriesUrl: string = "Home/Categories/Subcategories/";
    private wordsUrl: string = "Home/Categories/Subcategories/Words/";
    private testIndexUrl: string = "Home/Categories/Subcategories/Tests/";
    private testUrl: string = "Home/Categories/Subcategories/Tests/Test/";

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

    getAllTests(subcategoryId: number) {
        return this.http.get(this.testIndexUrl + subcategoryId);
    }

    getTest(subcategoryId: number) {
        return this.http.get(this.testUrl + subcategoryId);
    }
}
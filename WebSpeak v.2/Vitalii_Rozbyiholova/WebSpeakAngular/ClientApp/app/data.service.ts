import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Category } from '../Models/Category';

@Injectable()
export class DataService {

    private baseUrl: string = "/";
    private categoriesUrl: string = "/Categories";
    private subcategoriesUrl: string = "/Categories/Subcategories/";
    private wordsUrl: string = "Categories/Subcategories/Words/";

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
}
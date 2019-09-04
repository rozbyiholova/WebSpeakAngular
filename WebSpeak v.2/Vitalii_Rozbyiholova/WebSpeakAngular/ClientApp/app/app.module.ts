import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { Routes, RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { FooterComponent } from './app-footer/app-footer';
import { HeaderComponent } from './app-header/app-header'
import { CategoriesComponent } from './categories/categories.component';
import { SubcategoryComponent } from './subcategories/subcategories.component';
import { SlideShowComponent } from './slideshow/slideshow.component';
import { BreadcrumbComponent } from './breadcrumbs/breadcrumbs.component';
import { HomeComponent } from './home/home.component';
import { TestIndexComponent } from './Tests/index/testIndex.component';
import { TestComponent } from './Tests/test/test.component';

const routes: Routes = [
    { path: '', component: HomeComponent, pathMatch: 'full' },
    { path: 'Categories', component: CategoriesComponent },
    { path: 'Categories/Subcategories/:parentId', component: SubcategoryComponent },
    { path: 'Categories/Subcategories/:parentId/View/:subcategoryId', component: SlideShowComponent },
    { path: 'Categories/Subcategories/:parentId/Tests/:subcategoryId', component: TestIndexComponent },
    { path: 'Categories/Subcategories/:parentId/Tests/:subcategoryId/Test/:testId', component: TestComponent}
];

@NgModule({
    imports: [BrowserModule, FormsModule, HttpClientModule, NgbModule, RouterModule.forRoot(routes)],
    declarations: [AppComponent, HeaderComponent, FooterComponent,
        SubcategoryComponent, CategoriesComponent,
        SlideShowComponent, BreadcrumbComponent, HomeComponent, TestIndexComponent,
        TestComponent],
    bootstrap: [AppComponent]
})
export class AppModule { }
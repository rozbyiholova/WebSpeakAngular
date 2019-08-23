import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { Routes, RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { FooterComponent } from './app-footer/app-footer';
import { HeaderComponent } from './app-header/app-header'
import { CategoriesComponent } from './categories/categories.component';
import { SubcategoryComponent } from './subcategories/subcategories.component';
import { ManualComponent } from './manual/manual.component';

const routes: Routes = [
    { path: 'asd', component: AppComponent },
    {path: 'Categories', component: CategoriesComponent},
    { path: 'Categories/Subcategories/:parentId', component: SubcategoryComponent },
    {path: 'Categories/Subcategories/Manual/:subcategoryId', component: ManualComponent}
];

@NgModule({
    imports: [BrowserModule, FormsModule, HttpClientModule, RouterModule.forRoot(routes)],
    declarations: [AppComponent, HeaderComponent, FooterComponent,
        SubcategoryComponent, CategoriesComponent, ManualComponent],
    bootstrap: [AppComponent]
})
export class AppModule { }
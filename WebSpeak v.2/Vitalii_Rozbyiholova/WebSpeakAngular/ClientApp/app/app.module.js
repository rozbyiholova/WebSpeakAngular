var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { FooterComponent } from './app-footer/app-footer';
import { HeaderComponent } from './app-header/app-header';
import { CategoriesComponent } from './categories/categories.component';
import { SubcategoryComponent } from './subcategories/subcategories.component';
import { ManualComponent } from './manual/manual.component';
import { SlideShowComponent } from './slideshow/slideshow.component';
import { BreadcrumbComponent } from './breadcrumbs/breadcrumbs.component';
import { HomeComponent } from './home/home.component';
import { TestsComponent } from './Tests/index/tests.component';
var routes = [
    { path: '', component: HomeComponent, pathMatch: 'full' },
    { path: 'Categories', component: CategoriesComponent },
    { path: 'Categories/Subcategories/:parentId', component: SubcategoryComponent },
    { path: 'Categories/Subcategories/:parentId/Manual/:subcategoryId', component: ManualComponent },
    { path: 'Categories/Subcategories/:parentId/Slideshow/:subcategoryId', component: SlideShowComponent },
    { path: 'Categories/Subcategories/:parentId/Tests', component: TestsComponent }
];
//const routes: Routes = [
//    {
//        path: '',
//        component: HomeComponent,
//        children: [
//            {
//                path: 'Categories',
//                component: CategoriesComponent,
//                pathMatch: 'prefix',
//                data: { breadcrumb: "Categories" },
//                children: [
//                    {
//                        path: 'Subcategories/:parentId',
//                        component: SubcategoryComponent,
//                        data: { breadcrumb: "Subcategories" },
//                        children: [
//                            {
//                                path: 'Manual/:subcategoryId',
//                                component: ManualComponent,
//                                data: { breadcrumb: "Manual View" }
//                            },
//                            {
//                                path: 'Slideshow/:subcategoryId',
//                                component: SlideShowComponent,
//                                data: { breadcrumb: "Slide-show" }
//                            }
//                        ]
//                    }
//                ]
//            }
//        ]
//    }
//];
var AppModule = /** @class */ (function () {
    function AppModule() {
    }
    AppModule = __decorate([
        NgModule({
            imports: [BrowserModule, FormsModule, HttpClientModule, NgbModule, RouterModule.forRoot(routes)],
            declarations: [AppComponent, HeaderComponent, FooterComponent,
                SubcategoryComponent, CategoriesComponent, ManualComponent,
                SlideShowComponent, BreadcrumbComponent, HomeComponent, TestsComponent],
            bootstrap: [AppComponent]
        })
    ], AppModule);
    return AppModule;
}());
export { AppModule };
//# sourceMappingURL=app.module.js.map
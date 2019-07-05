import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { Routes, RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ReactiveFormsModule } from '@angular/forms';

import { AppComponent } from './components/app.component';
import { NavComponent } from './components/home/nav/nav.component';
import { HomeComponent } from './components/home/home/home.component';
import { CategoriesComponent } from './components/home/categories/categories.component';
import { SubCategoriesComponent } from './components/home/sub-categories/sub-categories.component';
import { TestsComponent } from './components/home/tests/tests.component';
import { BreadcrumbComponent } from './components/home/breadcrumb/breadcrumb.component';
import { ManualComponent } from './components/home/manual/manual.component';
import { SlideshowComponent } from './components/home/slideshow/slideshow.component';
import { TestComponent } from './components/home/test/test.component';
import { RegisterComponent } from './components/home/register/register.component';
import { LoginComponent } from './components/home/login/login.component';
import { ManageComponent } from './components/account/manage/manage.component';
import { ManageNavComponent } from './components/account/manage-nav/manage-nav.component';
import { AccountIndexComponent } from './components/account/index/index.component';
import { AccountPersonalInfoComponent } from './components/account/personal-info/personal-info.component';
import { AccountChangePasswordComponent } from './components/account/change-password/change-password.component';
import { AccountStatisticsComponent } from './components/account/statistics/statistics.component';
import { AccountRatingComponent } from './components/account/rating/rating.component';
import { ExternalLoginComponent } from './components/home/external-login/external-login.component';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations'

import { EventEmitterService } from './services/event-emitter.service'

const manageRoutes: Routes = [
    { path: 'Index', component: AccountIndexComponent },
    { path: 'PersonalInfo', component: AccountPersonalInfoComponent },
    { path: 'ChangePassword', component: AccountChangePasswordComponent },
    { path: 'Statistics', component: AccountStatisticsComponent },
    { path: 'Rating', component: AccountRatingComponent }
];

const appRoutes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'Home/Categories', component: CategoriesComponent },
    { path: 'Home/Categories/SubCategories', component: SubCategoriesComponent },
    { path: 'Home/Categories/SubCategories/Tests', component: TestsComponent },
    { path: 'Home/Categories/SubCategories/Tests/Manual', component: ManualComponent },
    { path: 'Home/Categories/SubCategories/Tests/Slideshow', component: SlideshowComponent },
    { path: 'Home/Categories/SubCategories/Tests/Test', component: TestComponent },
    { path: 'Account/Manage', component: ManageComponent, children: manageRoutes },
    { path: 'Account/Register', component: RegisterComponent },
    { path: 'Account/Login', component: LoginComponent },
    { path: 'Account/Callback', component: ExternalLoginComponent },
    { path: '**', redirectTo: '/' }
];

@NgModule({
    imports: [BrowserModule, FormsModule, HttpClientModule, RouterModule.forRoot(appRoutes, { useHash: true }),
              NgbModule, ReactiveFormsModule, BrowserAnimationsModule],
    declarations: [AppComponent, HomeComponent, CategoriesComponent, SubCategoriesComponent, TestsComponent,
                   NavComponent, BreadcrumbComponent, ManualComponent, SlideshowComponent, TestComponent,
                   RegisterComponent, LoginComponent, ManageComponent, ManageNavComponent, AccountIndexComponent,
                   AccountPersonalInfoComponent, AccountChangePasswordComponent, AccountStatisticsComponent,
                   AccountRatingComponent, ExternalLoginComponent],
    providers: [EventEmitterService],
    bootstrap: [AppComponent]
})
export class AppModule { }
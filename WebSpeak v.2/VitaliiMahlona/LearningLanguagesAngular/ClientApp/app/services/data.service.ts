import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DTO } from '../models/DTO';
import { DOCUMENT } from '@angular/common';

@Injectable()
export class DataService {

    private homeUrl = "/Home/Index";
    private categoriesUrl = "/Home/Categories";
    private subCategoriesUrl = "/Home/Categories/SubCategories";
    private testsUrl = "/Home/Categories/SubCategories/Tests";
    private manualUrl = "/Home/Categories/SubCategories/Tests/Manual";
    private testUrl = "/Home/Categories/SubCategories/Tests/Test";
    private accountIndexUrl = "/Account/Manage/Index";
    private accountPersonalInfoUrl = "/Account/Manage/PersonalInfo";
    private accountChangePasswordUrl = "/Account/Manage/ChangePassword";
    private accountStatisticsUrl = "/Account/Manage/Statistics";
    private accountRatingUrl = "/Account/Manage/Rating";
    private registerUrl = "/Account/Register";
    private loginUrl = "/Account/Login";
    private externalLoginUrl = "/Account/Login/ExternalLogin";
    private callbackUrl = "/Account/Callback";
    private usersInfoUrl = "/Account/UsersInfo";
    private logoutUrl = "/Account/Logout";

    constructor(@Inject(DOCUMENT) private document: Document, private http: HttpClient) {
    }

    getCategories() {
        return this.http.get(this.categoriesUrl);
    }

    setSession() {
        return this.http.get(this.homeUrl, { responseType: 'text' });
    }

    getSubCategories(id: number) {
        return this.http.get(this.subCategoriesUrl + '?id=' + id);
    }

    getTests(id: number) {
        return this.http.get(this.testsUrl + '?id=' + id);
    }

    getWords(id: number) {
        return this.http.get(this.manualUrl + '?id=' + id);
    }

    setResultTest(data: any) {
        return this.http.post(this.testUrl, data);
    }

    getSettings() {
        return this.http.get(this.accountIndexUrl);
    }

    setSettings(settings: DTO) {
        return this.http.post(this.accountIndexUrl, settings);
    }

    getPersonalInfo() {
        return this.http.get(this.accountPersonalInfoUrl);
    }

    setPersonalInfo(data: any) {
        return this.http.post(this.accountPersonalInfoUrl, data);
    }

    setPassword(data: any) {
        return this.http.post(this.accountChangePasswordUrl, data);
    }

    getStatistics() {
        return this.http.get(this.accountStatisticsUrl);
    }

    getRating() {
        return this.http.get(this.accountRatingUrl);
    }

    register(user: any) {
        return this.http.post(this.registerUrl, user);
    }

    loginGet(returnUrl: string) {
        return this.http.get(this.loginUrl + '?returnUrl=' + returnUrl);
    }

    loginPost(user: any) {
        return this.http.post(this.loginUrl, user);
    }

    externalLogin(provider: string, returnUrl: string) {
        var externalLogin = {
            provider,
            returnUrl
        };

        //return this.http.post(this.externalLoginUrl, externalLogin);
        var url = "https://localhost:44374/Account/Login/ExternalLogin?provider=" + provider + "&returnUrl=" + returnUrl;
        this.document.location.href = url;
    }

    callbackGet(returnUrl: string, remoteError: string) {
        if (remoteError) {
            return this.http.get(this.callbackUrl + '?returnUrl=' + returnUrl + "&remoteError=" + remoteError);
        }

        return this.http.get(this.callbackUrl + '?returnUrl=' + returnUrl);
    }

    callbackPost(data: any) {
        return this.http.post(this.callbackUrl, data);
    }

    getUsersInfo() {
        return this.http.get(this.usersInfoUrl);
    }

    logout() {
        return this.http.get(this.logoutUrl);
    }
}
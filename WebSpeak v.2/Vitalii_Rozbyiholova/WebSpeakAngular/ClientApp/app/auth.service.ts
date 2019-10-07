import { Injectable, EventEmitter, Output } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { BehaviorSubject, Subject } from "rxjs";
import { JwtHelperService } from "@auth0/angular-jwt";
import { LoginModel } from "../Models/LoginModel";
import { User } from "../Models/User";

@Injectable()
export class AuthService {
    @Output() loggedIn = new EventEmitter<boolean>();

    private readonly loginUrl: string = "api/auth/Login";
    private readonly registerUrl: string = "api/auth/Register";
    private readonly usersLoginsUrl: string = "api/auth/UsersLogins";
    private readonly getUserUrl: string = "api/auth/User/";
    private readonly getLanguagesUrl: string = "api/auth/Languages";

    constructor(private http: HttpClient, private jwtHelper: JwtHelperService) { }

    public login(user: LoginModel) {
        return this.http.post(this.loginUrl, user,
            {
                headers: new HttpHeaders({ "ContentType": "application/json" })
            });

    }

    public register(user: User) {
        return this.http.post(this.registerUrl, user,
            {
                headers: new HttpHeaders({ "ContentType": "application/json" })
            });
    }

    public getUser() {
        const str = encodeURIComponent(this.getDecodedToken()["userLogin"].toString());
        return this.http.get(this.getUserUrl + str);
    } 

    public getLanguages() {
        return this.http.get(this.getLanguagesUrl);
    }

    public isLoggedIn(): boolean {
        if (localStorage.getItem("jwt")) {
            return !this.jwtHelper.isTokenExpired();
        }
        return false;
    }

    public usersLogins() {
        return this.http.get(this.usersLoginsUrl);
    }

    public getDecodedToken() {
        return this.jwtHelper.decodeToken();
    }


    /*--------------------Work with events--------------------*/

    public notifyLogin(name: string): void {
       this.loggedIn.emit(true);
    }

    public notifyLogOut() {
        this.loggedIn.emit(false);
    }
}
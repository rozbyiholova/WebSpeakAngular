import { Injectable, Output, EventEmitter } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { LoginModel } from "../Models/LoginModel";
import { User } from "../Models/User";

@Injectable()
export class AuthService {

    @Output() loggedIn: EventEmitter<string> = new EventEmitter();

    private readonly loginUrl: string = "api/auth/Login";
    private readonly registerUrl: string = "api/auth/Register";
    private readonly usersLoginsUrl: string = "api/UsersLogins";

    constructor(private http: HttpClient) { }

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

    public isLoggedIn(): boolean {
        return localStorage.getItem("jwt") !== null;
    }

    public emitLogin(name: string): void {
        this.loggedIn.emit(name);
        console.log("emitted login event");
    }

    public usersLogins() {
        return this.http.get(this.usersLoginsUrl);
    }
}
import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { LoginModel } from "../Models/LoginModel";

@Injectable()
export class AuthService {
    private readonly loginUrl: string = "api/auth/login";

    constructor(private http: HttpClient) { }

    public login(user: LoginModel) {
        return this.http.post(this.loginUrl, user,
            {
                 headers: new HttpHeaders({ "ContentType": "application/json" })
            });
    }
}
import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { AuthService } from "../../auth.service";
import { User } from "../../../Models/User";

@Component({
    selector: "user-cabinet",
    templateUrl: "./cabinet.component.html",
    styleUrls: ["./cabinet.component.scss"]
})
export class CabinetComponent implements OnInit {

    user: User;

    constructor(private auth: AuthService, private router: Router) {

    }

    ngOnInit(): void {
        this.auth.getUser().subscribe(u => {
            this.user = u["user"] as User;
        });
    }

    public logOut(): void {
        localStorage.removeItem("jwt");
        this.auth.notifyLogOut();
        this.router.navigate(["/"]);
    }
}
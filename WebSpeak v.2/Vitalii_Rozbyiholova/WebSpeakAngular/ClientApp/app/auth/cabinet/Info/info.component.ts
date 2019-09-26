import { Component, OnInit } from "@angular/core";
import { AuthService } from "../../../auth.service";
import { User } from "../../../../Models/User";

@Component({
    selector: "user-info",
    templateUrl: "./info.component.html",
    styleUrls: []
})
export class InfoComponent implements OnInit {

    user: User;
    languages: object[];

    constructor(private auth: AuthService) {

    }

    ngOnInit(): void {
        this.auth.getUser().subscribe((u) => {
            this.user = new User();
            Object.assign(this.user, u);
            console.dir(this.user);
            console.log(this.user instanceof User);
        });

        this.auth.getLanguages().subscribe((languages: Object[]) => {
            this.languages = languages;
        });
    }
}
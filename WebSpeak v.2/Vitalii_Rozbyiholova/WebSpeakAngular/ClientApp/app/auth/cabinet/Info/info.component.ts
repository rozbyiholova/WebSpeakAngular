import { Component, OnInit } from "@angular/core";
import { AuthService } from "../../../auth.service";
import { User } from "../../../../Models/User";

@Component({
    selector: "user-info",
    templateUrl: "./info.component.html",
    styleUrls: []
})
export class InfoComponent implements OnInit {

    isUser: boolean = false;
    private user: User;
    public nativeLanguageId: any;
    public learningLanguageId: any;
    languages: object[];
    public userName: string;
    public userEmail: string;


    constructor(private auth: AuthService) {}

    ngOnInit(): void {
        this.auth.getUser().subscribe((u) => {
            this.user = new User();
            Object.assign(this.user, u["user"]);
            this.userEmail = this.user.Email;
            this.userName = this.user.UserName;
            this.isUser = true;
        });

        this.auth.getLanguages().subscribe((languages: Object[]) => {
            this.languages = languages;
        });
    }

    changeLanguages(): void {
        this.auth.setLanguages(this.userEmail, this.nativeLanguageId, this.learningLanguageId);
    }
}
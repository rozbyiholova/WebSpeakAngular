import { UserSettings } from "./UserSettings";
export class User {
    public Id: string;
    public UserName: string;
    public Email: string;
    public PasswordHash: string;
    public UserSettings: UserSettings[];
}
import { UserSettings } from "./UserSettings";
export class User {
    public id: string;
    public userName: string;
    public email: string;
    public passwordHash: string;
    public userSettings: UserSettings[];
}
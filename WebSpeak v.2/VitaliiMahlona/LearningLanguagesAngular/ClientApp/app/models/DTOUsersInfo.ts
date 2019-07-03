import { Users } from "./Users"

export class DTOUsersInfo {
    public currentUser: Users;
    public isSignedIn: boolean;
    public avatar: string;
}
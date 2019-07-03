import { AuthenticationScheme } from "./AuthenticationScheme"
import { Users } from "./Users"

export class LoginViewModel {
    public email: string;
    public password: string;
    public rememberMe: boolean;
    public returnUrl: string;
    public errorMessage: string;
    public externalLogins: AuthenticationScheme[];
}
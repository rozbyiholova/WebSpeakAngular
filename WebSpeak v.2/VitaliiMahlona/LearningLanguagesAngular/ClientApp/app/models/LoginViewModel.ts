import { AuthenticationScheme } from "./AuthenticationScheme"

export class LoginViewModel {
    public email: string;
    public password: string;
    public rememberMe: boolean;
    public returnUrl: string;
    public errorMessage: string;
    public externalLogins: AuthenticationScheme[];
}
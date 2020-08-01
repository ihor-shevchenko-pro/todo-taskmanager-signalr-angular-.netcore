import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs';
import { ISignInResponse } from '../interfaces/account/signin-response';
import { accountEndpoints } from '../configurations/endpoints-config';
import { RegistrationModel } from '../models/account/registration-model';
import { LoginModel } from '../models/account/login-model';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private readonly _loginUrl = accountEndpoints.login;
  private readonly _registrationUrl = accountEndpoints.registration;

  constructor(private _httpClient: HttpClient,
              private _router: Router) { }

  public login(loginFormModel: FormGroup): Observable<ISignInResponse> {
    let loginModel = new LoginModel();
    loginModel.login = loginFormModel.value.Login;
    loginModel.password = loginFormModel.value.Password;
    return this._httpClient.post<ISignInResponse>(this._loginUrl, loginModel);
  }

  public registration(registerFormModel: FormGroup): Observable<ISignInResponse> {
    let registrationModel = new RegistrationModel();
    registrationModel.login = registerFormModel.value.Login;
    registrationModel.password = registerFormModel.value.Password;
    registrationModel.user_name = registerFormModel.value.UserName;
    return this._httpClient.post<ISignInResponse>(this._registrationUrl, registrationModel);
  }

  public checkAutTokenInLocalStorage(): boolean {
    if (localStorage.getItem('token') != null) {
      return true;
    }
    else {
      return false;
    }
  }

  public getBearerToken(): string {
    return localStorage.getItem('token');
  }

  public onLogout(): void {   
    localStorage.removeItem('token');
    this._router.navigateByUrl('/account/login');
  }

}

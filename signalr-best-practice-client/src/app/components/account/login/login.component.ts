import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from 'src/core/services/account.service';
import { NotificationService } from 'src/core/services/notification.service';
import { ISignInResponse } from 'src/core/interfaces/account/signin-response';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styles: []
})
export class LoginComponent implements OnInit {

  constructor(private _accountService: AccountService,
              private _router: Router,
              private _formBuilder: FormBuilder,
              private _notifyService: NotificationService) {
  }

  // form validator
  public loginFormModel = this._formBuilder.group({
    Login: ['', [Validators.required, Validators.email]],
    Password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(20)]],
  });

  ngOnInit() {
    this.loginFormModel.reset();
    // check authorization
    let isAuthorized: boolean = this._accountService.checkAutTokenInLocalStorage();
    if(isAuthorized) this._router.navigateByUrl('/home');
  }

  public onLoginFormSubmit() {
    this._accountService.login(this.loginFormModel).subscribe(
      (res: ISignInResponse) => {
        // console.log(res);
        if (res.id != null && res.token != null) {
          this.loginFormModel.reset();
          localStorage.setItem('token', res.token);
          this._router.navigateByUrl('/home');
        }
      },
      errors => {
        // console.log(errors);
        errors.error.errors.forEach(element => {
          switch (element) {
            case 'Entity is not found':
              this._notifyService.showError("Invalid login or password", "");
              break;

            default:
              this._notifyService.showError("Authentication error", "");
              break;
          }
        });
      }
    );
  }
}

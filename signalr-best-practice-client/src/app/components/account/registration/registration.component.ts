import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/core/services/account.service';
import { FormBuilder, Validators } from '@angular/forms';
import { NotificationService } from 'src/core/services/notification.service';
import { Router } from '@angular/router';
import { ISignInResponse } from 'src/core/interfaces/account/signin-response';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styles: []
})
export class RegistrationComponent implements OnInit {

  constructor(private _accountService: AccountService,
              private _notifyService: NotificationService,
              private _formBuilder: FormBuilder,
              private _router: Router) { }

  public registerFormModel = this._formBuilder.group({
    UserName: [null,[Validators.required]],
    Login: [null, [Validators.required, Validators.email]],
    Password: [null, [Validators.required, Validators.minLength(4), Validators.maxLength(20)]],
  });

  ngOnInit() {
    this.registerFormModel.reset();
    // check authorization
    let isAuthorized: boolean = this._accountService.checkAutTokenInLocalStorage();
    if(isAuthorized) this._router.navigateByUrl('/home');
  }

  public onRegisterFormSubmit() {
    this._accountService.registration(this.registerFormModel).subscribe(
      (res: ISignInResponse) => {
        // console.log(res);
        if (res.refresh_token != null) {
          this._notifyService.showSuccess("User created successfully", "");
          const url = '/account/login';
          this._router.navigateByUrl(url);
        }
      },
      errors => {
        // console.log(errors);
        errors.error.errors.forEach(element => {
          switch (element) {
            case 'User with this email already exist':
              this._notifyService.showError("User with this email already exist", "");
              break;
            case 'User with this userName already exist':
              this._notifyService.showError("User with this userName already exist", "");
              break;

            default:
              this._notifyService.showError("Registration error", "");
              break;
          }
        });
      }
    );
  }

}

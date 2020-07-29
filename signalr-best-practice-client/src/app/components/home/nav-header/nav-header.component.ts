import { Component, OnInit } from '@angular/core';
import { IUser } from 'src/core/interfaces/user/user';
import { Router } from '@angular/router';
import { UserService } from 'src/core/services/user.service';
import { AccountService } from 'src/core/services/account.service';

@Component({
  selector: 'app-nav-header',
  templateUrl: './nav-header.component.html',
  styleUrls: ['./nav-header.component.css']
})
export class NavHeaderComponent implements OnInit {

  public userName: string = null;
  public email: string = null;

  constructor(private _router: Router, 
              private _userService: UserService,
              private _accountService: AccountService) { }

  ngOnInit(): void {
    this.getCurrentUser();
  }

  private getCurrentUser() {
    this._userService.getCurrentUser().subscribe(
      (res: IUser) => {
        // console.log(res);
        this.userName = res.user_name;
        this.email = res.email;
      },
      errors => {
        // console.log(errors);
      }
    );
  }

  public onLogout(): void {
    this._accountService.onLogout();
  }

}

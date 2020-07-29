import { Component, OnInit, OnDestroy, HostListener, DoCheck } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UserService } from 'src/core/services/user.service';
import { IUser } from 'src/core/interfaces/user/user';
import { IPaginationResponse } from 'src/core/interfaces/base/pagination-response';
import { EntitySortingEnum } from 'src/core/models/enum';
import { SignalrManager } from 'src/core/managers/signalr-manager';
import { AccountService } from 'src/core/services/account.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent extends SignalrManager implements OnInit, DoCheck, OnDestroy {

  private start: number = 0;
  private count: number = 100;
  private sort: EntitySortingEnum = EntitySortingEnum.ByCreate;
  public users: IUser[] = [];

  constructor(private _modalService: NgbModal,
              public accountService: AccountService,
              private _userService: UserService) {
    super(accountService);
    this.initializeHubConnection();
    this.handleNotification();
  }

  ngOnInit(): void {
    this.getUsers();
    this.handleNotification();
  }

  ngDoCheck(): void {
    if(this._signalRNotification != null){
      this._signalRNotification = null;
      this.getUsers();
    }
  }

  ngOnDestroy(): void {
    this.onDestroy();
  }

  @HostListener('window:beforeunload')
    onDestroy() {
    this.terminateConnection();
  }

  private getUsers(): void {
    this._userService.getUsers(this.start, this.count, this.sort).subscribe(
      (res: IPaginationResponse<IUser>) => {
        // console.log(res);
        if (res.models.length > 0) {
          this.users = res.models;
        };
      },
      errors => {
        // console.log(errors);
      }
    );
  }

}

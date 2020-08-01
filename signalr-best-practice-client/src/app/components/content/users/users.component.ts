import { Component, OnInit, DoCheck } from '@angular/core';
import { UserService } from 'src/core/services/user.service';
import { IUser } from 'src/core/interfaces/user/user';
import { IPaginationResponse } from 'src/core/interfaces/base/pagination-response';
import { EntitySortingEnum, ModelTypeEnum, NotificationStatusEnum, NotificationTypeEnum } from 'src/core/models/enum';
import { AccountService } from 'src/core/services/account.service';
import { Router } from '@angular/router';
import { SignalRService } from 'src/core/services/signal-r.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit, DoCheck {
  public currentUserEmail: string;
  private start: number = 0;
  private count: number = 100;
  private sort: EntitySortingEnum = EntitySortingEnum.ByCreate;
  public users: IUser[] = [];
  public touserid: string;

  constructor(private _router: Router,
    public accountService: AccountService,
    private _userService: UserService,
    private _signalRService: SignalRService) {
    // try to start SignalRConnection
    if (SignalRService.isConnected == false) {
      this._signalRService.startSignalRConnection();
    }
    // launch SignalRNotification handler
    this._signalRService.handleSignalRNotification();
  }

  ngOnInit(): void {
    this.getUsers();
    // try to start SignalRConnection
    if (SignalRService.isConnected == false) {
      this._signalRService.startSignalRConnection();
    }
    // launch SignalRNotification handler
    this._signalRService.handleSignalRNotification();
    // get currentUserEmail
    if (UserService.currentUser != null) {
      this.currentUserEmail = UserService.currentUser.email;
    } else {
      this.getCurrentUserEmail();
    }
  }

  ngDoCheck(): void {
    // handle SignalRNotification
    this.signalRNotificationHandle();
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

  private getCurrentUserEmail() {
    this._userService.getCurrentUser().subscribe(
      (res: IUser) => {
        // console.log(res);
        this.currentUserEmail = res.email;
      },
      errors => {
        // console.log(errors);
      }
    );
  }

  public redirectToCreateTask(userId: string = null) {
    if(userId != null){
      this._router.navigateByUrl('/home/add_todotask/' + userId);
    }else{
      this._router.navigateByUrl('/home/add_todotask/' + "all");
    }
  }

  private signalRNotificationHandle(): void {
    // New user has added to the App 
    if (SignalRService.signalRNotification != null &&
      SignalRService.signalRNotification.NotificationDataType == ModelTypeEnum.User &&
      SignalRService.signalRNotification.NotificationStatus == NotificationStatusEnum.New &&
      (SignalRService.signalRNotification.NotificationType == NotificationTypeEnum.ModelAdd ||
        NotificationTypeEnum.ModelUpdate || NotificationTypeEnum.ModelDelete ||
        NotificationTypeEnum.ModelChangeStatus)) {
      this.getUsers();
      SignalRService.signalRNotification.NotificationStatus = NotificationStatusEnum.Received;
    }
  }

}

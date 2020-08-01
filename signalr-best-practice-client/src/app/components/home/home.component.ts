import { Component, OnInit, DoCheck } from '@angular/core';
import { UserService } from 'src/core/services/user.service';
import { IUser } from 'src/core/interfaces/user/user';
import { SignalRService } from 'src/core/services/signal-r.service';
import { ModelTypeEnum, NotificationStatusEnum, NotificationTypeEnum } from 'src/core/models/enum';
import { NotificationService } from 'src/core/services/notification.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styles: []
})
export class HomeComponent implements OnInit, DoCheck {

  public userName: string;

  constructor(private _userService: UserService,
              private _signalRService: SignalRService,
              private _notifyService: NotificationService) {
    // try to start SignalRConnection
    if (SignalRService.isConnected == false) {
      this._signalRService.startSignalRConnection();
    }
    // // launch SignalRNotification handler
    // this._signalRService.handleSignalRNotification();
  }

  ngOnInit() {
    this.getCurrentUser();
    // try to start SignalRConnection
    if (SignalRService.isConnected == false) {
      this._signalRService.startSignalRConnection();
    }
  }

  ngDoCheck(): void {
    // handle SignalRNotification
    this.signalRNotificationHandle();
  }

  private getCurrentUser() {
    this._userService.getCurrentUser().subscribe(
      (res: IUser) => {
        // console.log(res);
        this.userName = res.user_name;
        // set currentUser into service
        this._userService.setCurrentUserData(res);
      },
      errors => {
        // console.log(errors);
      }
    );
  }

  private signalRNotificationHandle(): void {
    // New task
    if (SignalRService.signalRNotification != null &&
      SignalRService.signalRNotification.NotificationDataType == ModelTypeEnum.ToDoTask &&
      SignalRService.signalRNotification.NotificationStatus == NotificationStatusEnum.New &&
      SignalRService.signalRNotification.NotificationType == NotificationTypeEnum.ModelAdd) {
        this._notifyService.showInfo("You have received new task", "");
      SignalRService.signalRNotification.NotificationStatus = NotificationStatusEnum.Received;
    }
    // Task progress-status has been changed
    if (SignalRService.signalRNotification != null && 
      SignalRService.signalRNotification.NotificationDataType == ModelTypeEnum.ToDoTask &&
      SignalRService.signalRNotification.NotificationStatus == NotificationStatusEnum.New &&
      SignalRService.signalRNotification.NotificationType == NotificationTypeEnum.ChangeProgressStatus){
        this._notifyService.showWarning("Progress-status of your task has been changed", "");
        SignalRService.signalRNotification.NotificationStatus = NotificationStatusEnum.Received;
    }
  }

}

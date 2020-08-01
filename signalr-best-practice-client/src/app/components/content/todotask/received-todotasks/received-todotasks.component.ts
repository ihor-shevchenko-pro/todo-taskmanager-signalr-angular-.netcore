import { Component, OnInit, ViewEncapsulation, DoCheck } from '@angular/core';
import { ITodotask } from 'src/core/interfaces/todotask/todotask';
import { EntitySortingEnum, ToDoTaskStatusEnum, ModelTypeEnum, NotificationStatusEnum, NotificationTypeEnum } from 'src/core/models/enum';
import { TodotaskService } from 'src/core/services/todotask.service';
import { IPaginationResponse } from 'src/core/interfaces/base/pagination-response';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NotificationService } from 'src/core/services/notification.service';
import { ISuccessResponse } from 'src/core/interfaces/base/success-response';
import { SignalRService } from 'src/core/services/signal-r.service';

@Component({
  selector: 'app-received-todotasks',
  templateUrl: './received-todotasks.component.html',
  styleUrls: ['./received-todotasks.component.css'],
  encapsulation: ViewEncapsulation.None,
  styles: [`
  .dark-modal .modal-content {
    background-color: #292b2c!important;
    color: white!important;
  }
  .dark-modal .close {
    color: white;
  }
  .light-blue-backdrop {
    background-color: #5cb3fd;
  }
`]
})
export class ReceivedTodotasksComponent implements OnInit, DoCheck {

  public receivedTodotasks: ITodotask[] = [];
  public userTaskDetails: ITodotask;
  public taskIdForProgressStatus: string = null;
  private start: number = 0;
  private count: number = 100;
  private sort: EntitySortingEnum = EntitySortingEnum.ByCreate;
  public progressStatusData = [];
  public dropdownSettingsProgressStatus = {};
  public progressStatusEnumKey: string = null;

  constructor(private _todotaskService: TodotaskService,
              private _modalService: NgbModal,
              private _notifyService: NotificationService) { }

  ngOnInit(): void {
    this.getReceivedTodotasks();
    this.progressStatusData = [
      { item_id: '0', item_text: 'New' },
      { item_id: '1', item_text: 'In progress' },
      { item_id: '2', item_text: 'Complited' },
    ];
    this.dropdownSettingsProgressStatus = {
      singleSelection: true,
      idField: 'item_id',
      textField: 'item_text',
      itemsShowLimit: 3,
      // limitSelection: 1,
      allowSearchFilter: false
    };
  }

  ngDoCheck(): void {
    // handle SignalRNotification
    this.signalRNotificationHandle();
  }

  private getReceivedTodotasks(): void {
    this._todotaskService.getReceivedTodotasks(this.start, this.count, this.sort).subscribe(
      (res: IPaginationResponse<ITodotask>) => {
        // console.log(res);
        if (res.models.length > 0) {
          this.receivedTodotasks = res.models;
        };
      },
      errors => {
        // console.log(errors);
      }
    );
  }

  public openTaskDetailsModal(content, taskId: string): void {
    this.receivedTodotasks.forEach(element => {
      if (element.id == taskId) this.userTaskDetails = element;
    });
    this._modalService.open(content, { size: 'lg', windowClass: 'dark-modal', backdropClass: 'light-blue-backdrop' });
  }

  public openProgressStatusOptionsModal(content, taskId: string) {
    this.taskIdForProgressStatus = taskId;
    this._modalService.open(content, { windowClass: 'dark-modal', backdropClass: 'light-blue-backdrop' });
  }

  public onItemProgressStatusSelect(item: any): void {
    this.progressStatusEnumKey = item.item_id;
  }

  public changeProgressStatus(): void {
    let progressStatus = this.GetProgressStatus(this.progressStatusEnumKey);
    this._modalService.dismissAll();
    this._todotaskService.changeTaskProgressStatus(this.taskIdForProgressStatus, progressStatus).subscribe(
      (res: ISuccessResponse) => {
        // console.log(res);
        if (res.response == "success") {
          this.taskIdForProgressStatus = null;
          this.progressStatusEnumKey = null;
          this.getReceivedTodotasks();
        }
      },
      errors => {
        // console.log(errors);
        errors.error.errors.forEach(element => {
          switch (element) {
            case 'Entity is not found':
              this._notifyService.showError("Todotask is not found", "Error of changing progress-status");
              break;
            case 'Todotask is not found':
              this._notifyService.showError("Todotask is not found", "Error of changing progress-status");
              break;
            case 'Access denied for changing progress-status of todotask':
              this._notifyService.showError("Access denied", "Error of changing progress-status");
              break;
            case 'Access denied. You can\'t change progress-status of todotasks which are Cancelled':
              this._notifyService.showError("Access denied. You can't change progress-status of todotasks which are Cancelled", "Error of changing progress-status");
              break;
            case 'Access denied for cancelling todotask':
              this._notifyService.showError("Access denied for cancelling todotask", "Error of changing progress-status");
              break;

            default:
              this._notifyService.showError("Error of changing progress-status", "");
              break;
          }
        });
      }
    );
  }

  private GetProgressStatus(key: string): ToDoTaskStatusEnum {
    if (key == '0') return ToDoTaskStatusEnum.New;
    if (key == '1') return ToDoTaskStatusEnum.InProgress;
    if (key == '2') return ToDoTaskStatusEnum.Complited;
  }

  private signalRNotificationHandle(): void {
    // Receiving new task
    if (SignalRService.signalRNotification != null &&
      SignalRService.signalRNotification.NotificationDataType == ModelTypeEnum.ToDoTask &&
      SignalRService.signalRNotification.NotificationStatus == NotificationStatusEnum.Received &&
      SignalRService.signalRNotification.NotificationType == NotificationTypeEnum.ModelAdd) {
        this.getReceivedTodotasks();
        SignalRService.signalRNotification.NotificationStatus = NotificationStatusEnum.InProgress;
    }
    // Task progress-status has been changed
    if (SignalRService.signalRNotification != null &&
      SignalRService.signalRNotification.NotificationDataType == ModelTypeEnum.ToDoTask &&
      SignalRService.signalRNotification.NotificationStatus == NotificationStatusEnum.Received &&
      SignalRService.signalRNotification.NotificationType == NotificationTypeEnum.ChangeProgressStatus) {
        this.getReceivedTodotasks();
        SignalRService.signalRNotification.NotificationStatus = NotificationStatusEnum.InProgress;
    }
  }

}

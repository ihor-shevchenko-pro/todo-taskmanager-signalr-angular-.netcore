import { Component, OnInit, ViewEncapsulation, DoCheck } from '@angular/core';
import { TodotaskService } from 'src/core/services/todotask.service';
import { IPaginationResponse } from 'src/core/interfaces/base/pagination-response';
import { ITodotask } from 'src/core/interfaces/todotask/todotask';
import { EntitySortingEnum, ToDoTaskStatusEnum, ModelTypeEnum, NotificationStatusEnum, NotificationTypeEnum } from 'src/core/models/enum';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ISuccessResponse } from 'src/core/interfaces/base/success-response';
import { NotificationService } from 'src/core/services/notification.service';
import { SignalRService } from 'src/core/services/signal-r.service';

@Component({
  selector: 'app-sent-todotasks',
  templateUrl: './sent-todotasks.component.html',
  styleUrls: ['./sent-todotasks.component.css'],
  encapsulation: ViewEncapsulation.None,
  styles: [`
    .dark-modal .modal-content {
      background-color: #292b2c;
      color: white;
    }
    .dark-modal .close {
      color: white;
    }
    .light-blue-backdrop {
      background-color: #5cb3fd;
    }
  `]
})
export class SentTodotasksComponent implements OnInit, DoCheck {

  public sentTodotasks: ITodotask[] = [];
  public userTaskDetails: ITodotask;
  private start: number = 0;
  private count: number = 10000;
  private sort: EntitySortingEnum = EntitySortingEnum.ByCreate;

  constructor(private _todotaskService: TodotaskService,
              private _modalService: NgbModal,
              private _notifyService: NotificationService) { }

  ngOnInit(): void {
    this.getSentTodotasks();
  }

  ngDoCheck(): void {
    // handle SignalRNotification
    this.signalRNotificationHandle();
  }


  private getSentTodotasks(): void {
    this._todotaskService.getSentTodotasks(this.start, this.count, this.sort).subscribe(
      (res: IPaginationResponse<ITodotask>) => {
        // console.log(res);
        if (res.models.length > 0) {
          this.sentTodotasks = res.models;
        };
      },
      errors => {
        // console.log(errors);
      }
    );
  }

  public openTaskDetailsModal(content, taskId: string): void {
    this.sentTodotasks.forEach(element => {
      if (element.id == taskId) this.userTaskDetails = element;
    });
    this._modalService.open(content, { size: 'lg', windowClass: 'dark-modal', backdropClass: 'light-blue-backdrop' });
  }

  public cancellTask(taskId: string): void {
    this._todotaskService.changeTaskProgressStatus(taskId, ToDoTaskStatusEnum.Cancelled).subscribe(
      (res: ISuccessResponse) => {
        // console.log(res);
        if (res.response == "success") {
          this.getSentTodotasks();
        }
      },
      errors => {
        // console.log(errors);
        errors.error.errors.forEach(element => {
          switch (element) {
            case 'Entity is not found':
              this._notifyService.showError("Todotask is not found", "Error of cancelling task");
              break;
            case 'Todotask is not found':
              this._notifyService.showError("Todotask is not found", "Error of cancelling task");
              break;
            case 'Access denied for changing progress-status of todotask':
              this._notifyService.showError("Access denied", "Error of cancelling task");
              break;
            case 'Access denied. You can cancell only tasks which are in progress-status - NEW':
              this._notifyService.showError("Access denied. You can cancell only tasks which are in progress-status - NEW", "Error of cancelling task");
              break;
            case 'Access denied. You have access only for cancelling task':
              this._notifyService.showError("Access denied. You have access only for cancelling task", "Error of cancelling task");
              break;

            default:
              this._notifyService.showError("Error of cancelling task", "");
              break;
          }
        });
      }
    );
  }

  private signalRNotificationHandle(): void {
    // Task progress-status has been changed
    if (SignalRService.signalRNotification != null &&
      SignalRService.signalRNotification.NotificationDataType == ModelTypeEnum.ToDoTask &&
      SignalRService.signalRNotification.NotificationStatus == NotificationStatusEnum.Received &&
      SignalRService.signalRNotification.NotificationType == NotificationTypeEnum.ChangeProgressStatus) {
        this.getSentTodotasks();
        SignalRService.signalRNotification.NotificationStatus = NotificationStatusEnum.InProgress;
    }
  }

}

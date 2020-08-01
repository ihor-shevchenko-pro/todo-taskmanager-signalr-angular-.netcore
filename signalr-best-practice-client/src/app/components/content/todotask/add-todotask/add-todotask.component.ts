import { Component, OnInit } from '@angular/core';
import { NgbDateStruct, NgbDatepickerI18n } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TodotaskService } from 'src/core/services/todotask.service';
import { NotificationService } from 'src/core/services/notification.service';
import { ToDoTaskStatusEnum } from 'src/core/models/enum';
import { ISuccessResponse } from 'src/core/interfaces/base/success-response';
import { I18n, CustomDatepickerI18n } from 'src/core/shared/datepicker/datepicker-i18n';

@Component({
  selector: 'app-add-todotask',
  templateUrl: './add-todotask.component.html',
  styleUrls: ['./add-todotask.component.css'],
  providers: [
    // define custom NgbDatepickerI18n provider
    I18n, { provide: NgbDatepickerI18n, useClass: CustomDatepickerI18n },
  ]
})
export class AddTodotaskComponent implements OnInit {

  public model: NgbDateStruct;
  private _toUserId: string;

  constructor(private _formBuilder: FormBuilder,
              private _router: Router,
              private _actRoute: ActivatedRoute,
              private _toDoTaskService: TodotaskService,
              private _notifyService: NotificationService) { }

  ngOnInit(): void {
    this._toUserId = this._actRoute.snapshot.params.touserid;
  }

  // mainForm validator
  public addToDoTaskModel = this._formBuilder.group({
    Title: [null, [Validators.required]],
    Description: [null, [Validators.required]],
    ExpirationDate: [null],
    ProgressStatus: [ToDoTaskStatusEnum.New],
    ToUserId: [null]
  });

  public createToDoTaskHandler() {
    if (this._toUserId != "all") {
      this.createToDoTaskSingleUser();
    } else {
      this.createToDoTaskAllUsers();
    }
  }

  private createToDoTaskSingleUser(): void {
    this.addToDoTaskModel.controls['ToUserId'].setValue(this._toUserId);
    this._toDoTaskService.createToDoTaskSingleUser(this.addToDoTaskModel).subscribe(
      (res: ISuccessResponse) => {
        // console.log(res);
        if (res.response == "success") {
          this._notifyService.showSuccess("Task created successfully", "");
          this._router.navigateByUrl('/home/sent_todotasks');
        }
      },
      errors => {
        // console.log(errors);
        errors.error.errors.forEach(element => {
          switch (element) {
            case 'User recipient of task is not found':
              this._notifyService.showError("User recipient of task is not found", "Error of creating task");
              break;

            default:
              this._notifyService.showError("Error of creating task", "");
              break;
          }
        });
      }
    );
  }

  private createToDoTaskAllUsers(): void {
    this._toDoTaskService.createToDoTaskAllUsers(this.addToDoTaskModel).subscribe(
      (res: ISuccessResponse) => {
        // console.log(res);
        if (res.response == "success") {
          this._notifyService.showSuccess("Task has sent successfully for all users", "");
          this._router.navigateByUrl('/home/sent_todotasks');
        }
      },
      errors => {
        // console.log(errors);
        errors.error.errors.forEach(element => {
          switch (element) {
            case 'User recipient of task is not found':
              this._notifyService.showError("User recipient of task is not found", "Error of creating task");
              break;

            default:
              this._notifyService.showError("Error of creating task", "");
              break;
          }
        });
      }
    );
  }

}

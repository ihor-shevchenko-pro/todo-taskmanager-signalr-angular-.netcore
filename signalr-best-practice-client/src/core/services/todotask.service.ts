import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { FormGroup } from '@angular/forms';
import { AddToDoTaskModel } from '../models/todotask/add-todotask-model';
import { toDoTaskEndpoints } from '../configurations/endpoints-config';
import { ISuccessResponse } from '../interfaces/base/success-response';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { EntitySortingEnum, ToDoTaskStatusEnum } from '../models/enum';
import { IPaginationResponse } from '../interfaces/base/pagination-response';
import { ITodotask } from '../interfaces/todotask/todotask';
import { ToDoTaskChangeProgressstatusModel } from '../models/todotask/todotask-change-progress-status-model';

@Injectable({
  providedIn: 'root'
})
export class TodotaskService {

  private readonly _addToDoTaskSingleUser = toDoTaskEndpoints.addToDoTaskSingleUser;
  private readonly _addToDoTaskAllUsers = toDoTaskEndpoints.addToDoTaskAllUsers;
  private readonly _changeTaskProgressStatus = toDoTaskEndpoints.changeTaskProgressStatus;
  private readonly _getSentTodotasks = toDoTaskEndpoints.getSentTodotask;
  private readonly _getReceivedTodotasks = toDoTaskEndpoints.getReceivedTodotask;

  constructor(private _httpClient: HttpClient) { }

  public createToDoTaskSingleUser(addToDoTaskFormModel: FormGroup): Observable<ISuccessResponse>{
    let addToDoTaskModel = new AddToDoTaskModel();
    addToDoTaskModel.title = addToDoTaskFormModel.value.Title;
    addToDoTaskModel.description = addToDoTaskFormModel.value.Description;
    addToDoTaskModel.progress_status = addToDoTaskFormModel.value.ProgressStatus;
    addToDoTaskModel.to_user_id = addToDoTaskFormModel.value.ToUserId;
    addToDoTaskModel.expiration_date = this.dataConverter(addToDoTaskFormModel.value.ExpirationDate);
    return this._httpClient.post<ISuccessResponse>(this._addToDoTaskSingleUser, addToDoTaskModel);
  }

  public createToDoTaskAllUsers(addToDoTaskFormModel: FormGroup): Observable<ISuccessResponse>{
    let addToDoTaskModel = new AddToDoTaskModel();
    addToDoTaskModel.title = addToDoTaskFormModel.value.Title;
    addToDoTaskModel.description = addToDoTaskFormModel.value.Description;
    addToDoTaskModel.progress_status = addToDoTaskFormModel.value.ProgressStatus;
    addToDoTaskModel.to_user_id = null;
    addToDoTaskModel.expiration_date = this.dataConverter(addToDoTaskFormModel.value.ExpirationDate);
    return this._httpClient.post<ISuccessResponse>(this._addToDoTaskAllUsers, addToDoTaskModel);
  }

  public getSentTodotasks(start: number, count: number, sort: EntitySortingEnum): Observable<IPaginationResponse<ITodotask>>{
    let url = `${this._getSentTodotasks}?start=${start}&count=${count}&sort=${sort}`;
    return this._httpClient.get<IPaginationResponse<ITodotask>>(url);
  }

  public getReceivedTodotasks(start: number, count: number, sort: EntitySortingEnum): Observable<IPaginationResponse<ITodotask>>{
    let url = `${this._getReceivedTodotasks}?start=${start}&count=${count}&sort=${sort}`;
    return this._httpClient.get<IPaginationResponse<ITodotask>>(url);
  }

  public changeTaskProgressStatus(id: string, taskProgressStatus: ToDoTaskStatusEnum): Observable<ISuccessResponse>{
    let model = new ToDoTaskChangeProgressstatusModel();
    model.id = id;
     model.progress_status = taskProgressStatus;
    let url = `${this._changeTaskProgressStatus}/${id}`;
    return this._httpClient.put<ISuccessResponse>(url, model);
  }

  private dataConverter(date: NgbDateStruct): Date {
    return date ? new Date(Date.UTC(date.year, date.month - 1, date.day, 0, 0, 0)) : null;
  }
  
}

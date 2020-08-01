import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { userEndpoints } from '../configurations/endpoints-config';
import { Observable } from 'rxjs';
import { IUser } from '../interfaces/user/user';
import { EntitySortingEnum } from '../models/enum';
import { IPaginationResponse } from '../interfaces/base/pagination-response';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  public static currentUser: IUser = null;
  private readonly _getCurrentUser = userEndpoints.getCurrentUser;
  private readonly _getUsers = userEndpoints.getUsers;

  constructor(private _httpClient: HttpClient) { }

  public getCurrentUser(): Observable<IUser>{
    return this._httpClient.get<IUser>(this._getCurrentUser);
  }

  public getUsers(start: number, count: number, sort: EntitySortingEnum): Observable<IPaginationResponse<IUser>>{
    let url = `${this._getUsers}?start=${start}&count=${count}&sort=${sort}`;
    return this._httpClient.get<IPaginationResponse<IUser>>(url);
  }

  public setCurrentUserData(user: IUser): void {
    UserService.currentUser = user;
  }

}

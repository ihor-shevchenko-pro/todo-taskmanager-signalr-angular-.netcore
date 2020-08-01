import { Injectable } from '@angular/core';
import { signalRHubEndpoints } from '../configurations/endpoints-config';
import { AccountService } from './account.service';
import * as signalR from '@microsoft/signalr';
import { ISignalRNotificationModel } from '../interfaces/signalr/signalr-notification-model';

@Injectable({
  providedIn: 'root'
})
export abstract class SignalRService {
  public static isConnected: boolean = false;
  public static signalRNotification: ISignalRNotificationModel = null;
  private static _connection: any = null;
  private readonly _hubEndpoint = signalRHubEndpoints.hubEndpoint;

  constructor(private _accountService: AccountService) { }

  public startSignalRConnection(): void {
    const accessToken = this._accountService.getBearerToken();  
    //build connection
    SignalRService._connection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Information)
    .withUrl(this._hubEndpoint, { accessTokenFactory: () => accessToken })
    .build();
    // start connection
    SignalRService._connection.start()
    .then(() => console.log('SignalR hub connected!'))
    .catch(errors => console.error(errors));
    SignalRService.isConnected = true;
  }

  public closeSignalRConnection(): void {
    SignalRService._connection.stop()
    .then(() => console.log('SignalR hub disconnected!'))
    .catch(errors => console.error(errors));
    SignalRService.isConnected = false;
    SignalRService._connection = null;
    SignalRService.signalRNotification = null;
  }

  public handleSignalRNotification(): void {
    SignalRService._connection.on('HandleNotification', (notificationModel: ISignalRNotificationModel) => {
      SignalRService.signalRNotification = notificationModel;
    });
  }

}

import * as signalR from '@microsoft/signalr';
import { signalRHubEndpoints } from '../configurations/endpoints-config';
import { AccountService } from '../services/account.service';

export abstract class SignalrManager {

  public _signalRNotification: any = null;
  private _connection: any = null;
  private readonly _hubEndpoint = signalRHubEndpoints.hubEndpoint;

  constructor(private _accountService: AccountService) { }

  public initializeHubConnection(){
    this.buildConnection();
    this.startConnection();
  }

  private buildConnection(): void {
    const accessToken = this._accountService.getBearerToken();  
    this._connection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Information)
    .withUrl(this._hubEndpoint, { accessTokenFactory: () => accessToken })
    .build();
  }

  private startConnection(): void {
    this._connection.start()
    .then(() => console.log('SignalR hub connected!'))
    .catch(errors => console.error(errors));
  }

  public terminateConnection(): void {
    this._connection.stop()
    .then(() => console.log('SignalR hub disconnected!'))
    .catch(errors => console.error(errors));
  }

  public handleNotification(): void {
    this._connection.on('HandleNotification', (notificationModel: any) => {
    //   console.log(notificationModel);
    this._signalRNotification = notificationModel;
    });
  }

}

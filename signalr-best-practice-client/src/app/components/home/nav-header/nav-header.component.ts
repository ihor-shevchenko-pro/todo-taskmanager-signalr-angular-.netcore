import { Component, OnInit, Input } from '@angular/core';
import { AccountService } from 'src/core/services/account.service';
import { SignalRService } from 'src/core/services/signal-r.service';

@Component({
  selector: 'app-nav-header',
  templateUrl: './nav-header.component.html',
  styleUrls: ['./nav-header.component.css']
})
export class NavHeaderComponent implements OnInit {

  @Input() public userName: string;

  constructor(private _accountService: AccountService,
              private _signalRService: SignalRService) { }

  ngOnInit(): void {
  }

  public onLogout(): void {
    // try to start SignalRConnection
    if(SignalRService.isConnected == true){
      this._signalRService.closeSignalRConnection();
    }
    // logout
    this._accountService.onLogout();
  }

}

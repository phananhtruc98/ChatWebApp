import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { User, UserProfile } from '../_models/user';
import { Subject } from 'rxjs/internal/Subject';
import { Observable } from 'rxjs';
import * as signalR from '@microsoft/signalr';

export class SignalRService {
  private hubConnection: any;
  private $allFeed: Subject<UserProfile> = new Subject<UserProfile>();
  public startConnection() {
    return new Promise((resolve, reject) => {
      this.hubConnection = new HubConnectionBuilder()
        .withUrl('https://localhost:44305/accountHub', {
          skipNegotiation: true,
          transport: signalR.HttpTransportType.WebSockets,
        })
        .build();

      this.hubConnection
        .start()
        .then(() => {
          console.log('connection established');
          return resolve(true);
        })
        .catch((err: any) => {
          console.log('error occured' + err);
          reject(err);
        });
    });
  }

  constructor() {}

  public get AllContactsObservable(): Observable<any> {
    return this.$allFeed.asObservable();
  }

  public listenToUpdateProfile() {
    (<HubConnection>this.hubConnection).on('UpdateProfile', (res) => {
      this.$allFeed.next(res);
    });
  }
}

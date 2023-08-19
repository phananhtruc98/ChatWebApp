import { HubConnection } from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { UserProfile } from '../_models/user';
import { Subject } from 'rxjs/internal/Subject';
import { Observable } from 'rxjs';
import * as signalR from '@microsoft/signalr';
import { Injectable } from '@angular/core';
import { Message } from '../_models/message';
@Injectable()
export class SignalRService {
  private accountHubConnection: HubConnection;
  private chatHubConnection: HubConnection;
  private $allFeed: Subject<UserProfile> = new Subject<UserProfile>();
  private $message: Subject<Message> = new Subject<Message>();
  constructor() {
    this.chatHubConnection= new signalR.HubConnectionBuilder()
    .withUrl(environment.apiUrl + '/chatHub')
    .build();
    this.chatHubConnection.start().then(() => {
      console.log('Chat Hub connected:' + this.chatHubConnection.connectionId);
      this.listenToNewMessage();
    })
    this.accountHubConnection= new signalR.HubConnectionBuilder()
    .withUrl(environment.apiUrl + '/accountHub')
    .build();
    this.accountHubConnection.start().then(() => {
      console.log('Account Hub connected:' + this.accountHubConnection.connectionId);
      this.listenToUpdateProfile();
    })
  }

  public closeConnection() {
    this.accountHubConnection?.stop();
  }

  public get AllContactsObservable(): Observable<any> {
    return this.$allFeed.asObservable();
  }

  public listenToUpdateProfile() {
    (<HubConnection>this.accountHubConnection).on('UpdateProfile', (res) => {
      this.$allFeed.next(res);
    });
  }

  public listenToNewMessage() {
    (<HubConnection>this.chatHubConnection).on('SendMessage', (res) => {
      this.$message.next(res);
    });
  }
  public get MessageObservable(): Observable<any> {
    return this.$message.asObservable();
  }
}

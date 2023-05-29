import { Router } from '@angular/router';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { LoginUser, User } from '../_models/user';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AccountService } from './account.service';
import { UserContactForCreation } from '../_models/user-contact.model';
@Injectable({
  providedIn: 'root',
})
export class UserContactService {
  constructor(private http: HttpClient) {}
  createUserContact(userContact: UserContactForCreation) {
    return this.http.post(`${environment.apiUrl}/userContact`, userContact);
  }
  public getContacts(): Observable<User[]> {
    return this.http.get<User[]>(`${environment.apiUrl}/userContact`);
  }
}

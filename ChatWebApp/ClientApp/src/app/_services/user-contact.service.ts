import { Observable } from 'rxjs';
import { UserProfile } from '../_models/user';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserContactForCreation } from '../_models/user-contact.model';
@Injectable({
  providedIn: 'root',
})
export class UserContactService {
  constructor(private http: HttpClient) {}
  createUserContact(userContact: UserContactForCreation) {
    return this.http.post(`${environment.apiUrl}/userContact`, userContact);
  }
  public getContacts(): Observable<UserProfile[]> {
    return this.http.get<UserProfile[]>(`${environment.apiUrl}/userContact`);
  }
}

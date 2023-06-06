import { Component } from '@angular/core';
import { UserProfile } from 'src/app/_models/user';
import { UserContactService } from 'src/app/_services/user-contact.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.sass'],
})
export class MessagesComponent {
  userContacts!: UserProfile[];
  selectedContact!: UserProfile;
  constructor(private _userContactService: UserContactService) {
    this.getContacts();
  }

  getContacts() {
    this._userContactService.getContacts().subscribe((rs) => {
      this.userContacts = rs;
    });
  }
  selectContact(user: any) {
    console.log(user);
    this.selectedContact = user;
    console.log(user);
  }
  createChat() {}
}

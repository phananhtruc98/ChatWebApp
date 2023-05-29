import { Component } from '@angular/core';
import { map } from 'rxjs';
import { User } from 'src/app/_models/user';
import { UserContactService } from 'src/app/_services/user-contact.service';

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrls: ['./contacts.component.sass'],
})
export class ContactsComponent {
  userContacts!: User[];
  constructor(private _userContactService: UserContactService) {
    this.getContacts();
  }

  getContacts() {
    this._userContactService.getContacts().subscribe((rs) => {
      this.userContacts = rs;
    });
  }
}

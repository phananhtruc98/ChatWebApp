import { Component } from '@angular/core';
import { map } from 'rxjs';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { UserContactService } from 'src/app/_services/user-contact.service';

@Component({
  selector: 'app-suggestions',
  templateUrl: './suggestions.component.html',
  styleUrls: ['./suggestions.component.sass'],
})
export class SuggestionsComponent {
  user: User | null;
  users: User[] | null | undefined;

  constructor(
    private accountService: AccountService,
    private userContactService: UserContactService
  ) {
    this.user = this.accountService.userValue;
    this.getSuggestions();
  }

  getSuggestions() {
    this.accountService
      .getSuggestions()
      .pipe(
        map((response) => {
          if (response) {
            console.log(response);
            this.users = response;
          }
        })
      )
      .subscribe();
  }

  addContact(contactId: any) {
    console.log(contactId);
    var userId;
    this.accountService.user?.subscribe((user) => {
      userId = user?.id;
    });
    var userContactForCreation = { userId: userId, contactId: contactId };
    this.userContactService
      .createUserContact(userContactForCreation)
      .pipe(
        map((response) => {
          console.log(response);
        })
      )
      .subscribe();
  }
}

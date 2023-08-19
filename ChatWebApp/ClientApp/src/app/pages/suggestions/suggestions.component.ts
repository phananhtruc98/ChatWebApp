import { Component } from '@angular/core';
import { map } from 'rxjs';
import { User, UserProfile } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { UserContactService } from 'src/app/_services/user-contact.service';

@Component({
  selector: 'app-suggestions',
  templateUrl: './suggestions.component.html',
  styleUrls: ['./suggestions.component.sass'],
})
export class SuggestionsComponent {
  user: User | null;
  users: UserProfile[] | null | undefined;
  selectedSuggestion: UserProfile | undefined;
  isAddingContact: boolean = false;
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
            this.users = response;
          }
        })
      )
      .subscribe();
  }

  addContact(contactId: any) {
    this.isAddingContact = true;
    let userId;
    this.accountService.user?.subscribe((user) => {
      userId = user?.id;
    });
    const userContactForCreation = { userId: userId, contactId: contactId };
    this.userContactService
      .createUserContact(userContactForCreation)
      .subscribe(() => {
        this.isAddingContact = false;
        this.selectedSuggestion = undefined;
        alert('Add contact successfully');
      });
  }
  selectSuggestion(user: UserProfile) {
    this.selectedSuggestion = user;
  }
}

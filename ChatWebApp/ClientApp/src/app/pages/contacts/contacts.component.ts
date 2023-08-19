import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FirstMessageForCreation } from 'src/app/_models/message';
import { User, UserProfile } from 'src/app/_models/user';
import { ConversationService } from 'src/app/_services/conversation.service';
import { SignalRService } from 'src/app/_services/signalr.service';
import { UserContactService } from 'src/app/_services/user-contact.service';

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html',
  styleUrls: ['./contacts.component.sass'],
})
export class ContactsComponent {
  userContacts!: UserProfile[];
  selectedContact!: UserProfile;
  currentUser!: User;
  constructor(
    private _userContactService: UserContactService,
    private _signalrService: SignalRService,
    private _conversationService: ConversationService,
    private route: ActivatedRoute,
    private router: Router,
  ) {
    this.getContacts();
    this.currentUser = JSON.parse(localStorage.getItem('user')!)
  }
  ngOnInit() {
      this._signalrService.AllContactsObservable.subscribe((res: any) => {
        if (res.id == this.selectedContact?.id) {
          this.selectedContact = res;
        }
      });
  }
  getContacts() {
    this._userContactService.getContacts().subscribe((rs) => {
      this.userContacts = rs;
    });
  }
  selectContact(user: any) {
    this.selectedContact = user;
  }

  createChat() {
    const newMessage: FirstMessageForCreation = {};
    newMessage.content = "Hi";
    newMessage.name = this.currentUser.fullName + ", " + this.selectedContact.fullName;
    if(this.selectedContact.id && this.currentUser.id){
      newMessage.participants = [];
      newMessage.participants.push(this.selectedContact.id, this.currentUser.id);
      newMessage.sender = this.currentUser.id;
    }
    this._conversationService.createFirstMessage(newMessage).subscribe((res)=>{
      this.router.navigate(['/messages', { selectedConversationId: res?.conversationId }]);
    });
  }
}

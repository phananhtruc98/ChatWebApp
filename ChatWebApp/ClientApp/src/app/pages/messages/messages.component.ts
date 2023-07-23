import { Component, ElementRef, EventEmitter, ViewChild } from '@angular/core';
import { async } from 'rxjs';
import {
  Conversation,
  ConversationDto,
  ConversationInfoDto,
} from 'src/app/_models/conversation';
import { FirstMessageForCreation, Message } from 'src/app/_models/message';
import { User, UserProfile } from 'src/app/_models/user';
import { ConversationService } from 'src/app/_services/conversation.service';
import { UserContactService } from 'src/app/_services/user-contact.service';
import { ConversationDetailDialogComponent } from 'src/app/components/conversation-detail-dialog/conversation-detail-dialog.component';
import {
  MatDialog,
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogModule,
} from '@angular/material/dialog';
import { SignalRService } from 'src/app/_services/signalr.service';
@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.sass'],
})
export class MessagesComponent {
  @ViewChild('scrollMe')
  private myScrollContainer!: ElementRef;
  userContacts!: UserProfile[];
  selectedConversation!: Conversation;
  currentUser!: User;
  conversations!: ConversationDto[];
  currentMessages!: Message[];
  currentText!: string;
  currentParticipantId?: string;
  currentConversationDetail?: ConversationInfoDto;
  public messageReceived: EventEmitter<Message>;
  constructor(
    private _userContactService: UserContactService,
    private _conversationService: ConversationService,
    public dialog: MatDialog,
    private _signalrService: SignalRService
  ) {
    this.currentUser = JSON.parse(localStorage.getItem('user')!);
    this.messageReceived = new EventEmitter<Message>();
  }

  ngOnInit() {
    this.getConversations();
    this._signalrService.MessageObservable.subscribe((res: any) => {
      console.log(res);
      const conv: ConversationDto | undefined = this.conversations.find(
        (x) => x.id == res.conversationId
      );
      if (conv) {
        conv.lastMessage = res.content;
        conv.lastSender = res.createdBy.fullName;
        conv.lastSent = res.createdDate;
      }
      if (this.selectedConversation?.id == conv?.id) {
        this.currentMessages.push({
          content: res.content,
          conversationParticipantId: res.conversationParticipantId,
          createdDate: res.createdDate,
          createdBy: res.createdBy
        });
      }
      this.getConversations();
      this.scrollToBottom();
    });
  }
  ngAfterViewChecked() {
    this.scrollToBottom();
}

scrollToBottom(): void {
    try {
        this.myScrollContainer.nativeElement.scrollTop = this.myScrollContainer.nativeElement.scrollHeight;
    } catch(err) { }
}
  getContacts() {
    this._userContactService.getContacts().subscribe((rs) => {
      this.userContacts = rs;
    });
  }
  getConversations() {
    this._conversationService.getConversations().subscribe((rs) => {
      this.conversations = rs.sort((a, b) => {
        return +new Date(b.lastSent) - +new Date(a.lastSent);
      });
    });
  }
  selectConversation(conversation: any): void {
    this.selectedConversation = conversation;
    if (this.selectedConversation.id) {
      this._conversationService
        .getConversation(this.selectedConversation.id)
        .subscribe((rs) => {
          console.log(rs);
          this.currentConversationDetail = rs;
          this.getMessages(conversation.id);
          this.currentParticipantId = rs.participants?.find(
            (x) => x.userId == this.currentUser.id
          )?.id;
          console.log(this.currentParticipantId);
        });
    }
  }
  getMessages(conversationId: string) {
    this._conversationService.getMessages(conversationId).subscribe((rs) => {
      this.currentMessages = rs;
      console.log(this.currentMessages);
    });
  }
  sendMessage() {
    console.log(this.currentText);
    let newMessage: Message = {};
    if (this.currentText) {
      newMessage.content = this.currentText;
      newMessage.conversationParticipantId = this.currentParticipantId;
    }
    this._conversationService.sendMessage(newMessage).subscribe((rs) => {
      console.log('Send successfully');
      this.currentText = '';
    });
  }
  isSentByCurrentUser(userId: any) {
    return userId === this.currentUser.id;
  }

  conversationDetails() {
    let dialogRef = this.dialog.open(ConversationDetailDialogComponent, {
      data: this.currentConversationDetail,
    });
  }
  isToday(dateString: string): boolean {
    const today = new Date();
    const date = new Date(dateString);
    return today.toDateString() == date.toDateString();
  }
}

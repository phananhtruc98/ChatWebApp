import { HttpClient } from '@angular/common/http';
import { FirstMessageForCreation, Message } from '../_models/message';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Injectable } from '@angular/core';
import { Conversation, ConversationDto, ConversationInfoDto } from '../_models/conversation';
@Injectable()
export class ConversationService {
  constructor(private http: HttpClient) {}

  public createFirstMessage(firstMessage: FirstMessageForCreation): Observable<Message> {
    return this.http.post<Message>(`${environment.apiUrl}/Conversation/first-message`, firstMessage);
  }

  public getConversations(){
    return this.http.get<ConversationDto[]>(`${environment.apiUrl}/Conversation/conversations`);
  }

  public getMessages(conversationId: string){
    return this.http.get<Message[]>(`${environment.apiUrl}/Conversation/${conversationId}/messages`);
  }
  public sendMessage(message: Message){
    return this.http.post<Message>(`${environment.apiUrl}/Conversation/message`, message);
  }
  public getConversation(id: string){
    return this.http.get<ConversationInfoDto>(`${environment.apiUrl}/Conversation/${id}`);
  }
}

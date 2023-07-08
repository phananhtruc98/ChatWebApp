import { ConversationParticipant } from "./conversation-participant.model";

export class Conversation {
  id?: string;
  name?: string;
  avatar?: string;
  createdDate!: Date;
  modifiedDate?: Date;
  createdBy?: string;
  modifiedBy?: string;
}
export class ConversationDto {
  id?: string;
  name?: string;
  avatar?: string;
  lastMessage?: string;
  lastSender?: string;
  lastSent!: Date;
}
export class ConversationInfoDto{
  id?: string;
  name?: string;
  avatar?: string;
  participants?: ConversationParticipant[];
}

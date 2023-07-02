import { UserProfile } from "./user";

export class Message {
  id?: string;
  content?: string;
  conversationParticipantId?: string;
  createdDate?: Date;
  modifiedDate?: Date;
  createdBy?: UserProfile;
}

export class FirstMessageForCreation
{
  content?: string;
  name?: string;
  participants?: string[];
  avatar?: string;
  sender?: string;
}

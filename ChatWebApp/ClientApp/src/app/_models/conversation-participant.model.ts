import { UserProfile } from "./user";

export class ConversationParticipant{
  id?: string;
  userId?: string;
  conversationId?: string;
  user? : UserProfile;
}

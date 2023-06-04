export class User {
  id?: string;
  email?: string;
  password?: string;
  fullName?: string;
  token?: string;
}
export class LoginUser {
  email!: string;
  password!: string;
}
export class UserProfile {
  id?: string;
  fullName?: string;
  bio?: string;
  avatar?: string;
  email?: string;
  isFemale?: boolean;
  dateOfBirth?: Date;
}

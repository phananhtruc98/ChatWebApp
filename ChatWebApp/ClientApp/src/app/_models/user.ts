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
  fullName?: string;
  bio?: string;
  avatar?: string;
  email?: string;
}

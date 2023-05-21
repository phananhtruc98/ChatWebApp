export class User {
  id?: string;
  email?: string;
  password?: string;
  firstName?: string;
  lastName?: string;
  token?: string;
}
export class LoginUser {
  email!: string;
  password!: string;
}

import {
  CanActivate,
  Router,
  UrlTree,
} from '@angular/router';
import { Observable } from 'rxjs';
import { AccountService } from './account.service';
import { Injectable } from '@angular/core';
@Injectable()
export class AuthGuardService implements CanActivate {
  constructor(private router: Router, private account: AccountService) {}
  canActivate(
  ):
    | boolean
    | UrlTree
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree> {
    if (!this.account.isUserLoggedIn()) {
      alert(
        'You are not allowed to view this page. Please login to view this page!!!'
      );
      this.router.navigate(['/login']);
    }
    return true;
  }
}

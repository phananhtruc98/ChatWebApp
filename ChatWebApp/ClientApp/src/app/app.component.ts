import { Component } from '@angular/core';
import { AccountService } from './_services/account.service';
import { User } from 'src/app/_models/user'
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.sass'],
})
export class AppComponent {
  title = 'ClientApp';  
  user?: User | null;
  constructor(private accountService: AccountService) {
    this.accountService.user.subscribe((x) => (this.user = x));
  }

  logout() {
    this.accountService.logout();
  }
}

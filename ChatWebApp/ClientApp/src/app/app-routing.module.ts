import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { RegisterComponent } from './account/register/register.component';
import { LoginComponent } from './account/login/login.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { ContactsComponent } from './pages/contacts/contacts.component';
import { SuggestionsComponent } from './pages/suggestions/suggestions.component';
import { MessagesComponent } from './pages/messages/messages.component';
import { AuthGuardService } from './_services/auth-guard.service';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'login', component: LoginComponent },
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [AuthGuardService],
  },
  {
    path: 'contacts',
    component: ContactsComponent,
    canActivate: [AuthGuardService],
  },
  {
    path: 'suggestions',
    component: SuggestionsComponent,
    canActivate: [AuthGuardService],
  },
  {
    path: 'messages',
    component: MessagesComponent,
    canActivate: [AuthGuardService],
  },
  // { path: 'users', loadChildren: usersModule, canActivate: [AuthGuard] },
  // { path: 'account', loadChildren: accountModule },

  // otherwise redirect to home
  { path: '**', redirectTo: '' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}

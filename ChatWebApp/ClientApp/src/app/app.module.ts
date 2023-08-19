import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { RegisterComponent } from './account/register/register.component';
import { HomeComponent } from './pages/home/home.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './account/login/login.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { ContactsComponent } from './pages/contacts/contacts.component';
import { AuthInterceptor } from './_helpers/auth.interceptor';
import { ImagekitioAngularModule } from 'imagekitio-angular';
import { environment } from 'src/environments/environment';
import { SuggestionsComponent } from './pages/suggestions/suggestions.component';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatNativeDateModule } from '@angular/material/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MessagesComponent } from './pages/messages/messages.component';
import { SignalRService } from './_services/signalr.service';
import { AuthGuardService } from './_services/auth-guard.service';
import { ConversationService } from './_services/conversation.service';
import { ConversationDetailDialogComponent } from './components/conversation-detail-dialog/conversation-detail-dialog.component';
import { MatDialogModule} from '@angular/material/dialog';
import { DateAgoPipe } from './pipes/date-ago.pipe';
@NgModule({
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    ImagekitioAngularModule.forRoot({
      publicKey: environment.publicKey,
      urlEndpoint: environment.urlEndpoint,
    }),
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    BrowserAnimationsModule,
    MatCheckboxModule,
    MatButtonModule,
    MatSidenavModule,
    MatProgressSpinnerModule,
    MatIconModule,
    MatDialogModule
  ],
  declarations: [
    AppComponent,
    RegisterComponent,
    HomeComponent,
    LoginComponent,
    ProfileComponent,
    ContactsComponent,
    SuggestionsComponent,
    MessagesComponent,
    ConversationDetailDialogComponent,
    DateAgoPipe,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    { provide: SignalRService },
    { provide: AuthGuardService },
    { provide: ConversationService },
  ],
  bootstrap: [AppComponent],
  exports: [CommonModule, FormsModule, ReactiveFormsModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule {}
